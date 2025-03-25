using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Validation.BusinessRules;

/// <summary>
///     Represents any business rule for validating a Factur-X document.
/// </summary>
/// <param name="Name">The name of the rule.</param>
/// <param name="Description">A description of the rule.</param>
/// <param name="Profiles">The profiles in which this rule should be enforced.</param>
public abstract record CrossIndustryInvoiceBusinessRule(string Name, string Description, FacturXProfileFlags Profiles) : BusinessRule(
    Name,
    Description,
    Profiles,
    FacturXBusinessRuleSeverity.Fatal
)
{
    /// <summary>
    ///     Determines whether the invoice satisfies the conditions defined by the rule.
    /// </summary>
    /// <param name="cii">The Cross-Industry Invoice to validate.</param>
    /// <returns><c>true</c> if the rule is satisfied by the invoice; otherwise <c>false</c>.</returns>
    public abstract bool Check(CrossIndustryInvoice? cii);

    /// <inheritdoc />
    public override string Format() => $"[{Profiles.GetMinProfile()}] {Name} - {Description}";
}
