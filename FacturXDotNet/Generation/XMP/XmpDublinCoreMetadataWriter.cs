using System.Xml;
using FacturXDotNet.Generation.Extensions;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Generation.XMP;

class XmpDublinCoreMetadataWriter
{
    const string NsDc = "http://purl.org/dc/elements/1.1/";
    const string PrefixDc = "dc";

    public async Task WriteAsync(XmlWriter writer, XmpDublinCoreMetadata data)
    {
        await writer.WriteStartElementAsync("rdf", "Description", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
        await writer.WriteAttributeStringAsync("xmlns", PrefixDc, "http://www.w3.org/2000/xmlns/", NsDc);

        if (data.Contributor.Count > 0)
        {
            await writer.WriteStartElementAsync(PrefixDc, "contributor", NsDc);
            await writer.WriteBagAsync(data.Contributor, (w, s) => w.WriteStringAsync(s));
            await writer.WriteEndElementAsync();
        }

        if (data.Coverage != null)
        {
            await writer.WriteElementStringAsync(PrefixDc, "coverage", NsDc, data.Coverage);
        }

        if (data.Creator.Count > 0)
        {
            await writer.WriteStartElementAsync(PrefixDc, "creator", NsDc);
            await writer.WriteSeqAsync(data.Creator, (w, s) => w.WriteStringAsync(s));
            await writer.WriteEndElementAsync();
        }

        if (data.Date.Count > 0)
        {
            await writer.WriteStartElementAsync(PrefixDc, "date", NsDc);
            await writer.WriteSeqAsync(data.Date, (w, d) => w.WriteStringAsync(d.FormatXmpDate()));
            await writer.WriteEndElementAsync();
        }

        if (data.Description.Count > 0)
        {
            await writer.WriteStartElementAsync(PrefixDc, "description", NsDc);
            await writer.WriteAltAsync(data.Description, (w, s) => w.WriteStringAsync(s));
            await writer.WriteEndElementAsync();
        }

        if (data.Format != null)
        {
            await writer.WriteElementStringAsync(PrefixDc, "format", NsDc, data.Format);
        }

        if (data.Identifier != null)
        {
            await writer.WriteElementStringAsync(PrefixDc, "identifier", NsDc, data.Identifier);
        }

        if (data.Language.Count > 0)
        {
            await writer.WriteStartElementAsync(PrefixDc, "language", NsDc);
            await writer.WriteBagAsync(data.Language, (w, s) => w.WriteStringAsync(s));
            await writer.WriteEndElementAsync();
        }

        if (data.Publisher.Count > 0)
        {
            await writer.WriteStartElementAsync(PrefixDc, "publisher", NsDc);
            await writer.WriteBagAsync(data.Publisher, (w, s) => w.WriteStringAsync(s));
            await writer.WriteEndElementAsync();
        }

        if (data.Relation.Count > 0)
        {
            await writer.WriteStartElementAsync(PrefixDc, "relation", NsDc);
            await writer.WriteBagAsync(data.Relation, (w, s) => w.WriteStringAsync(s));
            await writer.WriteEndElementAsync();
        }

        if (data.Rights.Count > 0)
        {
            await writer.WriteStartElementAsync(PrefixDc, "rights", NsDc);
            await writer.WriteAltAsync(data.Rights, (w, s) => w.WriteStringAsync(s));
            await writer.WriteEndElementAsync();
        }

        if (data.Source != null)
        {
            await writer.WriteElementStringAsync(PrefixDc, "source", NsDc, data.Source);
        }

        if (data.Subject.Count > 0)
        {
            await writer.WriteStartElementAsync(PrefixDc, "subject", NsDc);
            await writer.WriteBagAsync(data.Subject, (w, s) => w.WriteStringAsync(s));
            await writer.WriteEndElementAsync();
        }

        if (data.Title.Count > 0)
        {
            await writer.WriteStartElementAsync(PrefixDc, "title", NsDc);
            await writer.WriteAltAsync(data.Title, (w, s) => w.WriteStringAsync(s));
            await writer.WriteEndElementAsync();
        }

        if (data.Type.Count > 0)
        {
            await writer.WriteStartElementAsync(PrefixDc, "type", NsDc);
            await writer.WriteBagAsync(data.Type, (w, s) => w.WriteStringAsync(s));
            await writer.WriteEndElementAsync();
        }

        await writer.WriteEndElementAsync();
    }
}
