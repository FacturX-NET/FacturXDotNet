using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Validation.BusinessRules.CII;

namespace FacturXDotNet.Validation;

/// <summary>
///     Validates a <see cref="CrossIndustryInvoice" /> instance against the business rules defined in the Factur-X specification.
/// </summary>
/// <remarks>
///     This class does not perform Schematron validation for performance reasons. All the rules of the schematron have been reimplemented in plain C# code.
/// </remarks>
public class CrossIndustryInvoiceValidator(CrossIndustryInvoiceValidationOptions? options = null)
{
    readonly CrossIndustryInvoiceValidationOptions _options = options ?? new CrossIndustryInvoiceValidationOptions();

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
    /// <param name="cii">The invoice to validate.</param>
    /// <returns><c>true</c> if the invoice meets all required business rules; otherwise, <c>false</c>.</returns>
    public bool Validate(CrossIndustryInvoice cii)
    {
        FacturXProfile profile = GetExpectedProfile(cii);

        IEnumerable<CrossIndustryInvoiceBusinessRule> businessRules = CrossIndustryInvoiceBusinessRules.Rules
            .Where(rule => !ShouldSkipRule(rule))
            .Where(rule => rule.Profiles.Match(profile));
        if (!businessRules.All(rule => rule.Check(cii)))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    ///     Determines wheter the given invoice satisfies the specified business rule.
    /// </summary>
    /// <param name="cii">The invoice to validate.</param>
    /// <param name="businessRuleName">The name of the business rule to validate, e.g. <c>BR-01</c>.</param>
    /// <returns><c>true</c> if the invoice meets the specified business rule; otherwise, <c>false</c>.</returns>
    public bool ValidateRule(CrossIndustryInvoice cii, string businessRuleName)
    {
        CrossIndustryInvoiceBusinessRule? rule = CrossIndustryInvoiceBusinessRules.Rules.SingleOrDefault(r => r.Name == businessRuleName);
        if (rule is null)
        {
            throw new InvalidOperationException($"Could not find rule with name {businessRuleName}");
        }

        return rule.Check(cii);
    }

    /// <summary>
    ///     Computes a detailed validation result for the given invoice.
    /// </summary>
    /// <remarks>
    ///     Unlike <see cref="Validate(CrossIndustryInvoice)" />, this method evaluates all business rules and categorizes
    ///     them as:
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
    /// <param name="cii">The invoice to validate.</param>
    /// <returns>
    ///     A <see cref="FacturXValidationResult" /> containing details of passed, failed, and skipped business rules.
    /// </returns>
    public FacturXValidationResult GetValidationResult(CrossIndustryInvoice cii)
    {
        FacturXValidationResultBuilder builder = new();

        FacturXProfile expectedProfile = GetExpectedProfile(cii);
        builder.SetExpectedProfile(expectedProfile);

        ValidationUtils.CheckBusinessRules(builder, expectedProfile, cii, _options.CheckCallback, _options.RulesToSkip);

        return builder.Build();
    }

    bool ShouldSkipRule(CrossIndustryInvoiceBusinessRule rule) => _options.RulesToSkip.Any(r => string.Equals(rule.Name, r, StringComparison.InvariantCultureIgnoreCase));

    FacturXProfile GetExpectedProfile(CrossIndustryInvoice? cii) =>
        _options.ProfileOverride.HasValue && _options.ProfileOverride is not FacturXProfile.None
            ? _options.ProfileOverride.Value
            : cii?.ExchangedDocumentContext?.GuidelineSpecifiedDocumentContextParameterId?.ToFacturXProfileOrNull() ?? FacturXProfile.None;
}
