using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules.CII;

/// <summary>
///     Represents a business rule for validating the Cross-Industry Invoice in a Factur-X document.
/// </summary>
/// <param name="Name">The name of the rule.</param>
/// <param name="Description">A description of the rule.</param>
/// <param name="Profiles">The profiles in which this rule should be enforced.</param>
public abstract record CrossIndustryInvoiceBusinessRule(string Name, string Description, FacturXProfileFlags Profiles) : FacturXBusinessRule(
    Name,
    Description,
    Profiles,
    FacturXBusinessRuleSeverity.Fatal
)
{
    /// <summary>
    ///     Determines whether the invoice satisfies the conditions defined by the rule.
    /// </summary>
    /// <param name="invoice">The invoice to validate.</param>
    /// <returns><c>true</c> if the rule is satisfied by the invoice; otherwise <c>false</c>.</returns>
    public abstract bool Check(CrossIndustryInvoice invoice);

    /// <inheritdoc />
    public override sealed bool Check(FacturXDocument invoice) => Check(invoice.CrossIndustryInvoice);

    /// <summary>
    ///     Returns a string representation of the business rule.
    /// </summary>
    public override string Format() => $"[{Profiles.GetMinProfile()}] {Name} - {Description}";
}
