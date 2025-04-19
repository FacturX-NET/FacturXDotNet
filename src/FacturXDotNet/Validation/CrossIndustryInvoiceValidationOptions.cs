using FacturXDotNet.Models;
using Microsoft.Extensions.Logging;

namespace FacturXDotNet.Validation;

/// <summary>
///     Provides configuration options for the <see cref="CrossIndustryInvoiceValidator" />.
/// </summary>
public class CrossIndustryInvoiceValidationOptions
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
    ///     The logger that should be used by the validator.
    /// </summary>
    public ILogger? Logger { get; set; }

    /// <summary>
    ///     The callback that should be called when a business rule validation result is available.
    ///     These results will also be available in the final validation report, this callback can be used to display the results asap, e.g. in a UI.
    ///     Note that this callback will be called for each rule, performance may be affected if the callback is slow.
    /// </summary>
    public Action<BusinessRuleValidationResult>? CheckCallback { get; set; }
}
