namespace FacturXDotNet.Models.Validation;

/// <summary>
///     Represents a business rule for validating a Factur-X Cross-Industry Invoice.
/// </summary>
/// <remarks>
///     This abstract class defines the structure for creating specific business rules that can be applied to an invoice.
///     Subclasses must implement the <see cref="Check" /> method to define the specific validation logic.
/// </remarks>
/// <param name="name">The name of the rule.</param>
/// <param name="description">A description of the rule.</param>
/// <param name="profiles">The profiles in which this rule should be enforced.</param>
public abstract class FacturXBusinessRule(string name, string description, FacturXProfileFlags profiles)
{
    /// <summary>
    ///     The name of the rule.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    ///     The description of the rule.
    /// </summary>
    public string Description { get; } = description;

    /// <summary>
    ///     The profiles in which the rule should be enforced.
    /// </summary>
    public FacturXProfileFlags Profiles { get; } = profiles;

    /// <summary>
    ///     Determines whether the invoice satisfies the conditions defined by the rule.
    /// </summary>
    /// <param name="invoice">The invoice to validate.</param>
    /// <returns><c>true</c> if the rule is satisfied by the invoice; otherwise <c>false</c>.</returns>
    public abstract bool Check(FacturXCrossIndustryInvoice invoice);
}
