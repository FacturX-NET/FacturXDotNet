using FacturXDotNet.Models;
using FacturXDotNet.Validation.CII.BusinessRules;
using FacturXDotNet.Validation.CII.BusinessRules.VatRelated.StandardAndReducedRate;

namespace FacturXDotNet.Validation.CII.Schematron;

/// <summary>
///     Validates a Factur-X Cross-Industry Invoice against business rules defined in the Factur-X specification.
/// </summary>
/// <remarks>
///     Even though the class is called <b>Schematron</b> validator, it does not perform Schematron validation for performance reasons. All the rules of the schematron
///     have been reimplemented in plain C# code.
/// </remarks>
public class CrossIndustryInvoiceSchematronValidator(CrossIndustryInvoiceSchematronValidationOptions? options = null)
{
    readonly CrossIndustryInvoiceSchematronValidationOptions _options = options ?? new CrossIndustryInvoiceSchematronValidationOptions();

    /// <summary>
    ///     Determines whether the given invoice satisfies all applicable business rules.
    /// </summary>
    /// <remarks>
    ///     This method applies business rules and returns <c>true</c> if all are satisfied; otherwise, <c>false</c>.
    ///     Validation stops at the first failing rule for efficiency.
    ///     <para>
    ///         Use <see cref="GetValidationResult" /> if you need a detailed report of all rule evaluations.
    ///     </para>
    /// </remarks>
    /// <param name="invoice">The invoice to validate.</param>
    /// <returns><c>true</c> if the invoice meets all required business rules; otherwise, <c>false</c>.</returns>
    public bool IsValid(CrossIndustryInvoice invoice)
    {
        FacturXProfile profile = _options.ProfileOverride ?? invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId.ToFacturXProfile();
        return Rules.Where(rule => !ShouldSkipRule(rule) && !IsExpectedToFail(rule, profile)).All(rule => rule.Check(invoice));
    }

    /// <summary>
    ///     Computes a detailed validation result for the given invoice.
    /// </summary>
    /// <remarks>
    ///     Unlike <see cref="IsValid" />, this method evaluates all business rules and categorizes them as:
    ///     <list type="bullet">
    ///         <item>
    ///             <description><b>Passed</b> - Rules that were successfully met.</description>
    ///         </item>
    ///         <item>
    ///             <description><b>Failed</b> - Rules that were not satisfied.</description>
    ///         </item>
    ///         <item>
    ///             <description><b>Skipped</b> - Rules that were not applicable for the selected Factur-X profile.</description>
    ///         </item>
    ///     </list>
    ///     This method provides full validation at the cost of additional computation time and memory usage.
    /// </remarks>
    /// <param name="invoice">The invoice to validate.</param>
    /// <returns>
    ///     A <see cref="FacturXValidationResult" /> containing details of passed, failed, and skipped business rules.
    /// </returns>
    public FacturXValidationResult GetValidationResult(CrossIndustryInvoice invoice)
    {
        FacturXProfile profile = _options.ProfileOverride ?? invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameterId.ToFacturXProfile();

        List<FacturXBusinessRule> passed = [];
        List<FacturXBusinessRule> failed = [];
        List<FacturXBusinessRule> expectedToFail = [];
        List<FacturXBusinessRule> skipped = [];

        foreach (FacturXBusinessRule rule in Rules)
        {
            if (ShouldSkipRule(rule))
            {
                skipped.Add(rule);
                continue;
            }

            if (rule.Check(invoice))
            {
                passed.Add(rule);
            }
            else
            {
                if (IsExpectedToFail(rule, profile))
                {
                    expectedToFail.Add(rule);
                }
                else
                {
                    failed.Add(rule);
                }
            }
        }

        return new FacturXValidationResult(passed, failed, expectedToFail, skipped);
    }

    bool ShouldSkipRule(FacturXBusinessRule rule) => _options.RulesToSkip.Any(r => string.Equals(rule.Name, r, StringComparison.InvariantCultureIgnoreCase));
    static bool IsExpectedToFail(FacturXBusinessRule rule, FacturXProfile profile) => !rule.Profiles.Match(profile);

    static readonly FacturXBusinessRule[] Rules =
    [
        new Br01InvoiceShallHaveSpecificationIdentifier(),
        new Br02InvoiceShallHaveInvoiceNumber(),
        new Br03InvoiceShallHaveIssueDate(),
        new Br04InvoiceShallHaveTypeCode(),
        new Br05InvoiceShallHaveCurrencyCode(),
        new Br06InvoiceShallHaveSellerName(),
        new Br07InvoiceShallHaveBuyerName(),
        new Br08InvoiceShallHaveSellerPostalAddress(),
        new Br09InvoiceShallHaveSellerPostalAddressWithCountryCode(),
        new Br13InvoiceShallHaveTotalAmountWithoutVat(),
        new Br14InvoiceShallHaveTotalAmountWithVat(),
        new Br15InvoiceShallHaveAmountDueForPayment(),
        new BrCo09(),
        new BrCo26(),
        new BrDec12InvoiceTotalAmountWithoutVatHasTwoDecimals(),
        new BrDec13InvoiceTotalVatAmountHasTwoDecimals(),
        new BrDec14InvoiceTotalAmountHasTwoDecimals(),
        new BrDec18InvoiceDueAmountHasTwoDecimals(),
        new BrS01()
    ];
}
