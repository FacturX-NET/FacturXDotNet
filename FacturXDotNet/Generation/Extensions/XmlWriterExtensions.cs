using System.Xml;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Generation.Extensions;

static class XmlWriterExtensions
{
    const string NsRdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
    const string PrefixRdf = "rdf";
    const string NsUdt = "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100";
    const string PrefixUdt = "udt";

    public static async Task StartBagAsync(this XmlWriter writer) => await writer.WriteStartElementAsync(PrefixRdf, "Bag", NsRdf);

    public static async Task WriteBagAsync<TValue>(this XmlWriter writer, IReadOnlyCollection<TValue> values, Func<XmlWriter, TValue, Task> writeValue)
    {
        await writer.StartBagAsync();

        foreach (TValue value in values)
        {
            await writer.StartLiAsync(false);
            await writeValue(writer, value);
            await writer.WriteEndElementAsync();
        }

        await writer.WriteEndElementAsync();
    }

    public static async Task StartSeqAsync(this XmlWriter writer) => await writer.WriteStartElementAsync(PrefixRdf, "Seq", NsRdf);

    public static async Task WriteSeqAsync<TValue>(this XmlWriter writer, IReadOnlyList<TValue> values, Func<XmlWriter, TValue, Task> writeValue)
    {
        await writer.StartSeqAsync();

        foreach (TValue value in values)
        {
            await writer.StartLiAsync(false);
            await writeValue(writer, value);
            await writer.WriteEndElementAsync();
        }

        await writer.WriteEndElementAsync();
    }

    public static async Task StartAltAsync(this XmlWriter writer) => await writer.WriteStartElementAsync(PrefixRdf, "Alt", NsRdf);

    public static async Task WriteAltAsync<TValue>(this XmlWriter writer, IReadOnlyList<TValue> values, Func<XmlWriter, TValue, Task> writeValue)
    {
        await writer.StartAltAsync();

        bool first = true;
        foreach (TValue value in values)
        {
            await writer.StartLiAsync(first);
            await writeValue(writer, value);
            await writer.WriteEndElementAsync();
            first = false;
        }

        await writer.WriteEndElementAsync();
    }

    public static async Task StartLiAsync(this XmlWriter writer, bool addXDefaultAttribute)
    {
        await writer.WriteStartElementAsync(PrefixRdf, "li", NsRdf);

        if (addXDefaultAttribute)
        {
            await writer.WriteAttributeStringAsync("xml", "lang", "http://www.w3.org/XML/1998/namespace", "x-default");
        }
    }

    public static async Task WriteDateOnlyAsync(this XmlWriter writer, DateOnly value)
    {
        await writer.WriteStartElementAsync(PrefixUdt, "DateTimeString", NsUdt);
        await writer.WriteAttributeStringAsync(null, "format", null, DateOnlyFormat.DateOnly.ToDateOnlyFormat().ToString());
        await writer.WriteStringAsync(value.ToString("yyyyMMdd"));
        await writer.WriteEndElementAsync();
    }
}
