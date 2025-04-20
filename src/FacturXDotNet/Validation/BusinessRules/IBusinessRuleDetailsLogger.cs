namespace FacturXDotNet.Validation.BusinessRules;

/// <summary>
///     Logger that logs the details of business rules.
/// </summary>
public interface IBusinessRuleDetailsLogger
{
    /// <summary>
    ///     Add an information message to the logger.
    /// </summary>
    /// <param name="message">The message to log.</param>
    void LogInformation(string message);

    /// <summary>
    ///     Add an information message to the logger.
    /// </summary>
    /// <param name="message">The message to log.</param>
    void LogWarning(string message);

    /// <summary>
    ///     Add an information message to the logger.
    /// </summary>
    /// <param name="message">The message to log.</param>
    void LogError(string message);
}
