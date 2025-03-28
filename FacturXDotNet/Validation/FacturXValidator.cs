using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;
using FacturXDotNet.Parsing.CII;
using FacturXDotNet.Parsing.XMP;
using FacturXDotNet.Validation.BusinessRules;
using FacturXDotNet.Validation.BusinessRules.CII;
using FacturXDotNet.Validation.BusinessRules.Hybrid;
using FacturXDotNet.Validation.BusinessRules.Internals;
using CrossIndustryInvoiceBusinessRule = FacturXDotNet.Validation.BusinessRules.CII.CrossIndustryInvoiceBusinessRule;

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
    ///         Use <see cref="GetValidationResultAsync" /> if you need a detailed report of all rule evaluations.
    ///     </para>
    /// </remarks>
    /// <param name="invoice">The invoice to validate.</param>
    /// <param name="ciiAttachmentName">The name of the attachment containing the Cross-Industry Invoice XML file. If not specified, the default name 'factur-x.xml' will be used.</param>
    /// <param name="password">The password to open the PDF document.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><c>true</c> if the invoice meets all required business rules; otherwise, <c>false</c>.</returns>
    public async Task<bool> ValidateAsync(FacturXDocument invoice, string? ciiAttachmentName = null, string? password = null, CancellationToken cancellationToken = default)
    {
        ciiAttachmentName ??= "factur-x.xml";
        (XmpMetadata? xmp, CrossIndustryInvoiceAttachment? ciiAttachment) = await ExtractXmpAndCiiAsync(invoice, ciiAttachmentName, password, cancellationToken);
        if (xmp is null || ciiAttachment is null)
        {
            return false;
        }

        CrossIndustryInvoice cii = await ciiAttachment.GetCrossIndustryInvoiceAsync(password, cancellationToken: cancellationToken);
        FacturXProfile profile = GetExpectedProfile(xmp, cii);

        IEnumerable<HybridBusinessRule> hybridRules = HybridBusinessRules.Rules
            .Where(rule => rule.Severity is BusinessRuleSeverity.Fatal || rule.Severity is BusinessRuleSeverity.Warning && _options.TreatWarningsAsErrors)
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
    ///     Determines wheter the given invoice satisfies the specified business rule.
    /// </summary>
    /// <param name="invoice">The invoice to validate.</param>
    /// <param name="businessRuleName">The name of the business rule to validate, e.g. <c>BR-01</c>.</param>
    /// <param name="ciiAttachmentName">The name of the attachment containing the Cross-Industry Invoice XML file. If not specified, the default name 'factur-x.xml' will be used.</param>
    /// <param name="password">The password to open the PDF document.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><c>true</c> if the invoice meets the specified business rule; otherwise, <c>false</c>.</returns>
    public async Task<bool> ValidateRuleAsync(
        FacturXDocument invoice,
        string businessRuleName,
        string? ciiAttachmentName = null,
        string? password = null,
        CancellationToken cancellationToken = default
    )
    {
        IEnumerable<BusinessRule> allRules = HybridBusinessRules.Rules.Concat<BusinessRule>(CrossIndustryInvoiceBusinessRules.Rules);
        BusinessRule? rule = allRules.SingleOrDefault(r => r.Name == businessRuleName);
        if (rule is null)
        {
            throw new InvalidOperationException($"Could not find rule with name {businessRuleName}");
        }

        ciiAttachmentName ??= "factur-x.xml";

        return rule switch
        {
            CrossIndustryInvoiceBusinessRule ciiRule => await CheckCrossIndustryInvoiceBusinessRuleAsync(invoice, ciiRule, ciiAttachmentName, password, cancellationToken),
            HybridBusinessRule hybridRule => await CheckHybridRuleAsync(invoice, hybridRule, ciiAttachmentName, password, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(rule), rule, null)
        };
    }

    /// <summary>
    ///     Computes a detailed validation result for the given invoice.
    /// </summary>
    /// <remarks>
    ///     Unlike <see cref="ValidateAsync(FacturXDotNet.FacturXDocument,string?,string?,System.Threading.CancellationToken)" />, this method evaluates all business rules and categorizes
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
    /// <param name="invoice">The invoice to validate.</param>
    /// <param name="ciiAttachmentName">The name of the attachment containing the Cross-Industry Invoice XML file. If not specified, the default name 'factur-x.xml' will be used.</param>
    /// <param name="password">The password to open the PDF document.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     A <see cref="FacturXValidationResult" /> containing details of passed, failed, and skipped business rules.
    /// </returns>
    public async Task<FacturXValidationResult> GetValidationResultAsync(
        FacturXDocument invoice,
        string? ciiAttachmentName = null,
        string? password = null,
        CancellationToken cancellationToken = default
    )
    {
        ciiAttachmentName ??= "factur-x.xml";
        FacturXValidationResultBuilder builder = new();

        (XmpMetadata? xmp, CrossIndustryInvoiceAttachment? ciiAttachment) = await ExtractXmpAndCiiAsync(invoice, ciiAttachmentName, password, cancellationToken);

        CrossIndustryInvoice? cii = ciiAttachment is null
            ? null
            : await ciiAttachment.GetCrossIndustryInvoiceAsync(password, new CrossIndustryInvoiceReaderOptions { Logger = options?.Logger }, cancellationToken);

        FacturXProfile expectedProfile = GetExpectedProfile(xmp, cii);
        builder.SetExpectedProfile(expectedProfile);

        CheckHybridRules(builder, xmp, cii is null ? null : ciiAttachmentName, cii);
        CheckBusinessRules(builder, expectedProfile, cii);

        return builder.Build();
    }

    void CheckHybridRules(FacturXValidationResultBuilder builder, XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii)
    {
        foreach (HybridBusinessRule rule in HybridBusinessRules.Rules)
        {
            // Hybrid rules are always expected to pass
            const BusinessRuleExpectedValidationStatus expectation = BusinessRuleExpectedValidationStatus.Success;

            if (ShouldSkipRule(rule))
            {
                builder.AddRuleStatus(rule, expectation, BusinessRuleValidationStatus.Skipped, []);
            }
            else
            {
                BusinessRuleDetailsLogger logger = new();
                BusinessRuleValidationStatus status = rule.Check(xmp, ciiAttachmentName, cii, logger) ? BusinessRuleValidationStatus.Passed : BusinessRuleValidationStatus.Failed;
                builder.AddRuleStatus(rule, expectation, status, logger.GetDetails());
            }
        }
    }

    void CheckBusinessRules(FacturXValidationResultBuilder builder, FacturXProfile expectedProfile, CrossIndustryInvoice? cii)
    {
        foreach (CrossIndustryInvoiceBusinessRule rule in CrossIndustryInvoiceBusinessRules.Rules)
        {
            // Hybrid rules are always expected to pass
            BusinessRuleExpectedValidationStatus expectation =
                IsRuleExpectedToFail(rule, expectedProfile) ? BusinessRuleExpectedValidationStatus.Failure : BusinessRuleExpectedValidationStatus.Success;

            if (ShouldSkipRule(rule))
            {
                builder.AddRuleStatus(rule, expectation, BusinessRuleValidationStatus.Skipped, []);
            }
            else
            {
                BusinessRuleDetailsLogger logger = new();
                BusinessRuleValidationStatus status = rule.Check(cii) ? BusinessRuleValidationStatus.Passed : BusinessRuleValidationStatus.Failed;
                builder.AddRuleStatus(rule, expectation, status, logger.GetDetails());
            }
        }
    }

    FacturXProfile GetExpectedProfile(XmpMetadata? xmp, CrossIndustryInvoice? cii) =>
        _options.ProfileOverride.HasValue && _options.ProfileOverride is not FacturXProfile.None
            ? _options.ProfileOverride.Value
            : xmp?.FacturX?.ConformanceLevel?.ToFacturXProfile()
              ?? cii?.ExchangedDocumentContext?.GuidelineSpecifiedDocumentContextParameterId?.ToFacturXProfileOrNull() ?? FacturXProfile.None;

    static bool IsRuleExpectedToFail(CrossIndustryInvoiceBusinessRule rule, FacturXProfile profile) => !rule.Profiles.Match(profile);

    bool ShouldSkipRule(CrossIndustryInvoiceBusinessRule rule) => _options.RulesToSkip.Any(r => string.Equals(rule.Name, r, StringComparison.InvariantCultureIgnoreCase));

    bool ShouldSkipRule(HybridBusinessRule rule) => _options.RulesToSkip.Any(r => string.Equals(rule.Name, r, StringComparison.InvariantCultureIgnoreCase));

    async Task<bool> CheckHybridRuleAsync(FacturXDocument invoice, HybridBusinessRule hybridRule, string ciiAttachmentName, string? password, CancellationToken cancellationToken)
    {
        (XmpMetadata? xmp, CrossIndustryInvoiceAttachment? ciiAttachment) = await ExtractXmpAndCiiAsync(invoice, ciiAttachmentName, password, cancellationToken);
        if (xmp is null || ciiAttachment is null)
        {
            return false;
        }

        CrossIndustryInvoice cii = await ciiAttachment.GetCrossIndustryInvoiceAsync(password, cancellationToken: cancellationToken);

        return hybridRule.Check(xmp, ciiAttachmentName, cii);
    }

    static async Task<bool> CheckCrossIndustryInvoiceBusinessRuleAsync(
        FacturXDocument invoice,
        CrossIndustryInvoiceBusinessRule ciiRule,
        string ciiAttachmentName,
        string? password,
        CancellationToken cancellationToken
    )
    {
        CrossIndustryInvoiceAttachment? ciiAttachment = await invoice.GetCrossIndustryInvoiceAttachmentAsync(ciiAttachmentName, password, cancellationToken);
        CrossIndustryInvoice? cii = ciiAttachment is null ? null : await ciiAttachment.GetCrossIndustryInvoiceAsync(password, cancellationToken: cancellationToken);
        return cii is not null && ciiRule.Check(cii);
    }

    async Task<(XmpMetadata? Xmp, CrossIndustryInvoiceAttachment? Cii)> ExtractXmpAndCiiAsync(
        FacturXDocument invoice,
        string ciiAttachmentName,
        string? password,
        CancellationToken cancellationToken
    )
    {
        XmpMetadata? xmp = await invoice.GetXmpMetadataAsync(password, new XmpMetadataReaderOptions { Logger = options?.Logger }, cancellationToken);
        CrossIndustryInvoiceAttachment? cii = await invoice.GetCrossIndustryInvoiceAttachmentAsync(ciiAttachmentName, password, cancellationToken);

        return (xmp, cii);
    }
}
