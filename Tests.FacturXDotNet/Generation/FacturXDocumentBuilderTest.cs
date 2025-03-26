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
        byte[] ciiContent = "cii_content"u8.ToArray();
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
        byte[] xmpContent = "xmp_content"u8.ToArray();
        await using MemoryStream xmpContentStream = new(xmpContent);

        await using FileStream basePdf = File.OpenRead("TestFiles/blank.pdf");

        FacturXDocument newFacturXDocument = await FacturXDocument.Create().WithBasePdf(basePdf).WithXmpMetadata(xmpContentStream).BuildAsync();
        await using Stream xmpStream = await newFacturXDocument.GetXmpMetadataStreamAsync();
        byte[] newDocumentXmpContent = new byte[xmpStream.Length];
        await xmpStream.ReadExactlyAsync(newDocumentXmpContent);

        newDocumentXmpContent.ToArray().Should().BeEquivalentTo(xmpContent.ToArray());
    }

    [TestMethod]
    public async Task ShouldBuildDocument_WithAttachment()
    {
        byte[] xmpContent = "attachment_content"u8.ToArray();
        const string attachmentName = "attachment.ext";
        PdfAttachmentData attachment = new(attachmentName, xmpContent);

        await using FileStream basePdf = File.OpenRead("TestFiles/blank.pdf");

        FacturXDocument newFacturXDocument = await FacturXDocument.Create().WithBasePdf(basePdf).WithAttachment(attachment).BuildAsync();
        List<FacturXDocumentAttachment> newDocumentAttachments = await newFacturXDocument.GetAttachmentsAsync().ToListAsync();
        FacturXDocumentAttachment? newDocumentAttachment = newDocumentAttachments.SingleOrDefault(a => a.Name == attachmentName);

        newDocumentAttachment.Should().NotBeNull();

        ReadOnlyMemory<byte> newDocumentAttachmentContent = await newDocumentAttachment.ReadAsync();

        newDocumentAttachmentContent.ToArray().Should().BeEquivalentTo(xmpContent.ToArray());
    }
}
