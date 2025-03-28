namespace FacturXDotNet.Validation.BusinessRules;

/// <summary>
///     A detail that was logged during the validation of a business rule.
/// </summary>
/// <param name="Severity">The severity of the detail.</param>
/// <param name="Message">The message of the detail.</param>
public readonly record struct BusinessRuleDetail(BusinessRuleDetailSeverity Severity, string Message);
