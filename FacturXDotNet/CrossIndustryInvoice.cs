using FacturXDotNet.Models.CII;

namespace FacturXDotNet;

/// <summary>
///     An invoice in the Factur-X format.
///     This class represents invoices in any profile of the Factur-X format. To that end, the nullability of the properties is determined by the MINIMUM profile.
/// </summary>
public sealed class CrossIndustryInvoice
{
    /// <inheritdoc cref="Models.CII.ExchangedDocumentContext" />
    public required ExchangedDocumentContext ExchangedDocumentContext { get; set; }

    /// <inheritdoc cref="Models.CII.ExchangedDocument" />
    public required ExchangedDocument ExchangedDocument { get; set; }

    /// <inheritdoc cref="Models.CII.SupplyChainTradeTransaction" />
    public required SupplyChainTradeTransaction SupplyChainTradeTransaction { get; set; }
}
