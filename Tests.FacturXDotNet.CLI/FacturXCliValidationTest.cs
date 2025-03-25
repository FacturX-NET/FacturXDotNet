using FacturXDotNet.CLI;
using Shouldly;

namespace Tests.FacturXDotNet.CLI;

[TestClass]
public class FacturXCliValidationTest
{
    [TestMethod]
    public async Task ShouldValidateFacturX()
    {
        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["validate", "TestFiles/facturx.pdf"]);

        result.ShouldBe(0);
    }

    [TestMethod]
    public async Task ShouldNotValidateFacturX_WithoutCii()
    {
        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["validate", "TestFiles/facturx_with_cii_named_xrechnung.pdf"]);
        result.ShouldBe(1);
    }

    [TestMethod]
    public async Task ShouldNotValidateFacturX_WithoutXmp()
    {
        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["validate", "TestFiles/facturx_without_xmp.pdf"]);
        result.ShouldBe(1);
    }
}
