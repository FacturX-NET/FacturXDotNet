namespace FacturXDotNet.Validation;

/// <summary>
///     Represent the expected validation status of a business rule. Some rules are expected to fail because they are associated with a higher profile.
/// </summary>
public enum BusinessRuleExpectedValidationStatus
{
    /// <summary>
    ///     The rule is expected to pass.
    /// </summary>
    Success,

    /// <summary>
    ///     The rule is expected to fail.
    /// </summary>
    Failure
}
