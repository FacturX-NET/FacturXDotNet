using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules;

/// <summary>
///     Represents any business rule for validating a Factur-X document.
/// </summary>
/// <param name="Name">The name of the rule.</param>
/// <param name="Profiles">The profiles in which this rule should be enforced.</param>
/// <param name="Description">A description of the rule.</param>
/// <param name="Severity">The severity of the rule.</param>
public abstract record BusinessRule(string Name, string Description, FacturXProfileFlags Profiles, FacturXBusinessRuleSeverity Severity)
{
    /// <summary>
    ///     Formats the rule as a string.
    /// </summary>
    public abstract string Format();
}
