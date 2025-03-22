using FacturXDotNet.CLI;
using Shouldly;

namespace Tests.FacturXDotNet.CLI;

[TestClass]
public class FacturXCliExtractionTest
{
    [TestMethod]
    public async Task ShouldExtractCii()
    {
        const string facturXPath = "TestFiles/facturx.pdf";
        await CommandLineConfigurationBuilder.Build().InvokeAsync(["extract", facturXPath, "--cii"]);

        await CompareFileContents("TestFiles/facturx.xml", "TestFiles/facturx.expected.xml");
    }

    [TestMethod]
    public async Task ShouldExtractCii_WithCustomName()
    {
        const string facturXPath = "TestFiles/facturx.pdf";
        await CommandLineConfigurationBuilder.Build().InvokeAsync(["extract", facturXPath, "--cii", "custom_name.xml"]);

        await CompareFileContents("custom_name.xml", "TestFiles/facturx.expected.xml");
    }

    [TestMethod]
    public async Task ShouldExtractXmp()
    {
        const string facturXPath = "TestFiles/facturx.pdf";
        await CommandLineConfigurationBuilder.Build().InvokeAsync(["extract", facturXPath, "--xmp"]);

        await CompareFileContents("TestFiles/facturx.xmp", "TestFiles/facturx.expected.xmp");
    }

    [TestMethod]
    public async Task ShouldExtractXmp_WithCustomName()
    {
        const string facturXPath = "TestFiles/facturx.pdf";
        await CommandLineConfigurationBuilder.Build().InvokeAsync(["extract", facturXPath, "--xmp", "custom_name.xml"]);

        await CompareFileContents("custom_name.xml", "TestFiles/facturx.expected.xmp");
    }

    static async Task CompareFileContents(string filePath, string expectedFilePath)
    {
        string file = await File.ReadAllTextAsync(filePath);
        string expectedFile = await File.ReadAllTextAsync(expectedFilePath);
        file.ShouldBe(expectedFile);
    }
}
