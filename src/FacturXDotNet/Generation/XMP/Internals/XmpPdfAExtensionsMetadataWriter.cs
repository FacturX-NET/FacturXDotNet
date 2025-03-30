using System.Xml;
using FacturXDotNet.Generation.Internals;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Generation.XMP.Internals;

class XmpPdfAExtensionsMetadataWriter
{
    const string NsPdfAExtension = "http://www.aiim.org/pdfa/ns/extension/";
    const string PrefixPdfAExtension = "pdfaExtension";
    const string NsPdfASchema = "http://www.aiim.org/pdfa/ns/schema#";
    const string PrefixPdfASchema = "pdfaSchema";
    const string NsPdfAProperty = "http://www.aiim.org/pdfa/ns/property#";
    const string PrefixPdfAProperty = "pdfaProperty";
    const string NsPdfAType = "http://www.aiim.org/pdfa/ns/type#";
    const string PrefixPdfAType = "pdfaType";
    const string NsPdfAField = "http://www.aiim.org/pdfa/ns/field#";
    const string PrefixPdfAField = "pdfaField";

    const string NsRdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
    const string PrefixRdf = "rdf";

    public async Task WriteAsync(XmlWriter writer, XmpPdfAExtensionsMetadata data)
    {
        await writer.WriteStartElementAsync("rdf", "Description", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
        await writer.WriteAttributeStringAsync("xmlns", PrefixPdfAExtension, "http://www.w3.org/2000/xmlns/", NsPdfAExtension);
        await writer.WriteAttributeStringAsync("rdf", "about", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "");

        if (data.Schemas.Count > 0)
        {
            await writer.WriteAttributeStringAsync("xmlns", PrefixPdfASchema, "http://www.w3.org/2000/xmlns/", NsPdfASchema);
        }

        if (data.Schemas.Any(s => s.Property.Count != 0))
        {
            await writer.WriteAttributeStringAsync("xmlns", PrefixPdfAProperty, "http://www.w3.org/2000/xmlns/", NsPdfAProperty);
        }

        if (data.Schemas.Any(s => s.ValueType.Count != 0))
        {
            await writer.WriteAttributeStringAsync("xmlns", PrefixPdfAType, "http://www.w3.org/2000/xmlns/", NsPdfAType);
        }

        await writer.WriteStartElementAsync(PrefixPdfAExtension, "schemas", NsPdfAExtension);
        await writer.WriteBagAsync(data.Schemas, WriteSchemaAsync);
        await writer.WriteEndElementAsync();

        await writer.WriteEndElementAsync();
    }

    static async Task WriteSchemaAsync(XmlWriter writer, XmpPdfASchemaMetadata schema)
    {
        await writer.WriteAttributeStringAsync(PrefixRdf, "parseType", NsRdf, "Resource");

        if (schema.NamespaceUri is not null)
        {
            await writer.WriteElementStringAsync(PrefixPdfASchema, "namespaceURI", NsPdfASchema, schema.NamespaceUri);
        }

        if (schema.Prefix is not null)
        {
            await writer.WriteElementStringAsync(PrefixPdfASchema, "prefix", NsPdfASchema, schema.Prefix);
        }

        if (schema.Property.Count > 0)
        {
            await writer.WriteStartElementAsync(PrefixPdfASchema, "property", NsPdfASchema);
            await writer.WriteSeqAsync(schema.Property, WritePropertyAsync);
            await writer.WriteEndElementAsync();
        }

        if (schema.Schema is not null)
        {
            await writer.WriteElementStringAsync(PrefixPdfASchema, "schema", NsPdfASchema, schema.Schema);
        }

        if (schema.ValueType.Count > 0)
        {
            await writer.WriteStartElementAsync(PrefixPdfASchema, "valueType", NsPdfASchema);
            await writer.WriteSeqAsync(schema.ValueType, WriteValueTypeAsync);
            await writer.WriteEndElementAsync();
        }
    }

    static async Task WritePropertyAsync(XmlWriter writer, XmpPdfAPropertyMetadata property)
    {
        await writer.WriteAttributeStringAsync(PrefixRdf, "parseType", NsRdf, "Resource");

        if (property.Category.HasValue)
        {
            await writer.WriteElementStringAsync(PrefixPdfAProperty, "category", NsPdfAProperty, property.Category.Value.ToXmpPdfAPropertyCategoryString());
        }

        if (property.Description is not null)
        {
            await writer.WriteElementStringAsync(PrefixPdfAProperty, "description", NsPdfAProperty, property.Description);
        }

        if (property.Name is not null)
        {
            await writer.WriteElementStringAsync(PrefixPdfAProperty, "name", NsPdfAProperty, property.Name);
        }

        if (property.ValueType is not null)
        {
            await writer.WriteElementStringAsync(PrefixPdfAProperty, "valueType", NsPdfAProperty, property.ValueType);
        }
    }

    static async Task WriteValueTypeAsync(XmlWriter writer, XmpPdfATypeMetadata type)
    {
        await writer.WriteAttributeStringAsync(PrefixRdf, "parseType", NsRdf, "Resource");

        if (type.Description is not null)
        {
            await writer.WriteElementStringAsync(PrefixPdfAType, "description", NsPdfAType, type.Description);
        }

        if (type.Field.Count > 0)
        {
            await writer.WriteStartElementAsync(PrefixPdfAType, "field", NsPdfAType);
            await writer.WriteSeqAsync(type.Field, WriteFieldAsync);
            await writer.WriteEndElementAsync();
        }

        if (type.NamespaceUri is not null)
        {
            await writer.WriteElementStringAsync(PrefixPdfAType, "namespaceURI", NsPdfAType, type.NamespaceUri);
        }

        if (type.Prefix is not null)
        {
            await writer.WriteElementStringAsync(PrefixPdfAType, "prefix", NsPdfAType, type.Prefix);
        }

        if (type.Type is not null)
        {
            await writer.WriteElementStringAsync(PrefixPdfAType, "type", NsPdfAType, type.Type);
        }
    }

    static async Task WriteFieldAsync(XmlWriter writer, XmpPdfAFieldMetadata field)
    {
        await writer.WriteAttributeStringAsync(PrefixRdf, "parseType", NsRdf, "Resource");

        if (field.Description is not null)
        {
            await writer.WriteElementStringAsync(PrefixPdfAField, "description", NsPdfAField, field.Description);
        }

        if (field.Name is not null)
        {
            await writer.WriteElementStringAsync(PrefixPdfAField, "name", NsPdfAField, field.Name);
        }

        if (field.ValueType is not null)
        {
            await writer.WriteElementStringAsync(PrefixPdfAField, "valueType", NsPdfAField, field.ValueType);
        }
    }
}
