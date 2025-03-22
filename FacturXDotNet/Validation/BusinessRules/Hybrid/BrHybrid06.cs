﻿namespace FacturXDotNet.Validation.BusinessRules.Hybrid;

record BrHybrid06() : HybridBusinessRule("BR-HYBRID-06", "The fx:DocumentType in the XMP instance SHALL be a value from the HybridDocumentType code list.")
{
    public override bool Check(XmpMetadata? xmp, string? ciiAttachmentName, CrossIndustryInvoice? cii) =>
        xmp?.FacturX?.DocumentType is not null && Enum.IsDefined(xmp.FacturX.DocumentType.Value);
}
