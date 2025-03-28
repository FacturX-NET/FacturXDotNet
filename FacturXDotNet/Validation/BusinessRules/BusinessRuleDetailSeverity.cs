namespace FacturXDotNet.Validation.BusinessRules;

/// <summary>
///     The severity of a business rule detail.
/// </summary>
public enum BusinessRuleDetailSeverity
{
    /// <summary>
    ///     Something that is not important to note in most cases, but can be useful when debugging.
    /// </summary>
    Trace,

    /// <summary>
    ///     Something that is interesting to note, but does not affect the rule.
    /// </summary>
    Information,

    /// <summary>
    ///     Something that did not cause the rule to fail, but is still important to know.
    /// </summary>
    Warning,

    /// <summary>
    ///     Something that caused the rule to fail.
    /// </summary>
    Fatal
}
