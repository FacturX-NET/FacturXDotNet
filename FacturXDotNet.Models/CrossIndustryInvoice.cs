namespace FacturXDotNet.Models;

/// <summary>
///     An invoice in the Factur-X format.
///     This class represents invoices in any profile of the Factur-X format. To that end, the nullability of the properties is determined by the MINIMUM profile.
/// </summary>
public sealed class CrossIndustryInvoice
{
    /// <inheritdoc cref="Models.ExchangedDocumentContext" />
    public required ExchangedDocumentContext ExchangedDocumentContext { get; set; }

    /// <inheritdoc cref="Models.ExchangedDocument" />
    public required ExchangedDocument ExchangedDocument { get; set; }

    /// <inheritdoc cref="Models.SupplyChainTradeTransaction" />
    public required SupplyChainTradeTransaction SupplyChainTradeTransaction { get; set; }
}
