using FacturXDotNet;
using FacturXDotNet.Generation.PDF;
using FluentAssertions;

namespace Tests.FacturXDotNet.Generation;

[TestClass]
public class FacturXDocumentBuilderTest
{
    [TestMethod]
    public async Task ShouldBuildDocument_WithProvidedCii()
    {
        byte[] ciiContent = await File.ReadAllBytesAsync("TestFiles/cii.xml");
        await using MemoryStream ciiContentStream = new(ciiContent);

        await using FileStream basePdf = File.OpenRead("TestFiles/blank.pdf");

        FacturXDocument newFacturXDocument = await FacturXDocument.Create().WithBasePdf(basePdf).WithCrossIndustryInvoice(ciiContentStream).BuildAsync();
        CrossIndustryInvoiceAttachment? ciiAttachment = await newFacturXDocument.GetCrossIndustryInvoiceAttachmentAsync();

        ciiAttachment.Should().NotBeNull();

        ReadOnlyMemory<byte> newDocumentCiiContent = await ciiAttachment.ReadAsync();

        newDocumentCiiContent.ToArray().Should().BeEquivalentTo(ciiContent.ToArray());
    }

    [TestMethod]
    public async Task ShouldBuildDocument_WithXmp()
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

        await using FileStream basePdf = File.OpenRead("TestFiles/facturx.pdf");

        FacturXDocument newFacturXDocument = await FacturXDocument.Create().WithBasePdf(basePdf).WithXmpMetadata(xmpContentStream).BuildAsync();
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

        await using FileStream basePdf = File.OpenRead("TestFiles/facturx.pdf");

        FacturXDocument newFacturXDocument = await FacturXDocument.Create().WithBasePdf(basePdf).WithAttachment(attachment).BuildAsync();
        List<FacturXDocumentAttachment> newDocumentAttachments = await newFacturXDocument.GetAttachmentsAsync().ToListAsync();
        FacturXDocumentAttachment? newDocumentAttachment = newDocumentAttachments.SingleOrDefault(a => a.Name == attachmentName);

        newDocumentAttachment.Should().NotBeNull();

        ReadOnlyMemory<byte> newDocumentAttachmentContent = await newDocumentAttachment.ReadAsync();

        newDocumentAttachmentContent.ToArray().Should().BeEquivalentTo(xmpContent.ToArray());
    }
}
