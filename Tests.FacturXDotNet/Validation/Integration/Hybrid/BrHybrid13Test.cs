using CommunityToolkit.HighPerformance;
using FacturXDotNet;
using FacturXDotNet.Generation;
using FacturXDotNet.Validation;
using Shouldly;

namespace Tests.FacturXDotNet.Validation.Integration.Hybrid;

[TestClass]
public class BrHybrid13Test
{
    [TestMethod]
    public async Task ShouldFail()
    {
        const string ciiAttachmentName = "some-other-name.xml";

        FacturXDocument basePdf = await FacturXDocument.LoadFromFileAsync("TestFiles/facturx.pdf");

        CrossIndustryInvoiceAttachment? ciiAttachment = await basePdf.GetCrossIndustryInvoiceAttachmentAsync();
        ReadOnlyMemory<byte> ciiContent = await ciiAttachment!.ReadAsync();
        await using Stream ciiStream = ciiContent.AsStream();

        FacturXDocument invoice = await FacturXDocument.Create().WithBasePdfFile("TestFiles/facturx.pdf").WithCrossIndustryInvoice(ciiStream, ciiAttachmentName).BuildAsync();

        FacturXValidator validator = new();
        FacturXValidationResult result = await validator.GetValidationResultAsync(invoice, ciiAttachmentName);

        BusinessRuleValidationResult rule = result.Rules.SingleOrDefault(r => r.Rule.Name == "BR-HYBRID-13");
        rule.Rule.ShouldNotBeNull();
        rule.Status.ShouldBe(BusinessRuleValidationStatus.Failed);
    }
}
