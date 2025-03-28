using FacturXDotNet;
using FacturXDotNet.Generation.FacturX;
using FacturXDotNet.Generation.PDF;
using FluentAssertions;
using PdfSharp.Pdf;

namespace Tests.FacturXDotNet.Generation;

[TestClass]
public class FacturXDocumentBuilderTest
{
    [TestMethod]
    public async Task ShouldBuildDocument_WithProvidedCii()
    {
        byte[] ciiContent = """
                            <?xml version='1.0' encoding='UTF-8'?>
                            <rsm:CrossIndustryInvoice xmlns:qdt="urn:un:unece:uncefact:data:standard:QualifiedDataType:100"
                            xmlns:ram="urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100"
                            xmlns:rsm="urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100"
                            xmlns:udt="urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100"
                            xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                            </rsm:CrossIndustryInvoice>
                            """u8.ToArray();
        await using MemoryStream ciiContentStream = new(ciiContent);

        PdfDocument blankPdf = new();
        blankPdf.AddPage();

        await using MemoryStream blankPdfStream = new();
        await blankPdf.SaveAsync(blankPdfStream);

        FacturXDocument newFacturXDocument = await FacturXDocument.Create()
            .WithBasePdf(blankPdfStream)
            .WithCrossIndustryInvoice(ciiContentStream)
            .WithXmpMetadataFile("TestFiles/xmp.xml")
            .BuildAsync();

        CrossIndustryInvoiceAttachment? ciiAttachment = await newFacturXDocument.GetCrossIndustryInvoiceAttachmentAsync();

        ciiAttachment.Should().NotBeNull();

        ReadOnlyMemory<byte> newDocumentCiiContent = await ciiAttachment.ReadAsync();

        newDocumentCiiContent.ToArray().Should().BeEquivalentTo(ciiContent.ToArray());
    }

    [TestMethod]
    public async Task ShouldBuildDocument_WithXmp_AsIs()
    {
        byte[] xmpContent = """
                            ﻿<?xpacket begin='﻿' id='W5M0MpCehiHzreSzNTczkc9d'?>
                            <x:xmpmeta xmlns:x="adobe:ns:meta/">
                              <rdf:RDF xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#">
                                  <rdf:Description xmlns:xmp="http://ns.adobe.com/xap/1.0/" rdf:about="">
                                    <xmp:CreateDate>0001-02-03T04:05:06Z</xmp:CreateDate>
                                  </rdf:Description>
                              </rdf:RDF>
                            </x:xmpmeta>
                            <?xpacket end='w'?>
                            """u8.ToArray();
        await using MemoryStream xmpContentStream = new(xmpContent);

        PdfDocument blankPdf = new();
        blankPdf.AddPage();

        await using MemoryStream blankPdfStream = new();
        await blankPdf.SaveAsync(blankPdfStream);

        FacturXDocument newFacturXDocument = await FacturXDocument.Create()
            .WithBasePdf(blankPdfStream)
            .WithCrossIndustryInvoiceFile("TestFiles/cii.xml")
            .WithXmpMetadata(xmpContentStream)
            .BuildAsync();

        await using Stream xmpStream = await newFacturXDocument.GetXmpMetadataStreamAsync();
        using StreamReader streamReader = new(xmpStream);
        string newDocumentXmpString = await streamReader.ReadToEndAsync();

        // ensure value has been kept from the original xmp
        newDocumentXmpString.Should().Contain("<xmp:CreateDate>0001-02-03T04:05:06Z</xmp:CreateDate>");
    }

    [TestMethod]
    public async Task ShouldBuildDocument_WithAttachment()
    {
        byte[] xmpContent = "attachment_content"u8.ToArray();
        const string attachmentName = "attachment.ext";
        PdfAttachmentData attachment = new(attachmentName, xmpContent);

        PdfDocument blankPdf = new();
        blankPdf.AddPage();

        await using MemoryStream blankPdfStream = new();
        await blankPdf.SaveAsync(blankPdfStream);

        FacturXDocument newFacturXDocument = await FacturXDocument.Create()
            .WithBasePdf(blankPdfStream)
            .WithCrossIndustryInvoiceFile("TestFiles/cii.xml")
            .WithXmpMetadataFile("TestFiles/xmp.xml")
            .WithAttachment(attachment)
            .BuildAsync();

        List<FacturXDocumentAttachment> newDocumentAttachments = await newFacturXDocument.GetAttachmentsAsync().ToListAsync();
        FacturXDocumentAttachment? newDocumentAttachment = newDocumentAttachments.SingleOrDefault(a => a.Name == attachmentName);

        newDocumentAttachment.Should().NotBeNull();

        ReadOnlyMemory<byte> newDocumentAttachmentContent = await newDocumentAttachment.ReadAsync();

        newDocumentAttachmentContent.ToArray().Should().BeEquivalentTo(xmpContent.ToArray());
    }
}
