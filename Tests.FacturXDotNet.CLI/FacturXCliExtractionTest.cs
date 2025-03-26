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
        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["extract", FacturXPath, "--cii"]);

        result.Should().Be(0);
        CompareXmlFiles("TestFiles/facturx.xml", ExpectedCiiPath);
    }

    [TestMethod]
    public async Task ShouldFailToExtractCii_WhenNotInPdf()
    {
        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["extract", FacturXWithCiiNameXRechnung, "--cii"]);
        result.Should().Be(1);
    }

    [TestMethod]
    public async Task ShouldExtractCii_WithCustomName()
    {
        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["extract", FacturXPath, "--cii", "custom_name.xml"]);

        result.Should().Be(0);
        CompareXmlFiles("custom_name.xml", ExpectedCiiPath);
    }

    [TestMethod]
    public async Task ShouldExtractXmp()
    {
        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["extract", FacturXPath, "--xmp"]);

        result.Should().Be(0);
        CompareXmlFiles("TestFiles/facturx.xmp", ExpectedXmpPath);
    }

    [TestMethod]
    public async Task ShouldFailToExtractXmp_WhenNoXmpInPdf()
    {
        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["extract", FacturXWithoutXmpPath, "--xmp"]);
        result.Should().NotBe(0);
    }

    [TestMethod]
    public async Task ShouldExtractXmp_WithCustomName()
    {
        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["extract", FacturXPath, "--xmp", "custom_name.xml"]);

        result.Should().Be(0);
        CompareXmlFiles("custom_name.xml", ExpectedXmpPath);
    }

    static void CompareXmlFiles(string filePath, string expectedFilePath)
    {
        XDocument fileDocument = XDocument.Load(filePath);
        XDocument expectedFileDocument = XDocument.Load(expectedFilePath);

        fileDocument.Should().BeEquivalentTo(expectedFileDocument);
    }
}
