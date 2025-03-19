using System.Text.Json;
using System.Text.Json.Serialization;
using FacturXDotNet;
using FacturXDotNet.Parsers.CII;
using FacturXDotNet.Parsers.FacturX;
using FacturXDotNet.Validation;
using FacturXDotNet.Validation.CII;
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
            Cii =
            {
                Logger = environment.Equals("development", StringComparison.InvariantCultureIgnoreCase) ? loggerFactory.CreateLogger<CrossIndustryInvoiceParser>() : null
            }
        }
    );

    CrossIndustryInvoice result = await parser.ParseCiiXmlInFacturXPdfAsync(example);

    logger.LogInformation("-------------");
    logger.LogInformation("   RESULT");
    logger.LogInformation("-------------");
    logger.LogInformation("{Result}", JsonSerializer.Serialize(result, SourceGenerationContext.Default.CrossIndustryInvoice));

    CrossIndustryInvoiceValidator validator = new();
    FacturXValidationResult validationResult = validator.GetValidationResult(result);

    logger.LogInformation("-------------");
    logger.LogInformation(" VALIDATION");
    logger.LogInformation("-------------");

    logger.LogInformation("Success: {Success}", validationResult.Success);
    logger.LogInformation("Actual profile: {Profile}", validationResult.ValidProfiles.GetMaxProfile());

    logger.LogInformation("Details:");

    if (validationResult.Failed.Count > 0)
    {
        logger.LogError("- Failed: ({FailedCount})", validationResult.Failed.Count);
    }
    else
    {
        logger.LogInformation("- Failed: ({FailedCount})", validationResult.Failed.Count);
    }
    foreach (FacturXBusinessRule rule in validationResult.Failed)
    {
        logger.LogError("  - KO [{Profile}] {Code}: {Description}", rule.Profiles.GetMinProfile(), rule.Name, rule.Description);
    }

    logger.LogInformation("- Passed: ({PassedCount})", validationResult.Passed.Count);
    foreach (FacturXBusinessRule rule in validationResult.Passed)
    {
        logger.LogInformation("  - OK [{Profile}] {Code}: {Description}", rule.Profiles.GetMinProfile(), rule.Name, rule.Description);
    }

    logger.LogInformation("- Expected to fail: ({ExpectedToFailCount})", validationResult.ExpectedToFail.Count);
    foreach (FacturXBusinessRule rule in validationResult.ExpectedToFail)
    {
        logger.LogInformation("  - KO [{Profile}] {Code}: {Description}", rule.Profiles.GetMinProfile(), rule.Name, rule.Description);
    }

    logger.LogInformation("- Skipped: ({SkippedCount})", validationResult.Skipped.Count);
    foreach (FacturXBusinessRule rule in validationResult.Skipped)
    {
        logger.LogInformation("  - ?? [{Profile}] {Code}: {Description}", rule.Profiles.GetMinProfile(), rule.Name, rule.Description);
    }


}
catch (Exception exn)
{
    logger.LogCritical(exn, "Unhandled exception.");
}

class Options
{
    public string? Environment { get; set; }
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(CrossIndustryInvoice))]
partial class SourceGenerationContext : JsonSerializerContext
{
}
