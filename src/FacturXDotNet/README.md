# FacturX.NET

This library contains a set of tools to work with the Factur-X standard in .NET applications. It provides classes to represent Factur-X data structures, parse Factur-X PDF files, validate Factur-X data against the Factur-X schematron rules, and even generate Factur-X documents from structured data.

It is distributed as a [nuget package](https://www.nuget.org/packages/FacturXDotNet)
```
dotnet add package FacturXDotNet
```

## Getting Started

```csharp
await using FileStream myFileStream = File.OpenRead("path/to/my/file");

FacturXDocument document = await FacturXDocument.LoadAsync(myFileStream);

// Get the XMP metadata from the PDF document and parse it into a XmpMetadata object
XmpMetadata xmp = await document.GetXmpMetadataAsync();

// Get the attachment corresponding to the Cross-Industry Invoice (CII) data
var ciiAttachment = await document.GetCrossIndustryInvoiceAttachmentAsync();

// Get the content of the attachment and parse it into a CrossIndustryInvoice object
CrossIndustryInvoice cii = await ciiAttachment.GetCrossIndustryInvoiceAsync();

// Check that the document is valid according to the specifications
FacturXValidator validator = new();
FacturXValidationResult validationResult = await validator.GetValidationResultAsync(document);

Console.WriteLine($"Validation has succeeded: {validationResult.Success}");
Console.WriteLine($"Detected document profile: {validationResult.ValidProfiles.GetMaxProfile()}");
```

## Documentation

To check out docs, visit [facturxdotnet.org/docs](https://facturxdotnet.org/docs).

## Design Choices

This project consists of models that implement the Factur-X specification. The design choices ensure that the structure and documentation of the models remain as close as possible to the underlying Factur-X XML format.

### 1. Structural Consistency with Factur-X XML

The structure of the objects in this project closely matches the structure of the corresponding XML file. This ensures a direct and intuitive mapping between the object model and the XML representation.

For example:
- **XPath:** `/rsm:CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TaxTotalAmount`
- **Property:** `crossIndustryInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount`

### 2. Documentation Sourced from the Factur-X Specification

To ensure completeness and accuracy, the documentation for the classes and their properties is directly copied from the Factur-X specification. Each property is documented with the following details:
- **Summary:** The name of the concept in business terms (column I) and a description of the property's meaning and usage (column J).
- **Remarks:** Additional guidance on how the property should be used (column K).
- **ID:** The identifier of the property in the Factur-X specification (column E).
- **BR-\*:** Constraints and validation rules that must be fulfilled (column M).
- **CiiXPath:** The corresponding XPath in the Cross-Industry Invoice (CII) XML file within the Factur-X PDF document (column R).
- **Profile:** The Factur-X profile where the property is introduced (column AG).
- **ChorusPro:** Special considerations when using the property with the Chorus Pro platform (column L).

### 3. Intentional Redundancy for Documentation Clarity

The model intentionally introduces some redundancy to improve clarity and maintain accurate documentation. For example, instead of defining a single `TradeParty` object and using it for both the Buyer and Seller trade parties, the model defines separate classes:
- `SellerTradeParty`
- `BuyerTradeParty`

This separation ensures that each property is mapped to a unique business term from the Factur-X specification. If a single model were used for both cases, properties would need to match multiple business terms, making the documentation less precise and harder to interpret.

### 4. Nullable Properties for Flexibility

To ensure that the models can be used in a wide range of scenarios, all properties are nullable by default. This allows users to work with partial Factur-X data or to create new Factur-X documents without having to provide all properties.
It also allows the use of one model for all profiles, as properties that are not present in simpler profiles can be null.

### 5. Reimplementation of Schematron Rules in C#

For performance reasons, the validation does not use the schematron file provided by the Factur-X standard. Instead, it implements the same set of rules has been reimplemented in C#.