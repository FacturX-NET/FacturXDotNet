namespace FacturXDotNet;

/// <summary>
///     Code list for the Tax Scheme identifiers for VAT.
/// </summary>
/// <remarks>
///     This enumeration only has one value because there is only one possible code when the date only contains the day, month and year. However, this enumeration is still useful to
///     make the code more readable.
/// </remarks>
public enum VatOnlyTaxSchemeIdentifier
{
    Vat
}

public static class VatOnlyTaxSchemeIdentifierMappingExtensions
{
    public static string ToVatOnlyTaxSchemeIdentifier(this VatOnlyTaxSchemeIdentifier value) =>
        value switch
        {
            VatOnlyTaxSchemeIdentifier.Vat => "VA",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

    public static VatOnlyTaxSchemeIdentifier? ToVatOnlyTaxSchemeIdentifierOrNull(this ReadOnlySpan<char> value) =>
        value switch
        {
            "VA" => VatOnlyTaxSchemeIdentifier.Vat,
            _ => null
        };

    public static VatOnlyTaxSchemeIdentifier ToVatOnlyTaxSchemeIdentifier(this ReadOnlySpan<char> value) =>
        value.ToVatOnlyTaxSchemeIdentifierOrNull() ?? throw new ArgumentOutOfRangeException(nameof(DateOnlyFormat), value.ToString(), null);
}
