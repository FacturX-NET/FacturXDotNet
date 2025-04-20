using CommunityToolkit.HighPerformance;
using FacturXDotNet;
using FacturXDotNet.Generation.FacturX;
using FluentAssertions;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;

namespace Tests.FacturXDotNet.Generation;

[TestClass]
public class FacturXDocumentBuilderAddOutputIntentsTest
{
    [TestMethod]
    public async Task ShouldAddOutputIntents_IfBasePdfDoesntHaveAny()
    {
        PdfDocument blankPdf = new();
        blankPdf.AddPage();

        await using MemoryStream blankPdfStream = new();
        await blankPdf.SaveAsync(blankPdfStream);

        FacturXDocument newFacturXDocument = await FacturXDocument.Create()
            .WithBasePdf(blankPdfStream)
            .WithCrossIndustryInvoiceFile("TestFiles/cii.xml")
            .WithXmpMetadataFile("TestFiles/xmp.xml")
            .BuildAsync();

        await using Stream newFacturXDocumentStream = newFacturXDocument.Data.AsStream();
        using PdfDocument pdfDocument = PdfReader.Open(newFacturXDocumentStream, PdfDocumentOpenMode.Import);

        PdfArray? outputIntents = pdfDocument.Internals.Catalog.Elements.GetArray("/OutputIntents");
        outputIntents.Should().NotBeNull();

        PdfDictionary? pdfAOutputIntent = outputIntents.Elements.OfType<PdfReference>()
            .Select(r => r.Value)
            .OfType<PdfDictionary>()
            .SingleOrDefault(d => d.Elements.GetName("/S") == "/GTS_PDFA1");
        pdfAOutputIntent.Should().NotBeNull();

        pdfAOutputIntent.Elements.GetName("/Type").Should().Be("/OutputIntent");
        pdfAOutputIntent.Elements.GetString("/OutputConditionIdentifier").Should().Contain("sRGB");

        PdfReference? destOutputProfileRef = pdfAOutputIntent.Elements.GetReference("/DestOutputProfile");
        destOutputProfileRef.Should().NotBeNull();

        PdfDictionary? destOutputProfile = destOutputProfileRef.Value as PdfDictionary;
        destOutputProfile.Should().NotBeNull();

        destOutputProfile.Stream.Length.Should().BeGreaterThan(0);
        destOutputProfile.Stream.Value.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task ShouldAddOutputIntents_IfBasePdfHaveSome_ButOfOtherType()
    {
        PdfDocument blankPdf = new();
        blankPdf.AddPage();

        PdfArray blankOutputIntents = new();
        blankPdf.Internals.Catalog.Elements.Add("/OutputIntents", blankOutputIntents);

        PdfDictionary blankOutputIntent = new();
        blankOutputIntent.Elements.Add("/S", new PdfName("/GTS_PDFX"));
        blankPdf.Internals.AddObject(blankOutputIntent);
        blankOutputIntents.Elements.Add(blankOutputIntent.ReferenceNotNull);

        await using MemoryStream blankPdfStream = new();
        await blankPdf.SaveAsync(blankPdfStream);

        FacturXDocument newFacturXDocument = await FacturXDocument.Create()
            .WithBasePdf(blankPdfStream)
            .WithCrossIndustryInvoiceFile("TestFiles/cii.xml")
            .WithXmpMetadataFile("TestFiles/xmp.xml")
            .BuildAsync();

        await using Stream newFacturXDocumentStream = newFacturXDocument.Data.AsStream();
        using PdfDocument pdfDocument = PdfReader.Open(newFacturXDocumentStream, PdfDocumentOpenMode.Import);

        PdfArray? outputIntents = pdfDocument.Internals.Catalog.Elements.GetArray("/OutputIntents");
        outputIntents.Should().NotBeNull();

        PdfDictionary? pdfAOutputIntent = outputIntents.Elements.OfType<PdfReference>()
            .Select(r => r.Value)
            .OfType<PdfDictionary>()
            .SingleOrDefault(d => d.Elements.GetName("/S") == "/GTS_PDFA1");
        pdfAOutputIntent.Should().NotBeNull();
    }

    [TestMethod]
    public async Task ShouldNotAddOutputIntents_IfBasePdfHaveSome_WithExpectedType()
    {
        PdfDocument blankPdf = new();
        blankPdf.AddPage();

        PdfArray blankOutputIntents = new();
        blankPdf.Internals.Catalog.Elements.Add("/OutputIntents", blankOutputIntents);

        PdfDictionary blankOutputIntent = new();
        blankOutputIntent.Elements.Add("/S", new PdfName("/GTS_PDFA1"));
        blankOutputIntent.Elements.Add("/OutputCondition", new PdfString("test value to check that this intent has not been replaced"));
        blankPdf.Internals.AddObject(blankOutputIntent);
        blankOutputIntents.Elements.Add(blankOutputIntent.ReferenceNotNull);

        await using MemoryStream blankPdfStream = new();
        await blankPdf.SaveAsync(blankPdfStream);

        FacturXDocument newFacturXDocument = await FacturXDocument.Create()
            .WithBasePdf(blankPdfStream)
            .WithCrossIndustryInvoiceFile("TestFiles/cii.xml")
            .WithXmpMetadataFile("TestFiles/xmp.xml")
            .BuildAsync();

        await using Stream newFacturXDocumentStream = newFacturXDocument.Data.AsStream();
        using PdfDocument pdfDocument = PdfReader.Open(newFacturXDocumentStream, PdfDocumentOpenMode.Import);

        PdfArray? outputIntents = pdfDocument.Internals.Catalog.Elements.GetArray("/OutputIntents");
        outputIntents.Should().NotBeNull();

        PdfDictionary? pdfAOutputIntent = outputIntents.Elements.OfType<PdfReference>()
            .Select(r => r.Value)
            .OfType<PdfDictionary>()
            .SingleOrDefault(d => d.Elements.GetName("/S") == "/GTS_PDFA1");
        pdfAOutputIntent.Should().NotBeNull();

        pdfAOutputIntent.Elements.GetString("/OutputCondition").Should().Be("test value to check that this intent has not been replaced");
    }
}
