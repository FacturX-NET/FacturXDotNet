using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules;

/// <summary>
///     Represents any business rule for validating a Factur-X document.
/// </summary>
/// <param name="Name">The name of the rule.</param>
/// <param name="Description">A description of the rule.</param>
/// <param name="Profiles">The profiles in which this rule should be enforced.</param>
public abstract record FacturXBusinessRule(string Name, string Description, FacturXProfileFlags Profiles, FacturXBusinessRuleSeverity Severity)
{
    /// <summary>
    ///     Determines whether the invoice satisfies the conditions defined by the rule.
    /// </summary>
    /// <param name="invoice">The invoice to validate.</param>
    /// <returns><c>true</c> if the rule is satisfied by the invoice; otherwise <c>false</c>.</returns>
    public abstract bool Check(FacturX invoice);

    /// <summary>
    ///     Formats the rule as a string.
    /// </summary>
    public abstract string Format();
}
