using System.Text;
using System.Xml;
using FacturXDotNet.Generation.XMP;
using FacturXDotNet.Models.XMP;
using FluentAssertions;

namespace Tests.FacturXDotNet.Generation;

[TestClass]
public class XmpMetadataWriterTest
{
    [TestMethod]
    public async Task ShouldWriteXmpMetadataXml_Minimum()
    {
        XmpMetadata xmp = new()
        {
            PdfAIdentification = new XmpPdfAIdentificationMetadata
            {
                Amendment = "AMEND",
                Conformance = XmpPdfAConformanceLevel.B,
                Part = 123
            },
            Basic = new XmpBasicMetadata
            {
              Identifier = ["IDENTIFIER1", "IDENTIFIER2"],
              CreateDate = new DateTimeOffset(1, 2, 3, 0, 0, 0, TimeSpan.Zero),
                CreatorTool = "CREATOR_TOOL",
                Label = "LABEL",
                MetadataDate = new DateTimeOffset(2, 3, 4, 0, 0, 0, TimeSpan.Zero),
                ModifyDate = new DateTimeOffset(3, 4, 5, 0, 0, 0, TimeSpan.Zero),
                Rating = 4.2,
                BaseUrl = "BASE_URL",
                Nickname = "NICKNAME",
                Thumbnails = [new XmpThumbnail { Width = 123, Height = 456, Format = XmpThumbnailFormat.Jpeg, Image = "THUMBNAIL_IMAGE" }]
            },
            Pdf = new XmpPdfMetadata
            {
                Keywords = "PDF,KEYWORDS",
                PdfVersion = "PDF_VERSION",
                Producer = "PDF_PRODUCER",
                Trapped = true
            },
            DublinCore = new XmpDublinCoreMetadata
            {
                Contributor = ["DC_CONTRIBUTOR1", "DC_CONTRIBUTOR2"],
                Coverage = "DC_COVERAGE",
                Creator = ["DC_CREATOR1", "DC_CREATOR2"],
                Date =
                [
                  new DateTimeOffset(2, 3, 4, 0, 0, 0, TimeSpan.Zero),
                  new DateTimeOffset(3, 4, 5, 6, 7, 8, 9, 10, TimeSpan.Zero)
                ],
                Description = ["DC_DESCRIPTION1", "DC_DESCRIPTION2"],
                Format = "DC_FORMAT",
                Identifier = "DC_IDENTIFIER",
                Language = ["DC_LANGUAGE1", "DC_LANGUAGE2"],
                Publisher = ["DC_PUBLISHER1", "DC_PUBLISHER2"],
                Relation = ["DC_RELATION1", "DC_RELATION2"],
                Rights = ["DC_RIGHTS1", "DC_RIGHTS2"],
                Source = "DC_SOURCE",
                Subject = ["DC_SUBJECT1", "DC_SUBJECT2"],
                Title = ["DC_TITLE1", "DC_TITLE2"],
                Type = ["DC_TYPE1", "DC_TYPE2"]
            },
            PdfAExtensions = new XmpPdfAExtensionsMetadata
            {
                Schemas =
                [
                    new XmpPdfASchemaMetadata
                    {
                        NamespaceUri = "SCHEMA1_NAMESPACE",
                        Prefix = "SCHEMA1_PREFIX",
                        Property =
                        [
                            new XmpPdfAPropertyMetadata
                            {
                                Category = XmpPdfAPropertyCategory.External,
                                Description = "SCHEMA1_PROPERTY1_DESCRIPTION",
                                Name = "SCHEMA1_PROPERTY1_NAME",
                                ValueType = "SCHEMA1_PROPERTY1_VALUE_TYPE"
                            },
                            new XmpPdfAPropertyMetadata
                            {
                                Category = XmpPdfAPropertyCategory.Internal,
                                Description = "SCHEMA1_PROPERTY2_DESCRIPTION",
                                Name = "SCHEMA1_PROPERTY2_NAME",
                                ValueType = "SCHEMA1_PROPERTY2_VALUE_TYPE"
                            }
                        ],
                        Schema = "SCHEMA1_SCHEMA",
                        ValueType =
                        [
                            new XmpPdfATypeMetadata
                            {
                                Description = "SCHEMA1_VALUE_TYPE1_DESCRIPTION",
                                Field =
                                [
                                    new XmpPdfAFieldMetadata
                                    {
                                        Description = "SCHEMA1_VALUE_TYPE1_FIELD1_DESCRIPTION",
                                        Name = "SCHEMA1_VALUE_TYPE1_FIELD1_NAME",
                                        ValueType = "SCHEMA1_VALUE_TYPE1_FIELD1_VALUE_TYPE"
                                    },
                                    new XmpPdfAFieldMetadata
                                    {
                                        Description = "SCHEMA1_VALUE_TYPE1_FIELD2_DESCRIPTION",
                                        Name = "SCHEMA1_VALUE_TYPE1_FIELD2_NAME",
                                        ValueType = "SCHEMA1_VALUE_TYPE1_FIELD2_VALUE_TYPE"
                                    }
                                ],
                                NamespaceUri = "SCHEMA1_VALUE_TYPE1_NAMESPACE",
                                Prefix = "SCHEMA1_VALUE_TYPE1_PREFIX",
                                Type = "SCHEMA1_VALUE_TYPE1_TYPE"
                            },
                            new XmpPdfATypeMetadata
                            {
                                Description = "SCHEMA1_VALUE_TYPE2_DESCRIPTION",
                                Field =
                                [
                                    new XmpPdfAFieldMetadata
                                    {
                                        Description = "SCHEMA1_VALUE_TYPE2_FIELD1_DESCRIPTION",
                                        Name = "SCHEMA1_VALUE_TYPE2_FIELD1_NAME",
                                        ValueType = "SCHEMA1_VALUE_TYPE2_FIELD1_VALUE_TYPE"
                                    },
                                    new XmpPdfAFieldMetadata
                                    {
                                        Description = "SCHEMA1_VALUE_TYPE2_FIELD2_DESCRIPTION",
                                        Name = "SCHEMA1_VALUE_TYPE2_FIELD2_NAME",
                                        ValueType = "SCHEMA1_VALUE_TYPE2_FIELD2_VALUE_TYPE"
                                    }
                                ],
                                NamespaceUri = "SCHEMA1_VALUE_TYPE2_NAMESPACE",
                                Prefix = "SCHEMA1_VALUE_TYPE2_PREFIX",
                                Type = "SCHEMA1_VALUE_TYPE2_TYPE"
                            }
                        ]
                    }
                ]
            },
            FacturX = new XmpFacturXMetadata
            {
                DocumentFileName = "DOC_FILE_NAME",
                DocumentType = XmpFacturXDocumentType.Invoice,
                Version = "DOC_VERSION",
                ConformanceLevel = XmpFacturXConformanceLevel.En16931
            }
        };

        XmpMetadataWriter writer = new();

        await using MemoryStream resultStream = new();
        await writer.WriteAsync(resultStream, xmp);
        resultStream.Seek(0, SeekOrigin.Begin);

        const string expectedFile = """
                                    <?xpacket begin="﻿" id="W5M0MpCehiHzreSzNTczkc9d"?>
                                    <x:xmpmeta xmlns:x="adobe:ns:meta/">
                                      <rdf:RDF xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#">
                                        <rdf:Description xmlns:pdfaid="http://www.aiim.org/pdfa/ns/id/" rdf:about="">
                                          <pdfaid:amd>AMEND</pdfaid:amd>
                                          <pdfaid:conformance>B</pdfaid:conformance>
                                          <pdfaid:part>123</pdfaid:part>
                                        </rdf:Description>
                                        <rdf:Description xmlns:dc="http://purl.org/dc/elements/1.1/" rdf:about="">
                                          <dc:contributor>
                                            <rdf:Bag>
                                              <rdf:li>DC_CONTRIBUTOR1</rdf:li>
                                              <rdf:li>DC_CONTRIBUTOR2</rdf:li>
                                            </rdf:Bag>
                                          </dc:contributor>
                                          <dc:coverage>DC_COVERAGE</dc:coverage>
                                          <dc:creator>
                                            <rdf:Seq>
                                              <rdf:li>DC_CREATOR1</rdf:li>
                                              <rdf:li>DC_CREATOR2</rdf:li>
                                            </rdf:Seq>
                                          </dc:creator>
                                          <dc:date>
                                            <rdf:Seq>
                                              <rdf:li>0002-03-04</rdf:li>
                                              <rdf:li>0003-04-05T06:07:08.009010Z</rdf:li>
                                            </rdf:Seq>
                                          </dc:date>
                                          <dc:description>
                                            <rdf:Alt>
                                              <rdf:li xml:lang="x-default">DC_DESCRIPTION1</rdf:li>
                                              <rdf:li>DC_DESCRIPTION2</rdf:li>
                                            </rdf:Alt>
                                          </dc:description>
                                          <dc:format>DC_FORMAT</dc:format>
                                          <dc:identifier>DC_IDENTIFIER</dc:identifier>
                                          <dc:language>
                                            <rdf:Bag>
                                              <rdf:li>DC_LANGUAGE1</rdf:li>
                                              <rdf:li>DC_LANGUAGE2</rdf:li>
                                            </rdf:Bag>
                                          </dc:language>
                                          <dc:publisher>
                                            <rdf:Bag>
                                              <rdf:li>DC_PUBLISHER1</rdf:li>
                                              <rdf:li>DC_PUBLISHER2</rdf:li>
                                            </rdf:Bag>
                                          </dc:publisher>
                                          <dc:relation>
                                            <rdf:Bag>
                                              <rdf:li>DC_RELATION1</rdf:li>
                                              <rdf:li>DC_RELATION2</rdf:li>
                                            </rdf:Bag>
                                          </dc:relation>
                                          <dc:rights>
                                            <rdf:Alt>
                                              <rdf:li xml:lang="x-default">DC_RIGHTS1</rdf:li>
                                              <rdf:li>DC_RIGHTS2</rdf:li>
                                            </rdf:Alt>
                                          </dc:rights>
                                          <dc:source>DC_SOURCE</dc:source>
                                          <dc:subject>
                                            <rdf:Bag>
                                              <rdf:li>DC_SUBJECT1</rdf:li>
                                              <rdf:li>DC_SUBJECT2</rdf:li>
                                            </rdf:Bag>
                                          </dc:subject>
                                          <dc:title>
                                            <rdf:Alt>
                                              <rdf:li xml:lang="x-default">DC_TITLE1</rdf:li>
                                              <rdf:li>DC_TITLE2</rdf:li>
                                            </rdf:Alt>
                                          </dc:title>
                                          <dc:type>
                                            <rdf:Bag>
                                              <rdf:li>DC_TYPE1</rdf:li>
                                              <rdf:li>DC_TYPE2</rdf:li>
                                            </rdf:Bag>
                                          </dc:type>
                                        </rdf:Description>
                                        <rdf:Description xmlns:pdf="http://ns.adobe.com/pdf/1.3/" rdf:about="">
                                          <pdf:Keywords>PDF,KEYWORDS</pdf:Keywords>
                                          <pdf:PDFVersion>PDF_VERSION</pdf:PDFVersion>
                                          <pdf:Producer>PDF_PRODUCER</pdf:Producer>
                                          <pdf:Trapped>True</pdf:Trapped>
                                        </rdf:Description>
                                        <rdf:Description xmlns:xmp="http://ns.adobe.com/xap/1.0/" xmlns:xmpGlmg="http://ns.adobe.com/xap/1.0/g/img/" rdf:about="">
                                          <xmp:CreateDate>0001-02-03</xmp:CreateDate>
                                          <xmp:CreatorTool>CREATOR_TOOL</xmp:CreatorTool>
                                          <xmp:Identifier>
                                            <rdf:Bag>
                                              <rdf:li>IDENTIFIER1</rdf:li>
                                              <rdf:li>IDENTIFIER2</rdf:li>
                                            </rdf:Bag>
                                          </xmp:Identifier>
                                          <xmp:Label>LABEL</xmp:Label>
                                          <xmp:MetadataDate>0002-03-04</xmp:MetadataDate>
                                          <xmp:ModifyDate>0003-04-05</xmp:ModifyDate>
                                          <xmp:Rating>4.2</xmp:Rating>
                                          <xmp:BaseURL>BASE_URL</xmp:BaseURL>
                                          <xmp:Nickname>NICKNAME</xmp:Nickname>
                                          <xmp:Thumbnails>
                                            <rdf:Alt>
                                              <rdf:li xml:lang="x-default">
                                                <xmpGlmg:format>JPEG</xmpGlmg:format>
                                                <xmpGlmg:height>456</xmpGlmg:height>
                                                <xmpGlmg:width>123</xmpGlmg:width>
                                                <xmpGlmg:image>THUMBNAIL_IMAGE</xmpGlmg:image>
                                              </rdf:li>
                                            </rdf:Alt>
                                          </xmp:Thumbnails>
                                        </rdf:Description>
                                        <rdf:Description xmlns:pdfaExtension="http://www.aiim.org/pdfa/ns/extension/" 
                                          xmlns:pdfaSchema="http://www.aiim.org/pdfa/ns/schema#" 
                                          xmlns:pdfaProperty="http://www.aiim.org/pdfa/ns/property#" 
                                          xmlns:pdfaType="http://www.aiim.org/pdfa/ns/type#" 
                                          xmlns:pdfaField="http://www.aiim.org/pdfa/ns/field#" rdf:about="">
                                          <pdfaExtension:schemas>
                                            <rdf:Bag>
                                              <rdf:li rdf:parseType="Resource">
                                                <pdfaSchema:namespaceURI>SCHEMA1_NAMESPACE</pdfaSchema:namespaceURI>
                                                <pdfaSchema:prefix>SCHEMA1_PREFIX</pdfaSchema:prefix>
                                                <pdfaSchema:property>
                                                  <rdf:Seq>
                                                    <rdf:li rdf:parseType="Resource">
                                                      <pdfaProperty:category>external</pdfaProperty:category>
                                                      <pdfaProperty:description>SCHEMA1_PROPERTY1_DESCRIPTION</pdfaProperty:description>
                                                      <pdfaProperty:name>SCHEMA1_PROPERTY1_NAME</pdfaProperty:name>
                                                      <pdfaProperty:valueType>SCHEMA1_PROPERTY1_VALUE_TYPE</pdfaProperty:valueType>
                                                    </rdf:li>
                                                    <rdf:li rdf:parseType="Resource">
                                                      <pdfaProperty:category>internal</pdfaProperty:category>
                                                      <pdfaProperty:description>SCHEMA1_PROPERTY2_DESCRIPTION</pdfaProperty:description>
                                                      <pdfaProperty:name>SCHEMA1_PROPERTY2_NAME</pdfaProperty:name>
                                                      <pdfaProperty:valueType>SCHEMA1_PROPERTY2_VALUE_TYPE</pdfaProperty:valueType>
                                                    </rdf:li>
                                                  </rdf:Seq>
                                                </pdfaSchema:property>
                                                <pdfaSchema:schema>SCHEMA1_SCHEMA</pdfaSchema:schema>
                                                <pdfaSchema:valueType>
                                                  <rdf:Seq>
                                                    <rdf:li rdf:parseType="Resource">
                                                      <pdfaType:description>SCHEMA1_VALUE_TYPE1_DESCRIPTION</pdfaType:description>
                                                      <pdfaType:field>
                                                        <rdf:Seq>
                                                          <rdf:li rdf:parseType="Resource">
                                                            <pdfaField:description>SCHEMA1_VALUE_TYPE1_FIELD1_DESCRIPTION</pdfaField:description>
                                                            <pdfaField:name>SCHEMA1_VALUE_TYPE1_FIELD1_NAME</pdfaField:name>
                                                            <pdfaField:valueType>SCHEMA1_VALUE_TYPE1_FIELD1_VALUE_TYPE</pdfaField:valueType>
                                                          </rdf:li>
                                                          <rdf:li rdf:parseType="Resource">
                                                            <pdfaField:description>SCHEMA1_VALUE_TYPE1_FIELD2_DESCRIPTION</pdfaField:description>
                                                            <pdfaField:name>SCHEMA1_VALUE_TYPE1_FIELD2_NAME</pdfaField:name>
                                                            <pdfaField:valueType>SCHEMA1_VALUE_TYPE1_FIELD2_VALUE_TYPE</pdfaField:valueType>
                                                          </rdf:li>
                                                        </rdf:Seq>
                                                      </pdfaType:field>
                                                      <pdfaType:namespaceURI>SCHEMA1_VALUE_TYPE1_NAMESPACE</pdfaType:namespaceURI>
                                                      <pdfaType:prefix>SCHEMA1_VALUE_TYPE1_PREFIX</pdfaType:prefix>
                                                      <pdfaType:type>SCHEMA1_VALUE_TYPE1_TYPE</pdfaType:type>
                                                    </rdf:li>
                                                    <rdf:li rdf:parseType="Resource">
                                                      <pdfaType:description>SCHEMA1_VALUE_TYPE2_DESCRIPTION</pdfaType:description>
                                                      <pdfaType:field>
                                                        <rdf:Seq>
                                                          <rdf:li rdf:parseType="Resource">
                                                            <pdfaField:description>SCHEMA1_VALUE_TYPE2_FIELD1_DESCRIPTION</pdfaField:description>
                                                            <pdfaField:name>SCHEMA1_VALUE_TYPE2_FIELD1_NAME</pdfaField:name>
                                                            <pdfaField:valueType>SCHEMA1_VALUE_TYPE2_FIELD1_VALUE_TYPE</pdfaField:valueType>
                                                          </rdf:li>
                                                          <rdf:li rdf:parseType="Resource">
                                                            <pdfaField:description>SCHEMA1_VALUE_TYPE2_FIELD2_DESCRIPTION</pdfaField:description>
                                                            <pdfaField:name>SCHEMA1_VALUE_TYPE2_FIELD2_NAME</pdfaField:name>
                                                            <pdfaField:valueType>SCHEMA1_VALUE_TYPE2_FIELD2_VALUE_TYPE</pdfaField:valueType>
                                                          </rdf:li>
                                                        </rdf:Seq>
                                                      </pdfaType:field>
                                                      <pdfaType:namespaceURI>SCHEMA1_VALUE_TYPE2_NAMESPACE</pdfaType:namespaceURI>
                                                      <pdfaType:prefix>SCHEMA1_VALUE_TYPE2_PREFIX</pdfaType:prefix>
                                                      <pdfaType:type>SCHEMA1_VALUE_TYPE2_TYPE</pdfaType:type>
                                                    </rdf:li>
                                                  </rdf:Seq>
                                                </pdfaSchema:valueType>
                                              </rdf:li>
                                            </rdf:Bag>
                                          </pdfaExtension:schemas>
                                        </rdf:Description>
                                        <rdf:Description xmlns:fx="urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#" rdf:about="">
                                          <fx:DocumentFileName>DOC_FILE_NAME</fx:DocumentFileName>
                                          <fx:DocumentType>INVOICE</fx:DocumentType>
                                          <fx:Version>DOC_VERSION</fx:Version>
                                          <fx:ConformanceLevel>EN 16931</fx:ConformanceLevel>
                                        </rdf:Description>
                                      </rdf:RDF>
                                    </x:xmpmeta>
                                    <?xpacket end="w"?>
                                    """;
        await using MemoryStream expectedFileStream = new(Encoding.UTF8.GetBytes(expectedFile));

        CompareXmlFiles(resultStream, expectedFileStream);
    }

    static void CompareXmlFiles(Stream fileStream, Stream expectedFileStream)
    {
        XmlDocument fileDocument = new();
        fileDocument.Load(fileStream);

        XmlDocument expectedFileDocument = new();
        expectedFileDocument.Load(expectedFileStream);

        fileDocument.Should().BeEquivalentTo(expectedFileDocument);
    }
}
