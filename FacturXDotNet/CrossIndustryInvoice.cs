namespace FacturXDotNet;

/// <summary>
///     An invoice in the Factur-X format.
///     This class represents invoices in any profile of the Factur-X format. To that end, the nullability of the properties is determined by the MINIMUM profile.
/// </summary>
public sealed class CrossIndustryInvoice
{
    /// <inheritdoc cref="FacturXDotNet.ExchangedDocumentContext" />
    public required ExchangedDocumentContext ExchangedDocumentContext { get; set; }

    /// <inheritdoc cref="FacturXDotNet.ExchangedDocument" />
    public required ExchangedDocument ExchangedDocument { get; set; }

    /// <inheritdoc cref="FacturXDotNet.SupplyChainTradeTransaction" />
    public required SupplyChainTradeTransaction SupplyChainTradeTransaction { get; set; }
}
