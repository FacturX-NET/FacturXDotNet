using FacturXDotNet.Validation.BusinessRules;

namespace FacturXDotNet.Validation;

/// <summary>
///     The result of a business rule validation.
/// </summary>
/// <param name="Rule">The rule that was validated.</param>
/// <param name="ExpectedStatus">The expected status of the rule.</param>
/// <param name="Status">The actual status of the rule.</param>
public readonly record struct BusinessRuleValidationResult(
    BusinessRule Rule,
    BusinessRuleExpectedValidationStatus ExpectedStatus,
    BusinessRuleValidationStatus Status,
    IReadOnlyList<BusinessRuleDetail> Details
)
{
    /// <summary>
    ///     Returns true if the validation has failed, i.e., the rule was not expected to fail, and it failed.
    /// </summary>
    public bool HasFailed =>
        Rule.Severity is BusinessRuleSeverity.Fatal && ExpectedStatus is not BusinessRuleExpectedValidationStatus.Failure && Status is BusinessRuleValidationStatus.Failed;
}
