using FacturXDotNet.Models;
using FacturXDotNet.Parsing.CII;
using FacturXDotNet.Parsing.XMP;
using FacturXDotNet.Validation.BusinessRules;
using FacturXDotNet.Validation.BusinessRules.CII;
using FacturXDotNet.Validation.BusinessRules.Hybrid;
using CrossIndustryInvoiceBusinessRule = FacturXDotNet.Validation.BusinessRules.CrossIndustryInvoiceBusinessRule;

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
    ///         Use <see cref="ValidateAsync" /> if you need a detailed report of all rule evaluations.
    ///     </para>
    /// </remarks>
    /// <param name="invoice">The invoice to validate.</param>
    /// <param name="ciiAttachmentName">The name of the attachment containing the Cross-Industry Invoice XML file. If not specified, the default name 'factur-x.xml' will be used.</param>
    /// <param name="password">The password to open the PDF document.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><c>true</c> if the invoice meets all required business rules; otherwise, <c>false</c>.</returns>
    public async Task<bool> ValidateFastAsync(FacturXDocument invoice, string? ciiAttachmentName = null, string? password = null, CancellationToken cancellationToken = default)
    {
        XmpMetadata? xmp = await invoice.GetXmpMetadataAsync(password, new XmpMetadataParserOptions { Logger = options?.Logger }, cancellationToken);
        if (xmp == null)
        {
            return false;
        }

        ciiAttachmentName ??= "factur-x.xml";
        CrossIndustryInvoiceAttachment? ciiAttachment = await invoice.GetCrossIndustryInvoiceAttachmentAsync(ciiAttachmentName, password, cancellationToken);
        if (ciiAttachment == null)
        {
            return false;
        }

        CrossIndustryInvoice cii = await ciiAttachment.GetCrossIndustryInvoiceAsync(password, cancellationToken: cancellationToken);
        FacturXProfile profile = GetExpectedProfile(xmp, cii);

        IEnumerable<HybridBusinessRule> hybridRules = HybridBusinessRules.Rules
            .Where(rule => rule.Severity is FacturXBusinessRuleSeverity.Fatal || rule.Severity is FacturXBusinessRuleSeverity.Warning && _options.TreatWarningsAsErrors)
            .Where(rule => !ShouldSkipRule(rule));
        if (!hybridRules.All(rule => rule.Check(xmp, ciiAttachment.Name, cii)))
        {
            return false;
        }

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
    ///     Computes a detailed validation result for the given invoice.
    /// </summary>
    /// <remarks>
    ///     Unlike <see cref="ValidateFastAsync" />, this method evaluates all business rules and categorizes them as:
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
    /// <param name="ciiAttachmentName">The name of the attachment containing the Cross-Industry Invoice XML file. If not specified, the default name 'factur-x.xml' will be used.</param>
    /// <param name="password">The password to open the PDF document.</param>
    /// <param name="progress">An optional progress reporter for tracking the validation progress.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     A <see cref="FacturXValidationReport" /> containing details of passed, failed, and skipped business rules.
    /// </returns>
    public async Task<FacturXValidationReport> ValidateAsync(
        FacturXDocument invoice,
        string? ciiAttachmentName = null,
        string? password = null,
        IProgress<FacturXValidationProgressArgs>? progress = null,
        CancellationToken cancellationToken = default
    )
    {
        ciiAttachmentName ??= "factur-x.xml";

        (XmpMetadata? xmp, CrossIndustryInvoiceAttachment? ciiAttachment) = await ExtractXmpAndCiiAsync(invoice, ciiAttachmentName, password, cancellationToken);

        CrossIndustryInvoice? cii = ciiAttachment == null
            ? null
            : await ciiAttachment.GetCrossIndustryInvoiceAsync(password, new CrossIndustryInvoiceParserOptions { Logger = options?.Logger }, cancellationToken);

        FacturXProfile expectedProfile = GetExpectedProfile(xmp, cii);

        BusinessRule[] allRules = HybridBusinessRules.Rules.Concat<BusinessRule>(CrossIndustryInvoiceBusinessRules.Rules).ToArray();
        List<BusinessRuleValidationResult> results = [];

        Progress<BusinessRuleValidationResult>? hybridProgress = progress == null
            ? null
            : new Progress<BusinessRuleValidationResult>(
                lastResult => progress.Report(new FacturXValidationProgressArgs { Rules = allRules, Results = results, LastResult = lastResult })
            );
        CheckRules(results, HybridBusinessRules.Rules, xmp, cii == null ? null : ciiAttachmentName, cii, hybridProgress);

        Progress<BusinessRuleValidationResult>? ciiProgress = progress == null
            ? null
            : new Progress<BusinessRuleValidationResult>(
                lastResult => progress.Report(new FacturXValidationProgressArgs { Rules = allRules, Results = results, LastResult = lastResult })
            );
        CheckRules(results, CrossIndustryInvoiceBusinessRules.Rules, expectedProfile, cii, ciiProgress);

        return new FacturXValidationReport(expectedProfile, results);
    }

    void CheckRules(
        List<BusinessRuleValidationResult> results,
        HybridBusinessRule[] rules,
        XmpMetadata? xmp,
        string? ciiAttachmentName,
        CrossIndustryInvoice? cii,
        IProgress<BusinessRuleValidationResult>? progress = null
    )
    {
        foreach (HybridBusinessRule rule in rules)
        {
            // Hybrid rules are always expected to pass
            const BusinessRuleExpectedValidationStatus expectation = BusinessRuleExpectedValidationStatus.Success;

            BusinessRuleValidationResult ruleResult;
            if (ShouldSkipRule(rule))
            {
                ruleResult = new BusinessRuleValidationResult(rule, expectation, BusinessRuleValidationStatus.Skipped);
                results.Add(ruleResult);
            }
            else
            {
                BusinessRuleValidationStatus status = rule.Check(xmp, ciiAttachmentName, cii) ? BusinessRuleValidationStatus.Passed : BusinessRuleValidationStatus.Failed;
                ruleResult = new BusinessRuleValidationResult(rule, expectation, status);
                results.Add(ruleResult);
            }

            progress?.Report(ruleResult);
        }
    }

    void CheckRules(
        List<BusinessRuleValidationResult> results,
        CrossIndustryInvoiceBusinessRule[] rules,
        FacturXProfile expectedProfile,
        CrossIndustryInvoice? cii,
        IProgress<BusinessRuleValidationResult>? progress = null
    )
    {
        foreach (CrossIndustryInvoiceBusinessRule rule in rules)
        {
            // Hybrid rules are always expected to pass
            BusinessRuleExpectedValidationStatus expectation =
                IsRuleExpectedToFail(rule, expectedProfile) ? BusinessRuleExpectedValidationStatus.Failure : BusinessRuleExpectedValidationStatus.Success;

            BusinessRuleValidationResult ruleResult;
            if (ShouldSkipRule(rule))
            {
                ruleResult = new BusinessRuleValidationResult(rule, expectation, BusinessRuleValidationStatus.Skipped);
                results.Add(ruleResult);
            }
            else
            {
                BusinessRuleValidationStatus status = rule.Check(cii) ? BusinessRuleValidationStatus.Passed : BusinessRuleValidationStatus.Failed;
                ruleResult = new BusinessRuleValidationResult(rule, expectation, status);
                results.Add(ruleResult);
            }

            progress?.Report(ruleResult);
        }
    }

    FacturXProfile GetExpectedProfile(XmpMetadata? xmp, CrossIndustryInvoice? cii) =>
        _options.ProfileOverride.HasValue && _options.ProfileOverride is not FacturXProfile.None
            ? _options.ProfileOverride.Value
            : xmp?.FacturX?.ConformanceLevel?.ToFacturXProfile()
              ?? cii?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId.ToFacturXProfileOrNull() ?? FacturXProfile.None;

    async Task<(XmpMetadata? Xmp, CrossIndustryInvoiceAttachment? Cii)> ExtractXmpAndCiiAsync(
        FacturXDocument invoice,
        string ciiAttachmentName,
        string? password,
        CancellationToken cancellationToken
    )
    {
        XmpMetadata? xmp = await invoice.GetXmpMetadataAsync(password, new XmpMetadataParserOptions { Logger = options?.Logger }, cancellationToken);
        CrossIndustryInvoiceAttachment? cii = await invoice.GetCrossIndustryInvoiceAttachmentAsync(ciiAttachmentName, password, cancellationToken);

        return (xmp, cii);
    }

    static bool IsRuleExpectedToFail(CrossIndustryInvoiceBusinessRule rule, FacturXProfile profile) => !rule.Profiles.Match(profile);
    bool ShouldSkipRule(CrossIndustryInvoiceBusinessRule rule) => _options.RulesToSkip.Any(r => string.Equals(rule.Name, r, StringComparison.InvariantCultureIgnoreCase));
    bool ShouldSkipRule(HybridBusinessRule rule) => _options.RulesToSkip.Any(r => string.Equals(rule.Name, r, StringComparison.InvariantCultureIgnoreCase));
}

/// <summary>
///     The progress of a Factur-X validation.
/// </summary>
public class FacturXValidationProgressArgs
{
    internal FacturXValidationProgressArgs() { }

    /// <summary>
    ///     All the rules.
    /// </summary>
    public required IReadOnlyCollection<BusinessRule> Rules { get; init; }

    /// <summary>
    ///     The rules that were evaluated.
    /// </summary>
    public required IReadOnlyList<BusinessRuleValidationResult> Results { get; init; }

    /// <summary>
    ///     The last rule that was evaluated.
    /// </summary>
    public required BusinessRuleValidationResult? LastResult { get; init; }
}
