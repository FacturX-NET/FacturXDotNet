namespace FacturXDotNet.Validation;

/// <summary>
///     Represents the result of validating a Factur-X Cross-Industry Invoice against business rules.
/// </summary>
/// <remarks>
///     This record contains the outcome of each business rule applied during the validation process.
///     It categorizes the rules into those that have passed, failed, or been skipped based on the validation result.
/// </remarks>
/// <param name="Passed">The collection of business rules that passed the validation.</param>
/// <param name="Failed">The collection of business rules that failed the validation.</param>
/// <param name="Skipped">The collection of business rules that were skipped during the validation.</param>
public record FacturXValidationResult(
    IReadOnlyCollection<FacturXBusinessRule> Passed,
    IReadOnlyCollection<FacturXBusinessRule> Failed,
    IReadOnlyCollection<FacturXBusinessRule> Skipped
)
{
    /// <summary>
    ///     Gets a value indicating whether the validation was successful.
    /// </summary>
    /// <remarks>
    ///     The validation is considered successful if no business rules have failed.
    /// </remarks>
    public bool Success => Failed.Count == 0;
}
