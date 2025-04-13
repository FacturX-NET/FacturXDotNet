using FacturXDotNet;
using FacturXDotNet.Generation.FacturX;
using FacturXDotNet.Parsing.CII.Exceptions;
using FacturXDotNet.Parsing.XMP.Exceptions;
using FacturXDotNet.Validation;
using FluentAssertions;

namespace Tests.FacturXDotNet.Validation.Integration;

static class ValidationIntegrationTestUtils
{
    public static async Task CheckRulePasses(string ruleName, string? xmp = null, string? cii = null) => (await CheckRule(ruleName, xmp, cii)).Should().BeTrue();
    public static async Task CheckRuleFails(string ruleName, string? xmp = null, string? cii = null) => (await CheckRule(ruleName, xmp, cii)).Should().BeFalse();

    static async Task<bool> CheckRule(string ruleName, string? xmp = null, string? cii = null)
    {
        FacturXDocumentBuilder builder = FacturXDocument.Create().WithBasePdfFile("TestFiles/facturx.pdf");

        if (xmp is not null)
        {
            MemoryStream stream = new();
            await using StreamWriter streamWriter = new(stream, leaveOpen: true);
            await streamWriter.WriteAsync(xmp);
            await streamWriter.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);
            builder.WithXmpMetadata(stream, opt => opt.LeaveOpen = false);
        }

        if (cii is not null)
        {
            MemoryStream stream = new();
            await using StreamWriter streamWriter = new(stream, leaveOpen: true);
            await streamWriter.WriteAsync(cii);
            await streamWriter.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);
            builder.WithCrossIndustryInvoice(stream, opt => opt.LeaveOpen = false);
        }

        FacturXValidationResult result;

        try
        {
            FacturXDocument invoice = await builder.BuildAsync();

            FacturXValidator validator = new();
            result = await validator.GetValidationResultAsync(invoice);
        }
        catch (XmpMetadataReaderException)
        {
            // Failing to read the XMP is expected in some cases, it is assumed to be a failed validation
            return false;
        }
        catch (CrossIndustryInvoiceReaderException)
        {
            // Failing to read the CII is expected in some cases, it is assumed to be a failed validation
            return false;
        }

        BusinessRuleValidationResult rule = result.Rules.SingleOrDefault(r => r.Rule.Name == ruleName);
        if (rule.Rule is null)
        {
            throw new InvalidOperationException($"Could not find rule {ruleName}.");
        }

        if (rule.Status == BusinessRuleValidationStatus.Skipped)
        {
            throw new InvalidOperationException($"Rule {ruleName} has been skipped.");
        }

        return rule.Status == BusinessRuleValidationStatus.Passed;
    }
}
