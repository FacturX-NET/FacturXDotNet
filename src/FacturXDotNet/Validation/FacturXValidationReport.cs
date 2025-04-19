using FacturXDotNet.Models;

namespace FacturXDotNet.Validation;

/// <summary>
///     Represents the result of validating a Factur-X Cross-Industry Invoice against business rules.
/// </summary>
/// <remarks>
///     This record contains the outcome of each business rule applied during the validation process.
///     It categorizes the rules into those that have passed, failed, or been skipped based on the validation result.
/// </remarks>
/// <param name="ExpectedProfile">The profile that was expected for the document. This is the profile that is specified in the document.</param>
/// <param name="Rules">The validation status of each business rule.</param>
public readonly record struct FacturXValidationReport(FacturXProfile ExpectedProfile, IReadOnlyList<BusinessRuleValidationResult> Rules)
{
    /// <summary>
    ///     The profiles that are valid for the document.
    /// </summary>
    public FacturXProfileFlags ValidProfiles { get; } = ComputeActualProfile(Rules);

    /// <summary>
    ///     Whether the validation was successful.
    /// </summary>
    /// <remarks>
    ///     The validation is considered successful if no business rules have failed, except those that were expected to fail.
    /// </remarks>
    public bool Success => Rules.All(r => r.ExpectedStatus is BusinessRuleExpectedValidationStatus.Failure || r.Status is not BusinessRuleValidationStatus.Failed);

    static FacturXProfileFlags ComputeActualProfile(IReadOnlyList<BusinessRuleValidationResult> rules) =>
        rules.Where(r => r.Status is BusinessRuleValidationStatus.Failed)
            .Select(r => r.Rule.Profiles)
            .Aggregate(FacturXProfileFlags.All, (result, failedProfiles) => result & ~failedProfiles);
}
