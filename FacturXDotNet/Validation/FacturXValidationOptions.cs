using FacturXDotNet.Models;
using FacturXDotNet.Validation.BusinessRules.CII;

namespace FacturXDotNet.Validation;

/// <summary>
///     Provides configuration options for the <see cref="FacturXValidator" />.
/// </summary>
public class FacturXValidationOptions
{
    /// <summary>
    ///     The profile to use for validation.
    /// </summary>
    /// <remarks>
    ///     If set, this profile will be used for validation instead of the profile specified in the invoice.
    ///     This allows enforcing validation against a specific Factur-X profile regardless of the invoice's declared profile.
    ///     If <c>null</c>, the validator uses the profile defined in the invoice itself.
    /// </remarks>
    public FacturXProfile? ProfileOverride { get; set; } = null;

    /// <summary>
    ///     Whether to treat warnings as errors during validation.
    /// </summary>
    public bool TreatWarningsAsErrors { get; set; }

    /// <summary>
    ///     The list of rules to skip during validation.
    /// </summary>
    public List<string> RulesToSkip { get; } = [];

    /// <summary>
    ///     Skips all Cross-Industry Invoice business rules during validation.
    /// </summary>
    public void SkipCiiRules() => RulesToSkip.AddRange(CrossIndustryInvoiceBusinessRules.Rules.Select(r => r.Name));

    /// <summary>
    ///     Skips all Hybrid business rules during validation.
    /// </summary>
    public void SkipHybridRules() => RulesToSkip.AddRange(CrossIndustryInvoiceBusinessRules.Rules.Select(r => r.Name));
}
