using FacturXDotNet.Models;
using FacturXDotNet.Validation.BusinessRules;

namespace FacturXDotNet.Validation;

class FacturXValidationResultBuilder
{
    readonly List<BusinessRuleValidationResult> _results = [];
    FacturXProfile? _expectedProfile;

    public FacturXValidationResultBuilder SetExpectedProfile(FacturXProfile profile)
    {
        _expectedProfile = profile;
        return this;
    }

    public FacturXValidationResultBuilder AddRuleStatus(
        BusinessRule rule,
        BusinessRuleExpectedValidationStatus expectedStatus,
        BusinessRuleValidationStatus status,
        IReadOnlyList<BusinessRuleDetail> details
    )
    {
        _results.Add(new BusinessRuleValidationResult(rule, expectedStatus, status, details));
        return this;
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
