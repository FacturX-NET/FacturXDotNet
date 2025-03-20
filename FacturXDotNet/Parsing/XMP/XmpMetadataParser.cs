using TurboXml;

namespace FacturXDotNet.Parsing.XMP;

/// <summary>
///     Parse a <see cref="XmpMetadata" /> from an XML stream.
/// </summary>
public class XmpMetadataParser(XmpMetadataParserOptions? options = null)
{
    readonly XmpMetadataParserOptions _options = options ?? new XmpMetadataParserOptions();

    /// <summary>
    ///     Parse the given stream into a <see cref="XmpMetadata" />.
    /// </summary>
    public XmpMetadata ParseXmpMetadata(Stream stream)
    {
        XmpMetadata result = new();
        XmpMetadataXmlReadHandler handler = new(result, _options.Logger);

        XmlParser.Parse(stream, ref handler);

        return result;
    }
}
