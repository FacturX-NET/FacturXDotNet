namespace FacturXDotNet.Validation.BusinessRules;

/// <summary>
///     Represents a HYBRID rule for validating a Factur-X document.
/// </summary>
/// <param name="Name">The name of the rule.</param>
/// <param name="Description">A description of the rule.</param>
/// <param name="Severity">The severity of the rule.</param>
public abstract record HybridBusinessRule(string Name, string Description, FacturXBusinessRuleSeverity Severity = FacturXBusinessRuleSeverity.Fatal) : BusinessRule(
    Name,
    Description
)
{
    /// <summary>
    ///     Determines whether the invoice satisfies the conditions defined by the rule.
    /// </summary>
    /// <param name="xmp">The XMP metadata to validate.</param>
    /// <param name="ciiAttachmentName"></param>
    /// <param name="cii">The Cross-Industry Invoice to validate.</param>
    /// <returns><c>true</c> if the rule is satisfied by the invoice; otherwise <c>false</c>.</returns>
    public abstract bool Check(XmpMetadata xmp, string ciiAttachmentName, CrossIndustryInvoice cii);

    /// <inheritdoc />
    public override string Format() => $"[HYBRID] {Name} - {Description}";
}
