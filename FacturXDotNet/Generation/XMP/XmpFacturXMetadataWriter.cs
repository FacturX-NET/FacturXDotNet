using System.Xml;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Generation.XMP;

class XmpFacturXMetadataWriter(XmpFacturXMetadataWriterOptions? options = null)
{
    readonly XmpFacturXMetadataWriterOptions _options = options ?? new XmpFacturXMetadataWriterOptions();

    public async Task WriteAsync(XmlWriter writer, XmpFacturXMetadata data)
    {
        await writer.WriteStartElementAsync("rdf", "Description", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
        await writer.WriteAttributeStringAsync("xmlns", _options.Prefix, "http://www.w3.org/2000/xmlns/", _options.NamespaceUri);
        await writer.WriteAttributeStringAsync("rdf", "about", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "");

        if (data.DocumentFileName is not null)
        {
            await writer.WriteElementStringAsync(_options.Prefix, "DocumentFileName", _options.NamespaceUri, data.DocumentFileName);
        }

        if (data.DocumentType.HasValue)
        {
            await writer.WriteElementStringAsync(_options.Prefix, "DocumentType", _options.NamespaceUri, data.DocumentType.Value.ToFacturXDocumentTypeString().ToString());
        }

        if (data.Version is not null)
        {
            await writer.WriteElementStringAsync(_options.Prefix, "Version", _options.NamespaceUri, data.Version);
        }

        if (data.ConformanceLevel.HasValue)
        {
            await writer.WriteElementStringAsync(
                _options.Prefix,
                "ConformanceLevel",
                _options.NamespaceUri,
                data.ConformanceLevel.Value.ToXmpFacturXConformanceLevelString().ToString()
            );
        }

        await writer.WriteEndElementAsync();
    }
}

/// <summary>
///     Options for writing the FacturX metadata.
/// </summary>
public class XmpFacturXMetadataWriterOptions
{
    /// <summary>
    ///     The namespace URI to use for the FacturX metadata.
    /// </summary>
    public string NamespaceUri { get; set; } = "urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#";

    /// <summary>
    ///     The prefix to use for the FacturX metadata.
    /// </summary>
    public string Prefix { get; set; } = "fx";
}
