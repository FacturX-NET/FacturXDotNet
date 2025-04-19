using CommunityToolkit.HighPerformance;
using FacturXDotNet;
using FacturXDotNet.Generation.FacturX;
using FacturXDotNet.Models.XMP;
using FluentAssertions;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace Tests.FacturXDotNet.Generation;

[TestClass]
public class FacturXDocumentBuilderPostProcessXmpMetadataTest
{
    [TestMethod]
    public async Task ShouldBuildDocument_WithOverridenData()
    {
        FacturXDocument newFacturXDocument = await FacturXDocument.Create()
            .WithBasePdfFile("TestFiles/facturx.pdf")
            .WithCrossIndustryInvoiceFile("TestFiles/cii.xml")
            .PostProcess(
                pp => pp.XmpMetadata(
                    xmp =>
                    {
                        xmp.PdfAIdentification.Amendment = "AMEND";
                        xmp.PdfAIdentification.Conformance = XmpPdfAConformanceLevel.B;
                        xmp.PdfAIdentification.Part = 123;

                        xmp.Basic.Identifier = ["IDENTIFIER1", "IDENTIFIER2"];
                        xmp.Basic.CreateDate = new DateTimeOffset(1, 2, 3, 0, 0, 0, TimeSpan.Zero);
                        xmp.Basic.CreatorTool = "CREATOR_TOOL";
                        xmp.Basic.Label = "LABEL";
                        xmp.Basic.MetadataDate = new DateTimeOffset(2, 3, 4, 0, 0, 0, TimeSpan.Zero);
                        xmp.Basic.ModifyDate = new DateTimeOffset(3, 4, 5, 0, 0, 0, TimeSpan.Zero);
                        xmp.Basic.Rating = 4.2;
                        xmp.Basic.BaseUrl = "BASE_URL";
                        xmp.Basic.Nickname = "NICKNAME";
                        xmp.Basic.Thumbnails = [new XmpThumbnail { Width = 123, Height = 456, Format = XmpThumbnailFormat.Jpeg, Image = "THUMBNAIL_IMAGE" }];

                        xmp.Pdf.Keywords = "PDF,KEYWORDS";
                        xmp.Pdf.PdfVersion = "PDF_VERSION";
                        xmp.Pdf.Producer = "PDF_PRODUCER";
                        xmp.Pdf.Trapped = true;

                        xmp.DublinCore.Contributor = ["DC_CONTRIBUTOR1", "DC_CONTRIBUTOR2"];
                        xmp.DublinCore.Coverage = "DC_COVERAGE";
                        xmp.DublinCore.Creator = ["DC_CREATOR1", "DC_CREATOR2"];
                        xmp.DublinCore.Date =
                        [
                            new DateTimeOffset(2, 3, 4, 0, 0, 0, TimeSpan.Zero),
                            new DateTimeOffset(3, 4, 5, 6, 7, 8, 9, 10, TimeSpan.Zero)
                        ];
                        xmp.DublinCore.Description = ["DC_DESCRIPTION1", "DC_DESCRIPTION2"];
                        xmp.DublinCore.Format = "DC_FORMAT";
                        xmp.DublinCore.Identifier = "DC_IDENTIFIER";
                        xmp.DublinCore.Language = ["DC_LANGUAGE1", "DC_LANGUAGE2"];
                        xmp.DublinCore.Publisher = ["DC_PUBLISHER1", "DC_PUBLISHER2"];
                        xmp.DublinCore.Relation = ["DC_RELATION1", "DC_RELATION2"];
                        xmp.DublinCore.Rights = ["DC_RIGHTS1", "DC_RIGHTS2"];
                        xmp.DublinCore.Source = "DC_SOURCE";
                        xmp.DublinCore.Subject = ["DC_SUBJECT1", "DC_SUBJECT2"];
                        xmp.DublinCore.Title = ["DC_TITLE1", "DC_TITLE2"];
                        xmp.DublinCore.Type = ["DC_TYPE1", "DC_TYPE2"];

                        xmp.PdfAExtensions.Schemas =
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
                            },
                            // This schema is necessary in order to be able to read the FacturX Metadata, it tells the reader which prefix to look for
                            new XmpPdfASchemaMetadata
                            {
                                NamespaceUri = "urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#",
                                Prefix = "fx"
                            }
                        ];

                        xmp.FacturX.DocumentFileName = "DOC_FILE_NAME";
                        xmp.FacturX.DocumentType = XmpFacturXDocumentType.Invoice;
                        xmp.FacturX.Version = "DOC_VERSION";
                        xmp.FacturX.ConformanceLevel = XmpFacturXConformanceLevel.En16931;
                    }
                )
            )
            .BuildAsync();

        XmpMetadata? newDocumentXmpMetadata = await newFacturXDocument.GetXmpMetadataAsync();

        newDocumentXmpMetadata.Should().NotBeNull();

        newDocumentXmpMetadata.PdfAIdentification.Should().NotBeNull();
        newDocumentXmpMetadata.PdfAIdentification.Amendment.Should().Be("AMEND");
        newDocumentXmpMetadata.PdfAIdentification.Conformance.Should().Be(XmpPdfAConformanceLevel.B);
        newDocumentXmpMetadata.PdfAIdentification.Part.Should().Be(123);

        newDocumentXmpMetadata.Basic.Should().NotBeNull();
        newDocumentXmpMetadata.Basic.Identifier.Should().BeEquivalentTo("IDENTIFIER1", "IDENTIFIER2");
        newDocumentXmpMetadata.Basic.CreateDate.Should().Be(new DateTimeOffset(1, 2, 3, 0, 0, 0, TimeSpan.Zero));
        newDocumentXmpMetadata.Basic.Label.Should().Be("LABEL");
        newDocumentXmpMetadata.Basic.MetadataDate.Should().Be(new DateTimeOffset(2, 3, 4, 0, 0, 0, TimeSpan.Zero));
        newDocumentXmpMetadata.Basic.ModifyDate.Should().Be(new DateTimeOffset(3, 4, 5, 0, 0, 0, TimeSpan.Zero));
        newDocumentXmpMetadata.Basic.Rating.Should().Be(4.2);
        newDocumentXmpMetadata.Basic.BaseUrl.Should().Be("BASE_URL");
        newDocumentXmpMetadata.Basic.Nickname.Should().Be("NICKNAME");
        newDocumentXmpMetadata.Basic.Thumbnails.Should()
            .BeEquivalentTo([new XmpThumbnail { Width = 123, Height = 456, Format = XmpThumbnailFormat.Jpeg, Image = "THUMBNAIL_IMAGE" }]);

        newDocumentXmpMetadata.Pdf.Should().NotBeNull();
        newDocumentXmpMetadata.Pdf.Keywords.Should().Be("PDF,KEYWORDS");
        newDocumentXmpMetadata.Pdf.PdfVersion.Should().Be("PDF_VERSION");
        newDocumentXmpMetadata.Pdf.Trapped.Should().BeTrue();

        newDocumentXmpMetadata.DublinCore.Should().NotBeNull();
        newDocumentXmpMetadata.DublinCore.Contributor.Should().BeEquivalentTo("DC_CONTRIBUTOR1", "DC_CONTRIBUTOR2");
        newDocumentXmpMetadata.DublinCore.Coverage.Should().Be("DC_COVERAGE");
        newDocumentXmpMetadata.DublinCore.Creator.Should().BeEquivalentTo("DC_CREATOR1", "DC_CREATOR2");
        newDocumentXmpMetadata.DublinCore.Date.Should()
            .BeEquivalentTo(
                [
                    new DateTimeOffset(2, 3, 4, 0, 0, 0, TimeSpan.Zero),
                    new DateTimeOffset(3, 4, 5, 6, 7, 8, 9, 10, TimeSpan.Zero)
                ]
            );
        newDocumentXmpMetadata.DublinCore.Description.Should().BeEquivalentTo("DC_DESCRIPTION1", "DC_DESCRIPTION2");
        newDocumentXmpMetadata.DublinCore.Format.Should().Be("DC_FORMAT");
        newDocumentXmpMetadata.DublinCore.Identifier.Should().Be("DC_IDENTIFIER");
        newDocumentXmpMetadata.DublinCore.Language.Should().BeEquivalentTo("DC_LANGUAGE1", "DC_LANGUAGE2");
        newDocumentXmpMetadata.DublinCore.Publisher.Should().BeEquivalentTo("DC_PUBLISHER1", "DC_PUBLISHER2");
        newDocumentXmpMetadata.DublinCore.Relation.Should().BeEquivalentTo("DC_RELATION1", "DC_RELATION2");
        newDocumentXmpMetadata.DublinCore.Rights.Should().BeEquivalentTo("DC_RIGHTS1", "DC_RIGHTS2");
        newDocumentXmpMetadata.DublinCore.Source.Should().Be("DC_SOURCE");
        newDocumentXmpMetadata.DublinCore.Subject.Should().BeEquivalentTo("DC_SUBJECT1", "DC_SUBJECT2");
        newDocumentXmpMetadata.DublinCore.Title.Should().BeEquivalentTo("DC_TITLE1", "DC_TITLE2");
        newDocumentXmpMetadata.DublinCore.Type.Should().BeEquivalentTo("DC_TYPE1", "DC_TYPE2");

        newDocumentXmpMetadata.PdfAExtensions.Should().NotBeNull();
        newDocumentXmpMetadata.PdfAExtensions.Schemas.Should()
            .BeEquivalentTo(
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
                    },
                    new XmpPdfASchemaMetadata
                    {
                        NamespaceUri = "urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#",
                        Prefix = "fx"
                    }
                ]
            );

        newDocumentXmpMetadata.FacturX.Should().NotBeNull();
        newDocumentXmpMetadata.FacturX.DocumentFileName.Should().Be("DOC_FILE_NAME");
        newDocumentXmpMetadata.FacturX.DocumentType.Should().Be(XmpFacturXDocumentType.Invoice);
        newDocumentXmpMetadata.FacturX.Version.Should().Be("DOC_VERSION");
        newDocumentXmpMetadata.FacturX.ConformanceLevel.Should().Be(XmpFacturXConformanceLevel.En16931);

        await using Stream newDocumentAsStream = newFacturXDocument.Data.AsStream();
        using PdfDocument newPdfDocument = PdfReader.Open(newDocumentAsStream, PdfDocumentOpenMode.Import);

        newPdfDocument.Info.Title.Should().Be("DC_TITLE1");
        newPdfDocument.Info.Subject.Should().Be("DC_DESCRIPTION1");
        newPdfDocument.Info.CreationDate.Should().Be(new DateTime(1, 2, 3));
        newPdfDocument.Info.ModificationDate.Should().Be(new DateTime(3, 4, 5));
        newPdfDocument.Info.Keywords.Should().Be("PDF,KEYWORDS");
        newPdfDocument.Info.Author.Should().Be("DC_CREATOR1, DC_CREATOR2");
    }

    [TestMethod]
    public async Task ShouldBuildDocument_WithoutOverridingData()
    {
        FacturXDocument newFacturXDocument = await FacturXDocument.Create()
            .WithBasePdfFile("TestFiles/facturx.pdf")
            .WithCrossIndustryInvoiceFile("TestFiles/cii.xml")
            .PostProcess(
                pp => pp.XmpMetadata(
                    xmp =>
                    {
                        xmp.Basic.CreatorTool = "CREATOR_TOOL";
                        xmp.Pdf.Producer = "PDF_PRODUCER";
                    }
                )
            )
            .BuildAsync();

        XmpMetadata? newDocumentXmpMetadata = await newFacturXDocument.GetXmpMetadataAsync();

        newDocumentXmpMetadata.Should().NotBeNull();
        newDocumentXmpMetadata.Basic.Should().NotBeNull();
        newDocumentXmpMetadata.Basic.CreatorTool.Should().StartWith("FacturX.NET");
        newDocumentXmpMetadata.Pdf.Should().NotBeNull();
        newDocumentXmpMetadata.Pdf.Producer.Should().StartWith("FacturX.NET");
    }
}
