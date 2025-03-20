namespace FacturXDotNet.Validation;

/// <summary>
///     Represents a business rule for validating a Factur-X Cross-Industry Invoice.
/// </summary>
/// <remarks>
///     This abstract class defines the structure for creating specific business rules that can be applied to an invoice.
///     Subclasses must implement the <see cref="Check" /> method to define the specific validation logic.
/// </remarks>
/// <param name="Name">The name of the rule.</param>
/// <param name="Description">A description of the rule.</param>
/// <param name="Profiles">The profiles in which this rule should be enforced.</param>
public abstract record FacturXBusinessRule(string Name, string Description, FacturXProfileFlags Profiles)
{
    /// <summary>
    ///     Determines whether the invoice satisfies the conditions defined by the rule.
    /// </summary>
    /// <param name="invoice">The invoice to validate.</param>
    /// <returns><c>true</c> if the rule is satisfied by the invoice; otherwise <c>false</c>.</returns>
    public abstract bool Check(CrossIndustryInvoice invoice);

    /// <summary>
    ///     Returns a string representation of the business rule.
    /// </summary>
    public override string ToString() => $"[{Profiles.GetMinProfile()}] {Name} - {Description}";
}
