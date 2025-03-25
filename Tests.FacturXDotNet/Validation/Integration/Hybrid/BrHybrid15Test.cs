namespace Tests.FacturXDotNet.Validation.Integration.Hybrid;

[TestClass]
public class BrHybrid15Test
{
    [TestMethod]
    public async Task ShouldFail() =>
        await ValidationIntegrationTestUtils.CheckRuleFails(
            "BR-HYBRID-15",
            """
            <?xpacket begin="﻿" id="W5M0MpCehiHzreSzNTczkc9d"?>
            <x:xmpmeta xmlns:x="adobe:ns:meta/">
              <rdf:RDF xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#">
                <rdf:Description xmlns:pdfaid="http://www.aiim.org/pdfa/ns/id/" rdf:about="">
                  <pdfaid:part>3</pdfaid:part>
                  <pdfaid:conformance>B</pdfaid:conformance>
                </rdf:Description>
                <rdf:Description xmlns:dc="http://purl.org/dc/elements/1.1/" rdf:about="">
                  <dc:title>
                    <rdf:Alt>
                      <rdf:li xml:lang="x-default">LE FOURNISSEUR: Invoice F20220023</rdf:li>
                    </rdf:Alt>
                  </dc:title>
                  <dc:creator>
                    <rdf:Seq>
                      <rdf:li>LE FOURNISSEUR</rdf:li>
                    </rdf:Seq>
                  </dc:creator>
                  <dc:description>
                    <rdf:Alt>
                      <rdf:li xml:lang="x-default">Invoice F20220023 dated 2022-01-31 issued by LE FOURNISSEUR</rdf:li>
                    </rdf:Alt>
                  </dc:description>
                </rdf:Description>
                <rdf:Description xmlns:pdf="http://ns.adobe.com/pdf/1.3/" rdf:about="">
                  <pdf:Producer>PyPDF4</pdf:Producer>
                </rdf:Description>
                <rdf:Description xmlns:xmp="http://ns.adobe.com/xap/1.0/" rdf:about="">
                  <xmp:CreatorTool>factur-x python lib v2.5 by Alexis de Lattre</xmp:CreatorTool>
                  <xmp:CreateDate>2024-09-01T19:18:42+00:00</xmp:CreateDate>
                  <xmp:ModifyDate>2024-09-01T19:18:42+00:00</xmp:ModifyDate>
                </rdf:Description>
                <rdf:Description xmlns:pdfaExtension="http://www.aiim.org/pdfa/ns/extension/" xmlns:pdfaSchema="http://www.aiim.org/pdfa/ns/schema#" xmlns:pdfaProperty="http://www.aiim.org/pdfa/ns/property#" rdf:about="">
                  <pdfaExtension:schemas>
                    <rdf:Bag>
                      <rdf:li rdf:parseType="Resource">
                        <pdfaSchema:schema>Factur-X PDFA Extension Schema</pdfaSchema:schema>
                        <pdfaSchema:namespaceURI>urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#</pdfaSchema:namespaceURI>
                        <pdfaSchema:prefix>fx</pdfaSchema:prefix>
                        <pdfaSchema:property>
                          <rdf:Seq>
                            <rdf:li rdf:parseType="Resource">
                              <pdfaProperty:name>DocumentFileName</pdfaProperty:name>
                              <pdfaProperty:valueType>Text</pdfaProperty:valueType>
                              <pdfaProperty:category>external</pdfaProperty:category>
                              <pdfaProperty:description>The name of the embedded XML document</pdfaProperty:description>
                            </rdf:li>
                            <rdf:li rdf:parseType="Resource">
                              <pdfaProperty:name>DocumentType</pdfaProperty:name>
                              <pdfaProperty:valueType>Text</pdfaProperty:valueType>
                              <pdfaProperty:category>external</pdfaProperty:category>
                              <pdfaProperty:description>The type of the hybrid document in capital letters, e.g. INVOICE or ORDER</pdfaProperty:description>
                            </rdf:li>
                            <rdf:li rdf:parseType="Resource">
                              <pdfaProperty:name>Version</pdfaProperty:name>
                              <pdfaProperty:valueType>Text</pdfaProperty:valueType>
                              <pdfaProperty:category>external</pdfaProperty:category>
                              <pdfaProperty:description>The actual version of the standard applying to the embedded XML document</pdfaProperty:description>
                            </rdf:li>
                            <rdf:li rdf:parseType="Resource">
                              <pdfaProperty:name>ConformanceLevel</pdfaProperty:name>
                              <pdfaProperty:valueType>Text</pdfaProperty:valueType>
                              <pdfaProperty:category>external</pdfaProperty:category>
                              <pdfaProperty:description>The conformance level of the embedded XML document</pdfaProperty:description>
                            </rdf:li>
                          </rdf:Seq>
                        </pdfaSchema:property>
                      </rdf:li>
                    </rdf:Bag>
                  </pdfaExtension:schemas>
                </rdf:Description>
                <rdf:Description xmlns:fx="urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#" rdf:about="">
                  <fx:DocumentType>INVOICE</fx:DocumentType>
                  <fx:DocumentFileName>factur-x.xml</fx:DocumentFileName>
                  <fx:Version>1.0</fx:Version>
                  <fx:ConformanceLevel>EN 16931</fx:ConformanceLevel>
                </rdf:Description>
              </rdf:RDF>
            </x:xmpmeta>
            <?xpacket end="w"?>
            """,
            """
            <?xml version='1.0' encoding='UTF-8'?>
            <rsm:CrossIndustryInvoice xmlns:qdt="urn:un:unece:uncefact:data:standard:QualifiedDataType:100"
            xmlns:ram="urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100"
            xmlns:rsm="urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100"
            xmlns:udt="urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100"
            xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                 <rsm:ExchangedDocumentContext>
                      <ram:BusinessProcessSpecifiedDocumentContextParameter>
                           <ram:ID>A1</ram:ID>
                      </ram:BusinessProcessSpecifiedDocumentContextParameter>
                      <ram:GuidelineSpecifiedDocumentContextParameter>
                           <ram:ID>urn:factur-x.eu:1p0:basic</ram:ID>
                      </ram:GuidelineSpecifiedDocumentContextParameter>
                 </rsm:ExchangedDocumentContext>
                 <rsm:ExchangedDocument>
                      <ram:ID>F20220023</ram:ID>
                      <ram:TypeCode>380</ram:TypeCode>
                      <ram:IssueDateTime>
                           <udt:DateTimeString format="102">20220131</udt:DateTimeString>
                      </ram:IssueDateTime>
                 </rsm:ExchangedDocument>
                 <rsm:SupplyChainTradeTransaction>
                      <ram:ApplicableHeaderTradeAgreement>
                           <ram:BuyerReference>SERVEXEC</ram:BuyerReference>
                           <ram:SellerTradeParty>
                                <ram:Name>LE FOURNISSEUR</ram:Name>
                                <ram:SpecifiedLegalOrganization>
                                     <ram:ID schemeID="0002">123456782</ram:ID>
                                </ram:SpecifiedLegalOrganization>
                                <ram:PostalTradeAddress>
                                     <ram:CountryID>FR</ram:CountryID>
                                </ram:PostalTradeAddress>
                                <ram:SpecifiedTaxRegistration>
                                     <ram:ID schemeID="VA">FR11123456782</ram:ID>
                                </ram:SpecifiedTaxRegistration>
                           </ram:SellerTradeParty>
                           <ram:BuyerTradeParty>
                                <ram:Name>LE CLIENT</ram:Name>
                                <ram:SpecifiedLegalOrganization>
                                     <ram:ID schemeID="0002">987654321</ram:ID>
                                </ram:SpecifiedLegalOrganization>
                           </ram:BuyerTradeParty>
                           <ram:BuyerOrderReferencedDocument>
                                <ram:IssuerAssignedID>PO201925478</ram:IssuerAssignedID>
                           </ram:BuyerOrderReferencedDocument>
                      </ram:ApplicableHeaderTradeAgreement>
                      <ram:ApplicableHeaderTradeDelivery/>
                      <ram:ApplicableHeaderTradeSettlement>
                           <ram:InvoiceCurrencyCode>EUR</ram:InvoiceCurrencyCode>
                           <ram:SpecifiedTradeSettlementHeaderMonetarySummation>
                                <ram:TaxBasisTotalAmount>100.00</ram:TaxBasisTotalAmount>
                                <ram:TaxTotalAmount currencyID="EUR">4.90</ram:TaxTotalAmount>
                                <ram:GrandTotalAmount>104.90</ram:GrandTotalAmount>
                                <ram:DuePayableAmount>104.90</ram:DuePayableAmount>
                           </ram:SpecifiedTradeSettlementHeaderMonetarySummation>
                      </ram:ApplicableHeaderTradeSettlement>
                 </rsm:SupplyChainTradeTransaction>
            </rsm:CrossIndustryInvoice>
            """
        );
}
