using System.Globalization;
using FacturXDotNet.Parsers.XMP.Exceptions;
using Microsoft.Extensions.Logging;
using TurboXml;

namespace FacturXDotNet.Parsers.XMP;

struct XmpMetadataXmlReadHandler(XmpMetadata result, ILogger? logger) : IXmlReadHandler
{
    readonly Stack<ReadOnlyMemory<char>> _pathStack = [];
    bool _nextLanguageAlternativeIsDefault;

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
            throw new XmpMetadataParsingException(line, column, exception);
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

        try
        {
            if (HandleLanguageAlternative(path.Span, name, value))
            {
                return;
            }

            string attributePath = $"{path}/{name}";
            Handle(attributePath, value);
        }
        catch (Exception exception)
        {
            throw new XmpMetadataParsingException(valueLine, valueColumn, exception);
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
            throw new XmpMetadataParsingException(line, column, exception);
        }
    }

    public void OnError(string message, int line, int column) => throw new XmpMetadataParsingException(line, column, message);

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
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:property/rdf:Seq/rdf:li":
                result.PdfAExtensions!.Schemas[^1].Property.Add(new XmpPdfAPropertyMetadata());
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:valueType/rdf:Seq/rdf:li":
                result.PdfAExtensions!.Schemas[^1].ValueType.Add(new XmpPdfATypeMetadata());
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:valueType/rdf:Seq/rdf:li/pdfAType:field/rdc:Seq/rdf:li":
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
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:Identifier":
                result.Basic!.Identifier = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:CreateDate":
                result.Basic!.CreateDate = ParseDate(value);
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:CreatorTool":
                result.Basic!.CreatorTool = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:Label":
                result.Basic!.Label = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:MetadataDate":
                result.Basic!.MetadataDate = ParseDate(value);
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:ModifyDate":
                result.Basic!.ModifyDate = ParseDate(value);
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/xmp:Rating":
                result.Basic!.Rating = ParseDecimal(value);
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
                result.DublinCore!.Date.Add(ParseDate(value));
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/dc:description/rdf:Alt/rdf:li":
                if (_nextLanguageAlternativeIsDefault)
                {
                    result.DublinCore!.Description.Insert(0, value.ToString());
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
                }
                else
                {
                    result.DublinCore!.Title.Add(value.ToString());
                }
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/dc:type":
                result.DublinCore!.Type.Add(value.ToString());
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:namespaceURI":
                result.PdfAExtensions!.Schemas[^1].NamespaceUri = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:prefix":
                result.PdfAExtensions!.Schemas[^1].Prefix = value.ToString();
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
            case
                "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:valueType/rdf:Seq/rdf:li/pdfaType:type/pdfaField/rdf:Seq/rdf:li/pdfaField:description"
                :
                result.PdfAExtensions!.Schemas[^1].ValueType[^1].Field[^1].Description = value.ToString();
                break;
            case "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:valueType/rdf:Seq/rdf:li/pdfaType:type/pdfaField/rdf:Seq/rdf:li/pdfaField:name"
                :
                result.PdfAExtensions!.Schemas[^1].ValueType[^1].Field[^1].Name = value.ToString();
                break;
            case
                "/x:xmpmeta/rdf:RDF/rdf:Description/pdfaExtension:schemas/rdf:Bag/rdf:li/pdfaSchema:valueType/rdf:Seq/rdf:li/pdfaType:type/pdfaField/rdf:Seq/rdf:li/pdfaField:valueType"
                :
                result.PdfAExtensions!.Schemas[^1].ValueType[^1].Field[^1].ValueType = value.ToString();
                break;

            default:
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

    static DateTime ParseDate(ReadOnlySpan<char> value) =>
        DateTime.TryParseExact(value, "yyyy", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out DateTime d1)
            ? d1
            : DateTime.TryParseExact(value, "yyyy-MM", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out DateTime d2)
                ? d2
                : DateTime.TryParseExact(value, "yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out DateTime d3)
                    ? d3
                    : DateTime.TryParseExact(value, "yyyy-MM-ddTHH:mmzzz", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out DateTime d4)
                        ? d4
                        : DateTime.TryParseExact(value, "yyyy-MM-ddTHH:mm:sszzz", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out DateTime d5)
                            ? d5
                            : DateTime.TryParseExact(value, "yyyy-MM-ddTHH:mm:ss.fffffffzzz", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out DateTime d6)
                                ? d6
                                : throw new FormatException($"Expected value to be a date, but found {value}.");

    static decimal ParseDecimal(ReadOnlySpan<char> value) =>
        decimal.TryParse(value, CultureInfo.InvariantCulture, out decimal d) ? d : throw new FormatException($"Expected value to be a decimal, but found {value}.");

    void CreatePdfAIdentificationMetadata() => result.PdfAIdentification ??= new XmpPdfAIdentificationXmpMetadata { Part = -1, Conformance = (XmpPdfAConformanceLevel)(-1) };
    void CreateBasicMetadata() => result.Basic ??= new XmpBasicMetadata();
    void CreatePdfMetadata() => result.Pdf ??= new XmpPdfMetadata();
    void CreateDublinCoreMetadata() => result.DublinCore ??= new XmpDublinCoreMetadata();
    void CreatePdfAExtensionsMetadata() => result.PdfAExtensions ??= new XmpPdfAExtensionsMetadata();
}
