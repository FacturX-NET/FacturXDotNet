namespace FacturXDotNet.Models.CII;

/// <summary>
///     Code list for the Tax Scheme identifiers for VAT.
/// </summary>
/// <remarks>
///     This enumeration only has one value because there is only one possible code when the date only contains the day, month and year. However, this enumeration is still useful to
///     make the code more readable.
/// </remarks>
public enum VatOnlyTaxSchemeIdentifier
{
    /// <summary>
    ///     VAT tax scheme identifier.
    /// </summary>
    Vat
}

/// <summary>
///     Mapping methods for the <see cref="VatOnlyTaxSchemeIdentifier" /> enumeration.
/// </summary>
public static class VatOnlyTaxSchemeIdentifierMappingExtensions
{
    /// <summary>
    ///     Convert the <see cref="VatOnlyTaxSchemeIdentifier" /> to its string representation.
    /// </summary>
    public static string ToVatOnlyTaxSchemeIdentifier(this VatOnlyTaxSchemeIdentifier value) =>
        value switch
        {
            VatOnlyTaxSchemeIdentifier.Vat => "VA",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

    /// <summary>
    ///     Convert the string to its <see cref="VatOnlyTaxSchemeIdentifier" /> representation.
    /// </summary>
    /// <seealso cref="ToVatOnlyTaxSchemeIdentifier(ReadOnlySpan{char})" />
    public static VatOnlyTaxSchemeIdentifier? ToVatOnlyTaxSchemeIdentifierOrNull(this ReadOnlySpan<char> value) =>
        value switch
        {
            "VA" => VatOnlyTaxSchemeIdentifier.Vat,
            _ => null
        };

    /// <summary>
    ///     Convert the string to its <see cref="VatOnlyTaxSchemeIdentifier" /> representation.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not a valid <see cref="VatOnlyTaxSchemeIdentifier" />.</exception>
    public static VatOnlyTaxSchemeIdentifier ToVatOnlyTaxSchemeIdentifier(this ReadOnlySpan<char> value) =>
        value.ToVatOnlyTaxSchemeIdentifierOrNull() ?? throw new ArgumentOutOfRangeException(nameof(DateOnlyFormat), value.ToString(), null);
}
