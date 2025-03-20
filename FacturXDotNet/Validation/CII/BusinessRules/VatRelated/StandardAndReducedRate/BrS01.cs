using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.CII.BusinessRules.VatRelated.StandardAndReducedRate;

record BrS01() : FacturXBusinessRule(
    "BR-S-01",
    """
    An Invoice that contains an Invoice line (BG-25), a Document level allowance (BG-20) or a Document level charge (BG-21) where the VAT category code (BT-151, BT-95 or BT-102) 
    is “Standard rated” shall contain in the VAT breakdown (BG-23) at least one VAT category code (BT-118) equal with "Standard rated".
    """,
    FacturXProfile.BasicWl.AndHigher()
)
{
    public override bool Check(CrossIndustryInvoice invoice) =>
        // TODO
        true;
}
