using FacturXDotNet.Models;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

/// <summary>
///     Represents a HYBRID rule for validating a Factur-X document.
/// </summary>
/// <param name="Name">The name of the rule.</param>
/// <param name="Description">A description of the rule.</param>
/// <param name="Severity">The severity of the rule.</param>
public abstract record HybridBusinessRule(string Name, string Description, BusinessRuleSeverity Severity = BusinessRuleSeverity.Fatal) : BusinessRule(
    Name,
    Description,
    FacturXProfileFlags.All,
    Severity
)
{
    /// <summary>
    ///     Determines whether the invoice satisfies the conditions defined by the rule.
    /// </summary>
    /// <param name="xmp">The XMP metadata to validate.</param>
    /// <param name="ciiAttachmentName"></param>
    /// <param name="cii">The Cross-Industry Invoice to validate.</param>
    /// <param name="logger"></param>
    /// <returns><c>true</c> if the rule is satisfied by the invoice; otherwise <c>false</c>.</returns>
    public abstract bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii, IBusinessRuleDetailsLogger? logger = null);

    /// <inheritdoc />
    public override string Format() => $"{Name} - {Description}";
}
