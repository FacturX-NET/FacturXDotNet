namespace FacturXDotNet.Validation.BusinessRules;

/// <summary>
///     The severity of a business rule.
/// </summary>
public enum BusinessRuleSeverity
{
    /// <summary>
    ///     Information rules are not critical and will not prevent the document from being considered valid.
    /// </summary>
    Information,

    /// <summary>
    ///     Warning rules are important but not critical and will not prevent the document from being considered valid.
    /// </summary>
    Warning,

    /// <summary>
    ///     Fatal rules are critical and will prevent the document from being considered valid.
    /// </summary>
    Fatal
}
