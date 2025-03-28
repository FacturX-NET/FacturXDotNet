using FacturXDotNet.Models;
using FacturXDotNet.Validation.BusinessRules;

namespace FacturXDotNet.Validation;

class FacturXValidationResultBuilder
{
    readonly List<BusinessRuleValidationResult> _results = [];
    FacturXProfile? _expectedProfile;

    public void SetExpectedProfile(FacturXProfile profile) => _expectedProfile = profile;

    public BusinessRuleValidationResult AddRuleStatus(
        BusinessRule rule,
        BusinessRuleExpectedValidationStatus expectedStatus,
        BusinessRuleValidationStatus status,
        IReadOnlyList<BusinessRuleDetail> details
    )
    {
        BusinessRuleValidationResult result = new(rule, expectedStatus, status, details);
        _results.Add(result);
        return result;
    }

    public FacturXValidationResult Build()
    {
        if (!_expectedProfile.HasValue)
        {
            throw new InvalidOperationException("Expected profile must be set before building the result.");
        }

        return new FacturXValidationResult(_expectedProfile.Value, _results);
    }
}
