using System.Xml;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Generation.XMP;

class XmpPdfMetadataWriter
{
    const string NsPdf = "http://ns.adobe.com/pdf/1.3/";
    const string PrefixPdf = "pdf";

    public async Task WriteAsync(XmlWriter writer, XmpPdfMetadata data)
    {
        await writer.WriteStartElementAsync("rdf", "Description", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
        await writer.WriteAttributeStringAsync("xmlns", PrefixPdf, "http://www.w3.org/2000/xmlns/", NsPdf);

        if (data.Keywords is not null)
        {
            await writer.WriteElementStringAsync(PrefixPdf, "Keywords", NsPdf, data.Keywords);
        }

        if (data.PdfVersion is not null)
        {
            await writer.WriteElementStringAsync(PrefixPdf, "PDFVersion", NsPdf, data.PdfVersion);
        }

        if (data.Producer is not null)
        {
            await writer.WriteElementStringAsync(PrefixPdf, "Producer", NsPdf, data.Producer);
        }

        if (data.Trapped.HasValue)
        {
            await writer.WriteElementStringAsync(PrefixPdf, "Trapped", NsPdf, data.Trapped.Value ? "True" : "False");
        }

        await writer.WriteEndElementAsync();
    }
}
