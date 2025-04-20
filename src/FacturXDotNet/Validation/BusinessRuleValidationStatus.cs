namespace FacturXDotNet.Validation;

/// <summary>
///     Represents the validation status of a business rule.
/// </summary>
public enum BusinessRuleValidationStatus
{
    /// <summary>
    ///     The rule passed.
    /// </summary>
    Passed,

    /// <summary>
    ///     The rule failed.
    /// </summary>
    Failed,

    /// <summary>
    ///     The rule was skipped.
    /// </summary>
    Skipped
}
