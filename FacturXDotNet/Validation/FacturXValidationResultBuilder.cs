using FacturXDotNet.Validation.BusinessRules;

namespace FacturXDotNet.Validation;

class FacturXValidationResultBuilder
{
    public FacturXValidationResultBuilder AddError(string error) =>
        // TODO: Implement this method
        this;

    public FacturXValidationResultBuilder AddRuleStatus(BusinessRule rule, BusinessRuleExpectedValidationStatus expectedStatus, BusinessRuleValidationStatus status) =>
        // TODO: Implement this method
        this;

    public FacturXValidationResult Build() =>
        // TODO: Implement this method
        new();
}
