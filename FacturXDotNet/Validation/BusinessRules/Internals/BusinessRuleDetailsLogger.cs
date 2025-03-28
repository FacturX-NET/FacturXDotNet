namespace FacturXDotNet.Validation.BusinessRules.Internals;

/// <summary>
///     A logger that stores additional details about business rule validation.
/// </summary>
class BusinessRuleDetailsLogger : IBusinessRuleDetailsLogger
{
    readonly List<BusinessRuleDetail> _details = [];

    /// <summary>
    ///     Add an information message to the logger.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void LogInformation(string message) => AddDetail(BusinessRuleDetailSeverity.Information, message);

    /// <summary>
    ///     Add an information message to the logger.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void LogWarning(string message) => AddDetail(BusinessRuleDetailSeverity.Warning, message);

    /// <summary>
    ///     Add an information message to the logger.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void LogError(string message) => AddDetail(BusinessRuleDetailSeverity.Fatal, message);

    public IReadOnlyList<BusinessRuleDetail> GetDetails() => _details;

    void AddDetail(BusinessRuleDetailSeverity severity, string message) => _details.Add(new BusinessRuleDetail(severity, message.EndsWith('.') ? message : $"{message}."));
}
