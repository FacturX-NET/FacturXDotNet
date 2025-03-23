using TurboXml;

namespace FacturXDotNet.Parsing.XMP;

/// <summary>
///     Read a <see cref="XmpMetadata" /> from an XML stream.
/// </summary>
public class XmpMetadataReader(XmpMetadataReaderOptions? options = null)
{
    readonly XmpMetadataReaderOptions _options = options ?? new XmpMetadataReaderOptions();

    /// <summary>
    ///     Parse the given stream into a <see cref="XmpMetadata" />.
    /// </summary>
    public XmpMetadata Read(Stream stream)
    {
        XmpMetadata result = new();
        XmpMetadataXmlReadHandler handler = new(result, _options.Logger);

        XmlParser.Parse(stream, ref handler);

        return result;
    }
}
