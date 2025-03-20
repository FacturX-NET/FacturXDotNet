using System.Text.Json;
using System.Text.Json.Serialization;
using FacturXDotNet;
using FacturXDotNet.CLI;
using FacturXDotNet.Models;
using FacturXDotNet.Parsing;
using FacturXDotNet.Parsing.CII;
using FacturXDotNet.Parsing.XMP;
using FacturXDotNet.Validation;
using FacturXDotNet.Validation.BusinessRules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
SerilogLoggerFactory loggerFactory = new(Log.Logger);

ILogger logger = loggerFactory.CreateLogger("Program");

try
{
    IConfigurationRoot configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
    Options options = configuration.Get<Options>() ?? new Options();
    string environment = options.Environment ?? string.Empty;

    await using FileStream example = File.OpenRead(@"D:\source\repos\FacturXDotNet\FacturXDotNet.CLI\Examples\Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf");

    FacturXParser parser = new(
        new FacturXParserOptions
        {
            Xmp =
            {
                Logger = environment.Equals("development", StringComparison.InvariantCultureIgnoreCase) ? loggerFactory.CreateLogger<XmpMetadataParser>() : null
            },
            Cii =
            {
                Logger = environment.Equals("development", StringComparison.InvariantCultureIgnoreCase) ? loggerFactory.CreateLogger<CrossIndustryInvoiceParser>() : null
            }
        }
    );

    FacturX facturX = await parser.ParseFacturXPdfAsync(example);
    logger.LogInformation("-------------");
    logger.LogInformation("     XMP");
    logger.LogInformation("-------------");
    logger.LogInformation("{XMP}", JsonSerializer.Serialize(facturX.XmpMetadata, SourceGenerationContext.Default.XmpMetadata));

    logger.LogInformation("-------------");
    logger.LogInformation("     CII");
    logger.LogInformation("-------------");
    logger.LogInformation("{CII}", JsonSerializer.Serialize(facturX.CrossIndustryInvoice, SourceGenerationContext.Default.CrossIndustryInvoice));

    FacturXValidator validator = new();
    FacturXValidationResult validationResult = validator.GetValidationResult(facturX);

    logger.LogInformation("-------------");
    logger.LogInformation(" VALIDATION");
    logger.LogInformation("-------------");

    logger.LogInformation("Success: {Success}", validationResult.Success);
    logger.LogInformation("Actual profile: {Profile}", validationResult.ValidProfiles.GetMaxProfile());

    logger.LogInformation("Details:");

    if (validationResult.Fatal.Count > 0)
    {
        logger.LogError("- Failed: ({FailedCount})", validationResult.Fatal.Count);
    }
    else
    {
        logger.LogInformation("- Failed: ({FailedCount})", validationResult.Fatal.Count);
    }
    foreach (FacturXBusinessRule rule in validationResult.Fatal)
    {
        logger.LogError("  - KO {Rule}", rule.Format());
    }

    logger.LogInformation("- Passed: ({PassedCount})", validationResult.Passed.Count);
    foreach (FacturXBusinessRule? rule in validationResult.Passed)
    {
        logger.LogInformation("  - OK {Rule}", rule.Format());
    }

    logger.LogInformation("- Expected to fail: ({ExpectedToFailCount})", validationResult.ExpectedToFail.Count);
    foreach (FacturXBusinessRule? rule in validationResult.ExpectedToFail)
    {
        logger.LogInformation("  - KO {Rule}", rule.Format());
    }

    logger.LogInformation("- Skipped: ({SkippedCount})", validationResult.Skipped.Count);
    foreach (FacturXBusinessRule? rule in validationResult.Skipped)
    {
        logger.LogInformation("  - ?? {Rule}", rule.Format());
    }


}
catch (Exception exn)
{
    logger.LogCritical(exn, "Unhandled exception.");
}

namespace FacturXDotNet.CLI
{
    class Options
    {
        public string? Environment { get; set; }
    }

    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(CrossIndustryInvoice))]
    [JsonSerializable(typeof(XmpMetadata))]
    partial class SourceGenerationContext : JsonSerializerContext
    {
    }
}
