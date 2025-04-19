using FacturXDotNet.CLI;
using FluentAssertions;

namespace Tests.FacturXDotNet.CLI;

[TestClass]
public class FacturXCliGenerationTest
{
    [TestMethod]
    public async Task ShouldGenerateFacturX()
    {
        string outputPath = $"{Guid.CreateVersion7()}.pdf";

        int result = await CommandLineConfigurationBuilder.Build()
            .InvokeAsync(["generate", "--pdf", "TestFiles/facturx.pdf", "--cii", "TestFiles/facturx.expected.xml", "-o", outputPath]);

        result.Should().Be(0);
        File.Exists(outputPath).Should().BeTrue();
    }

    [TestMethod]
    public async Task ShouldNotGenerateFacturX_WhenValidationFails()
    {
        string outputPath = $"{Guid.CreateVersion7()}.pdf";

        int result = await CommandLineConfigurationBuilder.Build().InvokeAsync(["generate", "--pdf", "TestFiles/facturx.pdf", "--cii", "TestFiles/bad_cii.xml", "-o", outputPath]);

        result.Should().Be(1);
        File.Exists(outputPath).Should().BeFalse();
    }

    [TestMethod]
    public async Task ShouldGenerateFacturX_WhenNoValidation()
    {
        string outputPath = $"{Guid.CreateVersion7()}.pdf";

        int result = await CommandLineConfigurationBuilder.Build()
            .InvokeAsync(["generate", "--pdf", "TestFiles/facturx.pdf", "--cii", "TestFiles/bad_cii.xml", "--skip-validation", "-o", outputPath]);

        result.Should().Be(0);
        File.Exists(outputPath).Should().BeTrue();
    }
}
