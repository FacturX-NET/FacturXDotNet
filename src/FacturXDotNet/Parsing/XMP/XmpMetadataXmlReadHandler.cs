using System.Globalization;
using FacturXDotNet.Models.XMP;
using FacturXDotNet.Parsing.XMP.Exceptions;
using Microsoft.Extensions.Logging;
using TurboXml;

namespace FacturXDotNet.Parsing.XMP;

struct XmpMetadataXmlReadHandler(XmpMetadata result, ILogger? logger) : IXmlReadHandler
{
    readonly Stack<ReadOnlyMemory<char>> _pathStack = [];
    bool _nextLanguageAlternativeIsDefault;

    /// <summary>
    ///     True when the value of <see cref="_pdfaSchemaPrefix" /> is the prefix that should be used to parse the <see cref="XmpFacturXMetadata" />.
    /// </summary>
    bool _schemaPrefixIsFacturXSchemaPrefix;

    /// <summary>
    ///     A prefix that was found in a PDF/A Schema declaration. When <see cref="_schemaPrefixIsFacturXSchemaPrefix" /> is true, this value should not be edited anymore
    ///     and should be the one to look for to find the data for <see cref="XmpFacturXMetadata" />.
    /// </summary>
    ReadOnlyMemory<char> _pdfaSchemaPrefix = ReadOnlyMemory<char>.Empty;

    public void OnBeginTag(ReadOnlySpan<char> name, int line, int column)
    {
        ReadOnlyMemory<char> parentPath = _pathStack.TryPeek(out ReadOnlyMemory<char> p) ? p : ReadOnlyMemory<char>.Empty;
        string newPath = $"{parentPath}/{name}";
        _pathStack.Push(newPath.AsMemory());
        name.ToString().AsMemory();

        try
        {
            HandleTag(newPath);
        }
        catch (Exception exception)
        {
            throw XmpMetadataReaderException.ParsingError(newPath, line, column, exception);
        }
    }

    public void OnEndTagEmpty() => _pathStack.Pop();
    public void OnEndTag(ReadOnlySpan<char> name, int line, int column) => _pathStack.Pop();

    public void OnAttribute(ReadOnlySpan<char> name, ReadOnlySpan<char> value, int nameLine, int nameColumn, int valueLine, int valueColumn)
    {
        if (!_pathStack.TryPeek(out ReadOnlyMemory<char> path) || value.IsWhiteSpace())
        {
            return;
        }

        if (HandleLanguageAlternative(path.Span, name, value))
        {
            return;
        }

        string attributePath = $"{path}/{name}";
        try
        {
            Handle(attributePath, value);
        }
        catch (Exception exception)
        {
            throw XmpMetadataReaderException.ParsingError(attributePath, valueLine, valueColumn, exception);
        }
    }

    public void OnText(ReadOnlySpan<char> value, int line, int column)
    {
        if (!_pathStack.TryPeek(out ReadOnlyMemory<char> path) || value.IsWhiteSpace())
        {
            return;
        }

        try
        {
            Handle(path.Span, value);
        }
        catch (Exception exception)
        {
            throw XmpMetadataReaderException.ParsingError(path.Span, line, column, exception);
        }
    }

    public void OnError(string message, int line, int column)
    {
        string path = _pathStack.TryPeek(out ReadOnlyMemory<char> p) ? p.ToString() : string.Empty;
        throw XmpMetadataReaderException.ParsingError(path, line, column, message);
    }

    void HandleTag(string path)
    {
        switch (path)
        {
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:Thumbnails":
                CreateBasicMetadata();
                result.Basic!.Thumbnails.Add(new XmpThumbnail());
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li":
                CreatePdfAExtensionsMetadata();
                result.PdfAExtensions!.Schemas.Add(new XmpPdfASchemaMetadata());
                // if we already found the prefix for the FacturX metadata, we don't reset the prefix
                if (!_schemaPrefixIsFacturXSchemaPrefix)
                {
                    // reset the prefix so that it can be set by the current schema
                    _pdfaSchemaPrefix = ReadOnlyMemory<char>.Empty;
                }
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:property/rdf:Seq/rdf:li":
                result.PdfAExtensions!.Schemas[^1].Property.Add(new XmpPdfAPropertyMetadata());
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:valueType/rdf:Seq/rdf:li":
                result.PdfAExtensions!.Schemas[^1].ValueType.Add(new XmpPdfATypeMetadata());
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:valueType/rdf:Seq/rdf:li/pdfaType:field/rdf:Seq/rdf:li":
                result.PdfAExtensions!.Schemas[^1].ValueType[^1].Field.Add(new XmpPdfAFieldMetadata());
                break;
        }
    }

    void Handle(ReadOnlySpan<char> path, ReadOnlySpan<char> value)
    {
        switch (path)
        {
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmlns:pdfaid":
                CreatePdfAIdentificationMetadata();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaid:part":
                result.PdfAIdentification!.Part = ParseInt(value);
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaid:conformance":
                result.PdfAIdentification!.Conformance = value.ToXmpPdfAConformanceLevel();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaid:amd":
                result.PdfAIdentification!.Amendment = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmlns:xmp":
                CreateBasicMetadata();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:Identifier/rdf:Bag/rdf:li":
                result.Basic!.Identifier.Add(value.ToString());
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:CreateDate":
                result.Basic!.CreateDate = ParseDateOffset(value);
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:CreatorTool":
                result.Basic!.CreatorTool = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:Label":
                result.Basic!.Label = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:MetadataDate":
                result.Basic!.MetadataDate = ParseDateOffset(value);
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:ModifyDate":
                result.Basic!.ModifyDate = ParseDateOffset(value);
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:Rating":
                result.Basic!.Rating = ParseDouble(value);
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:BaseURL":
                result.Basic!.BaseUrl = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:Nickname":
                result.Basic!.Nickname = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:Thumbnails/rdf:Alt/rdf:li/xmpGlmg:format":
                result.Basic!.Thumbnails[^1].Format = value.ToXmpThumbnailFormat();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:Thumbnails/rdf:Alt/rdf:li/xmpGlmg:height":
                result.Basic!.Thumbnails[^1].Height = ParseInt(value);
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:Thumbnails/rdf:Alt/rdf:li/xmpGlmg:width":
                result.Basic!.Thumbnails[^1].Width = ParseInt(value);
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:Thumbnails/rdf:Alt/rdf:li/xmpGlmg:image":
                result.Basic!.Thumbnails[^1].Image = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmlns:pdf":
                CreatePdfMetadata();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdf:Keywords":
                result.Pdf!.Keywords = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdf:PDFVersion":
                result.Pdf!.PdfVersion = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdf:Producer":
                result.Pdf!.Producer = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdf:Trapped":
                result.Pdf!.Trapped = ParseBool(value);
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmlns:dc":
                CreateDublinCoreMetadata();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/dc:contributor/rdf:Bag/rdf:li":
                result.DublinCore!.Contributor.Add(value.ToString());
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/dc:coverage":
                result.DublinCore!.Coverage = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/dc:creator/rdf:Seq/rdf:li":
                result.DublinCore!.Creator.Add(value.ToString());
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/dc:date/rdf:Seq/rdf:li":
                result.DublinCore!.Date.Add(ParseDateOffset(value));
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/dc:description/rdf:Alt/rdf:li":
                if (_nextLanguageAlternativeIsDefault)
                {
                    result.DublinCore!.Description.Insert(0, value.ToString());
                    _nextLanguageAlternativeIsDefault = false;
                }
                else
                {
                    result.DublinCore!.Description.Add(value.ToString());
                }
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/dc:format":
                result.DublinCore!.Format = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/dc:identifier":
                result.DublinCore!.Identifier = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/dc:language/rdf:Bag/rdf:li":
                result.DublinCore!.Language.Add(value.ToString());
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/dc:publisher/rdf:Bag/rdf:li":
                result.DublinCore!.Publisher.Add(value.ToString());
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/dc:relation/rdf:Bag/rdf:li":
                result.DublinCore!.Relation.Add(value.ToString());
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/dc:rights/rdf:Alt/rdf:li":
                if (_nextLanguageAlternativeIsDefault)
                {
                    result.DublinCore!.Rights.Insert(0, value.ToString());
                    _nextLanguageAlternativeIsDefault = false;
                }
                else
                {
                    result.DublinCore!.Rights.Add(value.ToString());
                }
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/dc:source":
                result.DublinCore!.Source = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/dc:subject/rdf:Bag/rdf:li":
                result.DublinCore!.Subject.Add(value.ToString());
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/dc:title/rdf:Alt/rdf:li":
                if (_nextLanguageAlternativeIsDefault)
                {
                    result.DublinCore!.Title.Insert(0, value.ToString());
                    _nextLanguageAlternativeIsDefault = false;
                }
                else
                {
                    result.DublinCore!.Title.Add(value.ToString());
                }
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/dc:type/rdf:Bag/rdf:li":
                result.DublinCore!.Type.Add(value.ToString());
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:namespaceURI":
                result.PdfAExtensions!.Schemas[^1].NamespaceUri = value.ToString();
                if (value is XmpFacturXMetadata.NamespaceUri)
                {
                    _schemaPrefixIsFacturXSchemaPrefix = true;
                }
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:prefix":
                result.PdfAExtensions!.Schemas[^1].Prefix = value.ToString();
                if (_pdfaSchemaPrefix.Length == 0)
                {
                    _pdfaSchemaPrefix = value.ToString().AsMemory();
                }
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:schema":
                result.PdfAExtensions!.Schemas[^1].Schema = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:property/rdf:Seq/rdf:li/pdfaProperty:category":
                result.PdfAExtensions!.Schemas[^1].Property[^1].Category = value.ToXmpPdfAPropertyCategory();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:property/rdf:Seq/rdf:li/pdfaProperty:description":
                result.PdfAExtensions!.Schemas[^1].Property[^1].Description = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:property/rdf:Seq/rdf:li/pdfaProperty:name":
                result.PdfAExtensions!.Schemas[^1].Property[^1].Name = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:property/rdf:Seq/rdf:li/pdfaProperty:valueType":
                result.PdfAExtensions!.Schemas[^1].Property[^1].ValueType = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:valueType/rdf:Seq/rdf:li/pdfaType:description":
                result.PdfAExtensions!.Schemas[^1].ValueType[^1].Description = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:valueType/rdf:Seq/rdf:li/pdfaType:namespaceURI":
                result.PdfAExtensions!.Schemas[^1].ValueType[^1].NamespaceUri = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:valueType/rdf:Seq/rdf:li/pdfaType:prefix":
                result.PdfAExtensions!.Schemas[^1].ValueType[^1].Prefix = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:valueType/rdf:Seq/rdf:li/pdfaType:type":
                result.PdfAExtensions!.Schemas[^1].ValueType[^1].Type = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:valueType/rdf:Seq/rdf:li/pdfaType:field/rdf:Seq/rdf:li/pdfaField:description":
                result.PdfAExtensions!.Schemas[^1].ValueType[^1].Field[^1].Description = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:valueType/rdf:Seq/rdf:li/pdfaType:field/rdf:Seq/rdf:li/pdfaField:name":
                result.PdfAExtensions!.Schemas[^1].ValueType[^1].Field[^1].Name = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:valueType/rdf:Seq/rdf:li/pdfaType:field/rdf:Seq/rdf:li/pdfaField:valueType":
                result.PdfAExtensions!.Schemas[^1].ValueType[^1].Field[^1].ValueType = value.ToString();
                break;

            default:
                // dynamic cases
                if (path.SequenceEqual($"/x:xmpmeta/rdf:RDF/rdf:Description/xmlns:{_pdfaSchemaPrefix}"))
                {
                    CreateFacturXMetadata();
                }
                else if (path.SequenceEqual($"/x:xmpmeta/rdf:RDF/rdf:Description/{_pdfaSchemaPrefix}:DocumentFileName"))
                {
                    result.FacturX!.DocumentFileName = value.ToString();
                }
                else if (path.SequenceEqual($"/x:xmpmeta/rdf:RDF/rdf:Description/{_pdfaSchemaPrefix}:DocumentType"))
                {
                    result.FacturX!.DocumentType = value.ToFacturXDocumentType();
                }
                else if (path.SequenceEqual($"/x:xmpmeta/rdf:RDF/rdf:Description/{_pdfaSchemaPrefix}:Version"))
                {
                    result.FacturX!.Version = value.ToString();
                }
                else if (path.SequenceEqual($"/x:xmpmeta/rdf:RDF/rdf:Description/{_pdfaSchemaPrefix}:ConformanceLevel"))
                {
                    result.FacturX!.ConformanceLevel = value.ToXmpFacturXConformanceLevel();
                }

                logger?.LogWarning("Unknown element '{Path}' with value '{Value}'.", path.ToString(), value.ToString());
                break;
        }
    }

    /// <summary>
    ///     The language alternative type is a list of strings where one item of the collection can have the xml:lang attribute set to "x-default".
    ///     In that case the value should be inserted at the first position of the list.
    /// </summary>
    bool HandleLanguageAlternative(ReadOnlySpan<char> path, ReadOnlySpan<char> name, ReadOnlySpan<char> value)
    {
        switch (path)
        {
            case "/x:xmpmeta/rdf:RDF/rdf:Description/dc:description/rdf:Alt/rdf:li":
            case "/x:xmpmeta/rdf:RDF/rdf:Description/dc:rights/rdf:Alt/rdf:li":
            case "/x:xmpmeta/rdf:RDF/rdf:Description/dc:title/rdf:Alt/rdf:li":
                if (name is "xml:lang" && value is "x-default")
                {
                    _nextLanguageAlternativeIsDefault = true;
                    return true;
                }
                break;
        }
        return false;
    }

    static bool ParseBool(ReadOnlySpan<char> value) =>
        // ReSharper disable once SimplifyConditionalTernaryExpression
        value is "True"
            ? true
            : value is "False"
                ? false
                : throw new FormatException($"Expected value to be either 'True' or 'False', but found {value}.");

    static int ParseInt(ReadOnlySpan<char> value) => int.TryParse(value, out int i) ? i : throw new FormatException($"Expected value to be an integer, but found {value}.");

    static DateTimeOffset ParseDateOffset(ReadOnlySpan<char> value) =>
        DateTimeOffset.TryParseExact(value, "yyyy", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AssumeUniversal, out DateTimeOffset d1)
            ? d1
            : DateTimeOffset.TryParseExact(value, "yyyy-MM", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AssumeUniversal, out DateTimeOffset d2)
                ? d2
                : DateTimeOffset.TryParseExact(value, "yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AssumeUniversal, out DateTimeOffset d3)
                    ? d3
                    : DateTimeOffset.TryParseExact(value, "yyyy-MM-ddTHH:mmK", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AssumeUniversal, out DateTimeOffset d4)
                        ? d4
                        : DateTimeOffset.TryParseExact(value, "yyyy-MM-ddTHH:mm:ssK", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AssumeUniversal, out DateTimeOffset d5)
                            ? d5
                            : DateTimeOffset.TryParseExact(
                                value,
                                "yyyy-MM-ddTHH:mm:ss.ffffffK",
                                DateTimeFormatInfo.InvariantInfo,
                                DateTimeStyles.AssumeUniversal,
                                out DateTimeOffset d6
                            )
                                ? d6
                                : throw new FormatException($"Expected value to be a date, but found {value}.");

    static double ParseDouble(ReadOnlySpan<char> value) =>
        double.TryParse(value, CultureInfo.InvariantCulture, out double d) ? d : throw new FormatException($"Expected value to be a double, but found {value}.");

    void CreatePdfAIdentificationMetadata() => result.PdfAIdentification ??= new XmpPdfAIdentificationMetadata();
    void CreateBasicMetadata() => result.Basic ??= new XmpBasicMetadata();
    void CreatePdfMetadata() => result.Pdf ??= new XmpPdfMetadata();
    void CreateDublinCoreMetadata() => result.DublinCore ??= new XmpDublinCoreMetadata();
    void CreatePdfAExtensionsMetadata() => result.PdfAExtensions ??= new XmpPdfAExtensionsMetadata();
    void CreateFacturXMetadata() => result.FacturX ??= new XmpFacturXMetadata();
}
