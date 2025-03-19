# FacturX.NET - Models

The main goal of this library is to provide well-documented data structures to work with Factur-X data, based on the [Factur-X standard](https://fnfe-mpe.org/factur-x/) version 1.0.07.2.

If you spot a mistake or believe something could be improved, feel free to **open an issue** or even **contribute a fix**! Your feedback and contributions help keep the project accurate and reliable.

This library is used by:
- [FacturX.NET - Parser](../FacturXDotNet.Parser/README.md) to parse Factur-X files.
- [FacturX.NET - FacturX Exporter](../FacturXDotNet.Exporter.FacturX/README.md) to create Factur-X files.
- [FacturX.NET - CII Exporter](../FacturXDotNet.Exporter.CII/README.md) to create CII files.
- [FacturX.NET - UBL EN16931 Exporter](../FacturXDotNet.Exporter.UblEn16931/README.md) to create UBL EN16931 files.

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

### 4. Support for Multiple Profiles

To ensure that a single object can support all Factur-X profiles, the model follows the constraints of the least restrictive profile, which is **MINIMUM**. This means that properties that are optional in MINIMUM remain nullable in all profiles, even if they are required in higher-level profiles such as BASIC or EN16931.

When working with higher profiles, users can assume that properties required by those profiles are set, even though they are technically nullable.

## Specifications

The specification of the Cross-Industry Invoice (CII) syntax and the associated business rules can be found in the [Factur-X specification](https://fnfe-mpe.org/factur-x/).