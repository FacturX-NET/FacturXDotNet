namespace FacturXDotNet.Models;

/// <summary>
///     An invoice in the Factur-X format.
///     This class represents invoices in any profile of the Factur-X format. To that end, the nullability of the properties is determined by the MINIMUM profile.
/// </summary>
public sealed class FacturXCrossIndustryInvoice
{
    /// <inheritdoc cref="FacturXExchangedDocumentContext" />
    public required FacturXExchangedDocumentContext ExchangedDocumentContext { get; set; }

    /// <inheritdoc cref="FacturXExchangedDocument" />
    public required FacturXExchangedDocument ExchangedDocument { get; set; }

    /// <inheritdoc cref="FacturXSupplyChainTradeTransaction" />
    public required FacturXSupplyChainTradeTransaction SupplyChainTradeTransaction { get; set; }
}
