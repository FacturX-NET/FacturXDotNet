namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

record BrHybrid13() : HybridBusinessRule("BR-HYBRID-13", "The embedded file name SHALL be one of the values defined in the HybridDocumentFilename code list.")
{
    public override bool Check(FacturXDocument invoice) =>
        // At this point this is necessarily true because we must have found it in order to create the FacturX instance. 
        true;
}
