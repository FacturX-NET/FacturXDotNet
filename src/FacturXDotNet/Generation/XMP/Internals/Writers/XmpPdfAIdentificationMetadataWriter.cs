using System.Xml;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Generation.XMP.Internals.Writers;

class XmpPdfAIdentificationMetadataWriter
{
    const string NsPdfaid = "http://www.aiim.org/pdfa/ns/id/";
    const string PrefixPdfaid = "pdfaid";

    public async Task WriteAsync(XmlWriter writer, XmpPdfAIdentificationMetadata data)
    {
        await writer.WriteStartElementAsync("rdf", "Description", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
        await writer.WriteAttributeStringAsync("xmlns", PrefixPdfaid, "http://www.w3.org/2000/xmlns/", NsPdfaid);
        await writer.WriteAttributeStringAsync("rdf", "about", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "");

        if (data.Amendment is not null)
        {
            await writer.WriteElementStringAsync(PrefixPdfaid, "amd", NsPdfaid, data.Amendment);
        }

        if (data.Conformance != null)
        {
            await writer.WriteElementStringAsync(PrefixPdfaid, "conformance", NsPdfaid, data.Conformance.Value.ToXmpPdfAConformanceLevel().ToString());
        }

        if (data.Part != null)
        {
            await writer.WriteElementStringAsync(PrefixPdfaid, "part", NsPdfaid, data.Part.Value.ToString());
        }

        await writer.WriteEndElementAsync();
    }
}
