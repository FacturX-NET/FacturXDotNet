using FacturXDotNet.Validation;
using FacturXDotNet.Validation.BusinessRules.CII;

namespace FacturXDotNet.API.Features.Validate;

static class ValidationResults
{
    public static IEnumerable<KeyValuePair<string, string[]>> BuildErrors(FacturXValidationResult validationResult)
    {
        Dictionary<string, List<string>> errors = new();

        foreach (BusinessRuleValidationResult failure in validationResult.Rules.Where(r => r.HasFailed))
        {
            if (failure.Rule is not CrossIndustryInvoiceBusinessRule ciiRule)
            {
                continue;
            }

            foreach (string fieldInvolved in ciiRule.TermsInvolved)
            {
                if (!errors.TryGetValue(fieldInvolved, out List<string>? failedRules))
                {
                    failedRules = [];
                    errors[fieldInvolved] = failedRules;
                }

                failedRules.Add(ciiRule.Name);
            }
        }

        return errors.Select(kv => new KeyValuePair<string, string[]>(kv.Key, kv.Value.ToArray()));
    }
}
