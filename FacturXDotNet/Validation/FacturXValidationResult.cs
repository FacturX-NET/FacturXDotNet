using FacturXDotNet.Models;
using FacturXDotNet.Validation.BusinessRules;

namespace FacturXDotNet.Validation;

/// <summary>
///     Represents the result of validating a Factur-X Cross-Industry Invoice against business rules.
/// </summary>
/// <remarks>
///     This record contains the outcome of each business rule applied during the validation process.
///     It categorizes the rules into those that have passed, failed, or been skipped based on the validation result.
/// </remarks>
/// <param name="Passed">The collection of business rules that passed the validation.</param>
/// <param name="Fatal">The collection of business rules that failed the validation.</param>
/// <param name="Warning">The collection of business rules that failed the validation with a warning.</param>
/// <param name="Information">The collection of business rules that failed the validation, but are informational.</param>
/// <param name="ExpectedToFail">
///     The collection of business rules that failed the validation, but were expected to fail because they target a profile that is higher than the one specified
///     in the document.
/// </param>
/// <param name="Skipped">The collection of business rules that were not checked.</param>
public readonly record struct FacturXValidationResult(
    IReadOnlyCollection<FacturXBusinessRule> Passed,
    IReadOnlyCollection<FacturXBusinessRule> Fatal,
    IReadOnlyCollection<FacturXBusinessRule> Warning,
    IReadOnlyCollection<FacturXBusinessRule> Information,
    IReadOnlyCollection<FacturXBusinessRule> ExpectedToFail,
    IReadOnlyCollection<FacturXBusinessRule> Skipped
)
{
    /// <summary>
    ///     Gets the profiles that are valid for the document.
    /// </summary>
    public FacturXProfileFlags ValidProfiles { get; } = ComputeActualProfile(Fatal, ExpectedToFail);

    /// <summary>
    ///     Gets a value indicating whether the validation was successful.
    /// </summary>
    /// <remarks>
    ///     The validation is considered successful if no business rules have failed, except those that were expected to fail.
    /// </remarks>
    public bool Success => Fatal.Count == 0;

    static FacturXProfileFlags ComputeActualProfile(IReadOnlyCollection<FacturXBusinessRule> failed, IReadOnlyCollection<FacturXBusinessRule> expectedToFail) =>
        failed.Concat(expectedToFail).Select(r => r.Profiles).Aggregate(FacturXProfileFlags.All, (result, failedProfiles) => result & ~failedProfiles);
}
