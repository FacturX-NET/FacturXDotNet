namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     The business rules for validating a FacturX.
/// </summary>
static class HybridBusinessRules
{
    public static readonly HybridBusinessRule[] Rules =
    [
        new BrHybrid01(),
        new BrHybrid02(),
        new BrHybrid03(),
        new BrHybrid04(),
        new BrHybrid05(),
        new BrHybrid06(),
        new BrHybrid07(),
        new BrHybrid08(),
        new BrHybrid09(),
        new BrHybrid10(),
        new BrHybrid11(),
        new BrHybrid12(),
        new BrHybrid13(),
        new BrHybrid14(),
        new BrHybrid15(),
        new BrHybridFr01()
    ];
}
