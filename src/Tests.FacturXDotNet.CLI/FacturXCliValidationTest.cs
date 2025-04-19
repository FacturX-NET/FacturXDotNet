using FacturXDotNet.CLI;
using FluentAssertions;

namespace Tests.FacturXDotNet.CLI;

[TestClass]
public class FacturXCliValidationTest
{
    [TestMethod]
    public async Task ShouldValidateFacturX()
    {
        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["validate", "TestFiles/facturx.pdf"]);

        result.Should().Be(0);
    }

    [TestMethod]
    public async Task ShouldUseCustomName_ForCii()
    {
        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["validate", "TestFiles/facturx_with_xrechnung.pdf", "--cii-name", "xrechnung.xml"]);
        result.Should().Be(1);
    }

    [TestMethod]
    public async Task ShouldNotValidateFacturX_WithoutCii()
    {
        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["validate", "TestFiles/facturx_with_xrechnung.pdf"]);
        result.Should().Be(1);
    }

    [TestMethod]
    public async Task ShouldNotValidateFacturX_WithoutXmp()
    {
        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["validate", "TestFiles/facturx_without_xmp.pdf"]);
        result.Should().Be(1);
    }
}
