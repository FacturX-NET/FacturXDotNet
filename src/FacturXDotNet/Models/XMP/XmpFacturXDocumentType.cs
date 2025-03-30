namespace FacturXDotNet.Models.XMP;

/// <summary>
///     Factur-X document type: INVOICE or ORDER.
/// </summary>
public enum XmpFacturXDocumentType
{
    /// <summary>
    ///     The document is an invoice.
    /// </summary>
    Invoice,

    /// <summary>
    ///     The document is an order.
    /// </summary>
    Order
}

/// <summary>
///     Mapping methods for the <see cref="XmpFacturXDocumentType" /> enumeration.
/// </summary>
public static class XmpFacturXDocumentTypeMappingExtensions
{
    /// <summary>
    ///     Convert the <see cref="XmpFacturXDocumentType" /> to its string representation.
    /// </summary>
    public static ReadOnlySpan<char> ToFacturXDocumentTypeString(this XmpFacturXDocumentType documentType) =>
        documentType switch
        {
            XmpFacturXDocumentType.Invoice => "INVOICE",
            XmpFacturXDocumentType.Order => "ORDER",
            _ => throw new ArgumentOutOfRangeException(nameof(documentType), documentType, null)
        };

    /// <summary>
    ///     Convert the string to its <see cref="XmpFacturXDocumentType" /> representation.
    /// </summary>
    /// <seealso cref="ToFacturXDocumentType(ReadOnlySpan{char})" />
    public static XmpFacturXDocumentType? ToFacturXDocumentTypeOrNull(this ReadOnlySpan<char> value) =>
        value switch
        {
            "INVOICE" => XmpFacturXDocumentType.Invoice,
            "ORDER" => XmpFacturXDocumentType.Order,
            _ => null
        };

    /// <summary>
    ///     Convert the string to its <see cref="XmpFacturXDocumentType" /> representation.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not a valid <see cref="XmpFacturXDocumentType" />.</exception>
    public static XmpFacturXDocumentType ToFacturXDocumentType(this ReadOnlySpan<char> value) =>
        ToFacturXDocumentTypeOrNull(value) ?? throw new ArgumentOutOfRangeException(nameof(value), value.ToString(), null);
}
