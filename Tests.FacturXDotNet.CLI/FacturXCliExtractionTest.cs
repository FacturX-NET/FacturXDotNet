using System.Xml.Linq;
using FacturXDotNet.CLI;
using Shouldly;

namespace Tests.FacturXDotNet.CLI;

[TestClass]
public class FacturXCliExtractionTest
{
    const string FacturXPath = "TestFiles/facturx.pdf";
    const string ExpectedCiiPath = "TestFiles/facturx.expected.xml";
    const string ExpectedXmpPath = "TestFiles/facturx.expected.xmp";

    [TestMethod]
    public async Task ShouldExtractCii()
    {
        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["extract", FacturXPath, "--cii"]);

        result.ShouldBe(0);
        CompareXmlFiles("TestFiles/facturx.xml", ExpectedCiiPath);
    }

    [TestMethod]
    public async Task ShouldExtractCii_WithCustomName()
    {
        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["extract", FacturXPath, "--cii", "custom_name.xml"]);

        result.ShouldBe(0);
        CompareXmlFiles("custom_name.xml", ExpectedCiiPath);
    }

    [TestMethod]
    public async Task ShouldExtractXmp()
    {
        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["extract", FacturXPath, "--xmp"]);

        result.ShouldBe(0);
        CompareXmlFiles("TestFiles/facturx.xmp", ExpectedXmpPath);
    }

    [TestMethod]
    public async Task ShouldExtractXmp_WithCustomName()
    {
        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["extract", FacturXPath, "--xmp", "custom_name.xml"]);

        result.ShouldBe(0);
        CompareXmlFiles("custom_name.xml", ExpectedXmpPath);
    }

    static void CompareXmlFiles(string filePath, string expectedFilePath)
    {
        XDocument fileDocument = XDocument.Load(filePath);
        XDocument expectedFileDocument = XDocument.Load(expectedFilePath);

        fileDocument.ShouldBeEquivalentTo(expectedFileDocument);
    }
}
