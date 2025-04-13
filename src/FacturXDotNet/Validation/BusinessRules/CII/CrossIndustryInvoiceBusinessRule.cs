using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.Validation.BusinessRules.CII;

/// <summary>
///     Represents any business rule for validating a Factur-X document.
/// </summary>
/// <param name="Name">The name of the rule.</param>
/// <param name="Description">A description of the rule.</param>
/// <param name="Profiles">The profiles in which this rule should be enforced.</param>
/// <param name="TermsInvolved">The business terms that are used to check this rule.</param>
public abstract record CrossIndustryInvoiceBusinessRule(string Name, string Description, FacturXProfileFlags Profiles, IReadOnlyCollection<string> TermsInvolved) : BusinessRule(
    Name,
    Description,
    Profiles,
    BusinessRuleSeverity.Fatal
)
{
    /// <summary>
    ///     Determines whether the invoice satisfies the conditions defined by the rule.
    /// </summary>
    /// <param name="cii">The Cross-Industry Invoice to validate.</param>
    /// <param name="logger"></param>
    /// <returns><c>true</c> if the rule is satisfied by the invoice; otherwise <c>false</c>.</returns>
    public abstract bool Check(CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null);

    /// <inheritdoc />
    public override string Format() => $"[{Profiles.GetMinProfile()}] {Name} - {Description}";
}
