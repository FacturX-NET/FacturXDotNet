using FacturXDotNet.CLI;
using Shouldly;

namespace Tests.FacturXDotNet.CLI;

[TestClass]
public class FacturXCliValidationTest
{
    [TestMethod]
    public async Task ShouldValidateFacturX()
    {
        const string facturXPath = "TestFiles/facturx.pdf";
        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["validate", facturXPath]);

        result.ShouldBe(0);
    }
}
