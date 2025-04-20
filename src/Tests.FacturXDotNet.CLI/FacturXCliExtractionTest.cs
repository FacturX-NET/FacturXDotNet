using System.Xml.Linq;
using FacturXDotNet.CLI;
using FluentAssertions;

namespace Tests.FacturXDotNet.CLI;

[TestClass]
public class FacturXCliExtractionTest
{
    const string FacturXPath = "TestFiles/facturx.pdf";
    const string FacturXWithoutXmpPath = "TestFiles/facturx_without_xmp.pdf";
    const string FacturXWithCiiNameXRechnung = "TestFiles/facturx_with_cii_named_xrechnung.pdf";
    const string ExpectedCiiPath = "TestFiles/facturx.expected.xml";
    const string ExpectedXmpPath = "TestFiles/facturx.expected.xmp";

    [TestMethod]
    public async Task ShouldExtractCii()
    {
        string path = TestUtils.DuplicateFileAtRandomLocation(FacturXPath);
        string ciiPath = Path.Join(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + ".xml");

        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["extract", path, "--cii"]);

        result.Should().Be(0);
        CompareXmlFiles(ciiPath, ExpectedCiiPath);
    }

    [TestMethod]
    public async Task ShouldExtractCii_WithCustomName()
    {
        string customName = $"{Guid.CreateVersion7()}.xml";

        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["extract", FacturXPath, "--cii", customName]);

        result.Should().Be(0);
        CompareXmlFiles(customName, ExpectedCiiPath);
    }

    [TestMethod]
    public async Task ShouldFailToExtractCii_WhenNotInPdf()
    {
        string customName = $"{Guid.CreateVersion7()}.xml";

        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["extract", FacturXWithCiiNameXRechnung, "--cii", customName]);
        result.Should().Be(1);
    }

    [TestMethod]
    public async Task ShouldExtractXmp()
    {
        string path = TestUtils.DuplicateFileAtRandomLocation(FacturXPath);
        string xmpPath = Path.Join(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + ".xmp");

        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["extract", path, "--xmp"]);

        result.Should().Be(0);
        CompareXmlFiles(xmpPath, ExpectedXmpPath);
    }

    [TestMethod]
    public async Task ShouldExtractXmp_WithCustomName()
    {
        string customName = $"{Guid.CreateVersion7()}.xmp";

        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["extract", FacturXPath, "--xmp", customName]);

        result.Should().Be(0);
        CompareXmlFiles(customName, ExpectedXmpPath);
    }

    [TestMethod]
    public async Task ShouldFailToExtractXmp_WhenNotInPdf()
    {
        string customName = $"{Guid.CreateVersion7()}.xmp";

        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["extract", FacturXWithoutXmpPath, "--xmp", customName]);
        result.Should().Be(1);
    }

    static void CompareXmlFiles(string filePath, string expectedFilePath)
    {
        XDocument fileDocument = XDocument.Load(filePath);
        XDocument expectedFileDocument = XDocument.Load(expectedFilePath);

        fileDocument.Should().BeEquivalentTo(expectedFileDocument);
    }
}
