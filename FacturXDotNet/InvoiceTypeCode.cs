namespace FacturXDotNet;

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
public enum InvoiceTypeCode
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
    public static int ToSpecificationIdentifierString(this InvoiceTypeCode value) =>
        value switch
        {
            InvoiceTypeCode.CommercialInvoice => 380,
            InvoiceTypeCode.CreditNote => 381,
            InvoiceTypeCode.CorrectionInvoice => 384,
            InvoiceTypeCode.SelfBilledInvoice => 389,
            InvoiceTypeCode.SelfBilledCreditNote => 261,
            InvoiceTypeCode.PrepaymentInvoice => 386,
            InvoiceTypeCode.InvoiceInformationForAccountingPurpose => 751,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

    public static InvoiceTypeCode? ToSpecificationIdentifierOrNull(this int value) =>
        value switch
        {
            380 => InvoiceTypeCode.CommercialInvoice,
            381 => InvoiceTypeCode.CreditNote,
            384 => InvoiceTypeCode.CorrectionInvoice,
            389 => InvoiceTypeCode.SelfBilledInvoice,
            261 => InvoiceTypeCode.SelfBilledCreditNote,
            386 => InvoiceTypeCode.PrepaymentInvoice,
            751 => InvoiceTypeCode.InvoiceInformationForAccountingPurpose,
            _ => null
        };

    public static InvoiceTypeCode ToSpecificationIdentifier(this int value) =>
        value.ToSpecificationIdentifierOrNull() ?? throw new ArgumentOutOfRangeException(nameof(InvoiceTypeCode), value, null);
}
