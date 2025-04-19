using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;
using FacturXDotNet.Validation.BusinessRules;
using FacturXDotNet.Validation.BusinessRules.CII;
using FacturXDotNet.Validation.BusinessRules.Hybrid;
using FacturXDotNet.Validation.BusinessRules.Internals;

namespace FacturXDotNet.Validation;

static class ValidationUtils
{
    public static bool ValidateHybridRules(
        XmpMetadata xmp,
        CrossIndustryInvoice cii,
        CrossIndustryInvoiceAttachment ciiAttachment,
        List<string> rulesToSkip,
        bool treatWarningsAsErrors
    ) =>
        HybridBusinessRules.Rules.Where(rule => rule.Severity is BusinessRuleSeverity.Fatal || rule.Severity is BusinessRuleSeverity.Warning && treatWarningsAsErrors)
            .Where(rule => !ShouldSkipRule(rule, rulesToSkip))
            .All(rule => rule.Check(xmp, ciiAttachment.Name, cii));

    public static bool ValidateBusinessRules(CrossIndustryInvoice cii, FacturXProfile expectedProfile, List<string> rulesToSkip) =>
        CrossIndustryInvoiceBusinessRules.Rules.Where(rule => !ShouldSkipRule(rule, rulesToSkip)).Where(rule => rule.Profiles.Match(expectedProfile)).All(rule => rule.Check(cii));

    public static void CheckHybridRules(
        FacturXValidationResultBuilder builder,
        XmpMetadata? xmp,
        string? ciiAttachmentName,
        CrossIndustryInvoice? cii,
        Action<BusinessRuleValidationResult>? checkCallback,
        List<string> rulesToSkip
    )
    {
        foreach (HybridBusinessRule rule in HybridBusinessRules.Rules)
        {
            // Hybrid rules are always expected to pass
            const BusinessRuleExpectedValidationStatus expectation = BusinessRuleExpectedValidationStatus.Success;

            if (ShouldSkipRule(rule, rulesToSkip))
            {
                builder.AddRuleStatus(rule, expectation, BusinessRuleValidationStatus.Skipped, []);
            }
            else
            {
                BusinessRuleDetailsLogger logger = new();
                BusinessRuleValidationStatus status = rule.Check(xmp, ciiAttachmentName, cii, logger) ? BusinessRuleValidationStatus.Passed : BusinessRuleValidationStatus.Failed;
                BusinessRuleValidationResult result = builder.AddRuleStatus(rule, expectation, status, logger.GetDetails());
                checkCallback?.Invoke(result);
            }
        }
    }

    public static void CheckBusinessRules(
        FacturXValidationResultBuilder builder,
        FacturXProfile expectedProfile,
        CrossIndustryInvoice? cii,
        Action<BusinessRuleValidationResult>? checkCallback,
        List<string> rulesToSkip
    )
    {
        foreach (CrossIndustryInvoiceBusinessRule rule in CrossIndustryInvoiceBusinessRules.Rules)
        {
            BusinessRuleExpectedValidationStatus expectation =
                IsRuleExpectedToFail(rule, expectedProfile) ? BusinessRuleExpectedValidationStatus.Failure : BusinessRuleExpectedValidationStatus.Success;

            if (ShouldSkipRule(rule, rulesToSkip))
            {
                builder.AddRuleStatus(rule, expectation, BusinessRuleValidationStatus.Skipped, []);
            }
            else
            {
                BusinessRuleDetailsLogger logger = new();
                BusinessRuleValidationStatus status = rule.Check(cii) ? BusinessRuleValidationStatus.Passed : BusinessRuleValidationStatus.Failed;
                BusinessRuleValidationResult result = builder.AddRuleStatus(rule, expectation, status, logger.GetDetails());
                checkCallback?.Invoke(result);
            }
        }
    }

    static bool ShouldSkipRule(HybridBusinessRule rule, List<string> rulesToSkip) => rulesToSkip.Any(r => string.Equals(rule.Name, r, StringComparison.InvariantCultureIgnoreCase));

    static bool ShouldSkipRule(CrossIndustryInvoiceBusinessRule rule, List<string> rulesToSkip) =>
        rulesToSkip.Any(r => string.Equals(rule.Name, r, StringComparison.InvariantCultureIgnoreCase));

    static bool IsRuleExpectedToFail(CrossIndustryInvoiceBusinessRule rule, FacturXProfile profile) => !rule.Profiles.Match(profile);
}
