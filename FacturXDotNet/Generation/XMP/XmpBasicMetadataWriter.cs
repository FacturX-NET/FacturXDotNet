using System.Globalization;
using System.Xml;
using FacturXDotNet.Generation.Extensions;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Generation.XMP;

class XmpBasicMetadataWriter
{
    const string NsXmp = "http://ns.adobe.com/pdf/1.3/";
    const string PrefixXmp = "xmp";
    const string NsXmpGlmg = "http://ns.adobe.com/xap/1.0/g/img/";
    const string PrefixXmpGlmg = "xmpGlmg";

    public async Task WriteAsync(XmlWriter writer, XmpBasicMetadata data)
    {
        await writer.WriteStartElementAsync("rdf", "Description", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
        await writer.WriteAttributeStringAsync("xmlns", PrefixXmp, "http://www.w3.org/2000/xmlns/", NsXmp);

        if (data.Thumbnails.Count > 0)
        {
            await writer.WriteAttributeStringAsync("xmlns", PrefixXmpGlmg, "http://www.w3.org/2000/xmlns/", NsXmpGlmg);
        }

        if (data.CreateDate.HasValue)
        {
            await writer.WriteElementStringAsync(PrefixXmp, "CreateDate", NsXmp, data.CreateDate.Value.FormatXmpDate());
        }

        if (data.CreatorTool is not null)
        {
            await writer.WriteElementStringAsync(PrefixXmp, "CreatorTool", NsXmp, data.CreatorTool);
        }

        if (data.Identifier.Count > 0)
        {
            await writer.WriteStartElementAsync(PrefixXmp, "Identifier", NsXmp);
            await writer.WriteBagAsync(data.Identifier, (w, s) => w.WriteStringAsync(s));
            await writer.WriteEndElementAsync();
        }

        if (data.Label is not null)
        {
            await writer.WriteElementStringAsync(PrefixXmp, "Label", NsXmp, data.Label);
        }

        if (data.MetadataDate.HasValue)
        {
            await writer.WriteElementStringAsync(PrefixXmp, "MetadataDate", NsXmp, data.MetadataDate.Value.FormatXmpDate());
        }

        if (data.ModifyDate.HasValue)
        {
            await writer.WriteElementStringAsync(PrefixXmp, "ModifyDate", NsXmp, data.ModifyDate.Value.FormatXmpDate());
        }

        if (data.Rating != 0)
        {
            await writer.WriteElementStringAsync(PrefixXmp, "Rating", NsXmp, data.Rating.ToString(CultureInfo.InvariantCulture));
        }

        if (data.BaseUrl is not null)
        {
            await writer.WriteElementStringAsync(PrefixXmp, "BaseURL", NsXmp, data.BaseUrl);
        }

        if (data.Nickname is not null)
        {
            await writer.WriteElementStringAsync(PrefixXmp, "Nickname", NsXmp, data.Nickname);
        }

        if (data.Thumbnails.Count > 0)
        {
            await writer.WriteStartElementAsync(PrefixXmp, "Thumbnails", NsXmp);
            await writer.WriteBagAsync(data.Thumbnails, WriteThumbnailAsync);
            await writer.WriteEndElementAsync();
        }

        await writer.WriteEndElementAsync();
    }

    static async Task WriteThumbnailAsync(XmlWriter writer, XmpThumbnail thumbnail)
    {
        if (thumbnail.Format.HasValue)
        {
            await writer.WriteElementStringAsync(PrefixXmpGlmg, "format", NsXmpGlmg, thumbnail.Format.Value.ToXmpThumbnailFormat());
        }

        if (thumbnail.Height.HasValue)
        {
            await writer.WriteElementStringAsync(PrefixXmpGlmg, "height", NsXmpGlmg, thumbnail.Height.Value.ToString());
        }

        if (thumbnail.Width.HasValue)
        {
            await writer.WriteElementStringAsync(PrefixXmpGlmg, "width", NsXmpGlmg, thumbnail.Width.Value.ToString());
        }

        if (thumbnail.Image is not null)
        {
            await writer.WriteElementStringAsync(PrefixXmpGlmg, "image", NsXmpGlmg, thumbnail.Image);
        }
    }
}
