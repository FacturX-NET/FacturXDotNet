using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     Represents a business rule for validating a Factur-X document.
///     Hybrid rules are about the data in the document that is not part of the Cross-Industry Invoice, e.g. XMP metadata.
/// </summary>
/// <param name="Name">The name of the rule.</param>
/// <param name="Description">A description of the rule.</param>
/// <param name="Severity">The severity of the rule.</param>
public abstract record HybridBusinessRule(string Name, string Description, FacturXBusinessRuleSeverity Severity = FacturXBusinessRuleSeverity.Fatal) : FacturXBusinessRule(
    Name,
    Description,
    FacturXProfileFlags.All,
    Severity
)
{
    /// <summary>
    ///     Returns a string representation of the business rule.
    /// </summary>
    public override string Format() => $"[HYBRID] {Name} - {Description}";
}
