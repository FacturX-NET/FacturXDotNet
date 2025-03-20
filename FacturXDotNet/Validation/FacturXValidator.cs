using FacturXDotNet.Models;
using FacturXDotNet.Validation.BusinessRules;
using FacturXDotNet.Validation.BusinessRules.CII;
using FacturXDotNet.Validation.BusinessRules.Hybrid;

namespace FacturXDotNet.Validation;

/// <summary>
///     Validates a Factur-X against the business rules defined in the Factur-X specification.
/// </summary>
/// <remarks>
///     This class does not perform Schematron validation for performance reasons. All the rules of the schematron have been reimplemented in plain C# code.
/// </remarks>
public class FacturXValidator(FacturXValidationOptions? options = null)
{
    readonly FacturXValidationOptions _options = options ?? new FacturXValidationOptions();

    /// <summary>
    ///     Determines whether the given invoice satisfies all applicable business rules.
    /// </summary>
    /// <remarks>
    ///     This method applies business rules and returns <c>true</c> if all are satisfied; otherwise, <c>false</c>.
    ///     Validation stops at the first failing rule for efficiency.
    ///     <para>
    ///         Use <see cref="GetValidationResult" /> if you need a detailed report of all rule evaluations.
    ///     </para>
    /// </remarks>
    /// <param name="invoice">The invoice to validate.</param>
    /// <returns><c>true</c> if the invoice meets all required business rules; otherwise, <c>false</c>.</returns>
    public bool IsValid(FacturX invoice)
    {
        FacturXProfile profile = GetExpectedProfile(invoice);
        IEnumerable<FacturXBusinessRule> rules = CrossIndustryInvoiceBusinessRules.Rules.Concat<FacturXBusinessRule>(HybridBusinessRules.Rules)
            .Where(rule => rule.Severity is FacturXBusinessRuleSeverity.Fatal || rule.Severity is FacturXBusinessRuleSeverity.Warning && _options.TreatWarningsAsErrors)
            .Where(rule => !ShouldSkipRule(rule))
            .Where(rule => rule.Profiles.Match(profile));

        return rules.All(rule => rule.Check(invoice));
    }

    /// <summary>
    ///     Computes a detailed validation result for the given invoice.
    /// </summary>
    /// <remarks>
    ///     Unlike <see cref="IsValid" />, this method evaluates all business rules and categorizes them as:
    ///     <list type="bullet">
    ///         <item>
    ///             <description><b>Passed</b> - Rules that were successfully met.</description>
    ///         </item>
    ///         <item>
    ///             <description><b>Failed</b> - Rules that were not satisfied.</description>
    ///         </item>
    ///         <item>
    ///             <description><b>Skipped</b> - Rules that were not applicable for the selected Factur-X profile.</description>
    ///         </item>
    ///     </list>
    ///     This method provides full validation at the cost of additional computation time and memory usage.
    /// </remarks>
    /// <param name="invoice">The invoice to validate.</param>
    /// <returns>
    ///     A <see cref="FacturXValidationResult" /> containing details of passed, failed, and skipped business rules.
    /// </returns>
    public FacturXValidationResult GetValidationResult(FacturX invoice)
    {
        IEnumerable<FacturXBusinessRule> rules = CrossIndustryInvoiceBusinessRules.Rules.Concat<FacturXBusinessRule>(HybridBusinessRules.Rules);
        (List<FacturXBusinessRule> passed, List<FacturXBusinessRule> failed, List<FacturXBusinessRule> skipped) = CheckRules(rules, invoice);
        return BuildValidationResult(invoice, failed, passed, skipped);
    }

    (List<FacturXBusinessRule>, List<FacturXBusinessRule>, List<FacturXBusinessRule>) CheckRules(IEnumerable<FacturXBusinessRule> rules, FacturX invoice)
    {
        List<FacturXBusinessRule> passed = [];
        List<FacturXBusinessRule> failed = [];
        List<FacturXBusinessRule> skipped = [];

        foreach (FacturXBusinessRule rule in rules)
        {
            if (ShouldSkipRule(rule))
            {
                skipped.Add(rule);
                continue;
            }

            if (rule.Check(invoice))
            {
                passed.Add(rule);
            }
            else
            {
                failed.Add(rule);
            }
        }

        return (passed, failed, skipped);
    }

    FacturXValidationResult BuildValidationResult(FacturX invoice, List<FacturXBusinessRule> failed, List<FacturXBusinessRule> passed, List<FacturXBusinessRule> skipped)
    {
        FacturXProfile profile = GetExpectedProfile(invoice);

        List<FacturXBusinessRule> fatal = [];
        List<FacturXBusinessRule> warning = [];
        List<FacturXBusinessRule> information = [];
        List<FacturXBusinessRule> expectedToFail = [];

        foreach (FacturXBusinessRule failedRule in failed)
        {
            if (!failedRule.Profiles.Match(profile))
            {
                expectedToFail.Add(failedRule);
                continue;
            }

            switch (failedRule.Severity)
            {
                case FacturXBusinessRuleSeverity.Fatal:
                case FacturXBusinessRuleSeverity.Warning when _options.TreatWarningsAsErrors:
                    fatal.Add(failedRule);
                    break;
                case FacturXBusinessRuleSeverity.Warning:
                    warning.Add(failedRule);
                    break;
                case FacturXBusinessRuleSeverity.Information:
                    information.Add(failedRule);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(failedRule.Severity), failedRule.Severity, null);
            }
        }

        return new FacturXValidationResult(passed, fatal, warning, information, expectedToFail, skipped);
    }

    bool ShouldSkipRule(FacturXBusinessRule rule) => _options.RulesToSkip.Any(r => string.Equals(rule.Name, r, StringComparison.InvariantCultureIgnoreCase));

    FacturXProfile GetExpectedProfile(FacturX invoice) =>
        _options.ProfileOverride
        ?? invoice.XmpMetadata.FacturX?.ConformanceLevel?.ToFacturXProfile()
        ?? invoice.CrossIndustryInvoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId.ToFacturXProfileOrNull() ?? FacturXProfile.Minimum;
}
