namespace FacturXDotNet.Models;

/// <summary>
///     <b>Invoice type code</b> - A code specifying the functional type of the Invoice.
/// </summary>
/// <remarks>
///     Commercial invoices and credit notes are defined according the entries in UNTDID 1001. Other  entries of UNTDID 1001 with specific invoices or credit notes may be used if
///     applicable.
/// </remarks>
/// <ID>BT-3</ID>
/// <BR-4>An Invoice shall have an Invoice type code.</BR-4>
/// <CiiXPath>/rsm:CrossIndustryInvoice/rsm:ExchangedDocument/ram:TypeCode</CiiXPath>
/// <Profile>MINIMUM</Profile>
public enum FacturXTypeCode
{
    CommercialInvoice = 380,
    CreditNote = 381,
    CorrectionInvoice = 384,
    SelfBilledInvoice = 389,

    /// <remarks>
    ///     Not accepted by Chorus Pro.
    /// </remarks>
    SelfBilledCreditNote = 261,
    PrepaymentInvoice = 386,

    /// <remarks>
    ///     Not accepted by Chorus Pro.
    /// </remarks>
    InvoiceInformationForAccountingPurpose = 751
}

public static class FacturXTypeCodeMappingExtensions
{
    public static int ToSpecificationIdentifierString(this FacturXTypeCode value) =>
        value switch
        {
            FacturXTypeCode.CommercialInvoice => 380,
            FacturXTypeCode.CreditNote => 381,
            FacturXTypeCode.CorrectionInvoice => 384,
            FacturXTypeCode.SelfBilledInvoice => 389,
            FacturXTypeCode.SelfBilledCreditNote => 261,
            FacturXTypeCode.PrepaymentInvoice => 386,
            FacturXTypeCode.InvoiceInformationForAccountingPurpose => 751,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

    public static FacturXTypeCode? ToSpecificationIdentifier(this int value) =>
        value switch
        {
            380 => FacturXTypeCode.CommercialInvoice,
            381 => FacturXTypeCode.CreditNote,
            384 => FacturXTypeCode.CorrectionInvoice,
            389 => FacturXTypeCode.SelfBilledInvoice,
            261 => FacturXTypeCode.SelfBilledCreditNote,
            386 => FacturXTypeCode.PrepaymentInvoice,
            751 => FacturXTypeCode.InvoiceInformationForAccountingPurpose,
            _ => null
        };
}
