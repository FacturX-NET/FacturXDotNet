// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using FacturXDotNet.Models;
using FacturXDotNet.Models.Validation;
using FacturXDotNet.Parser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using ILoggerFactory factory = LoggerFactory.Create(
    builder => builder.AddSimpleConsole(
        options =>
        {
            options.TimestampFormat = "[yyyy-MM-ddTHH:mm:ss] ";
            options.UseUtcTimestamp = true;
            options.SingleLine = true;
        }
    )
);
ILogger logger = factory.CreateLogger("Program");

try
{
    JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web) { WriteIndented = true };

    IConfigurationRoot configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
    string environment = configuration.GetValue<string>("ENVIRONMENT") ?? string.Empty;

    FacturXCrossIndustryInvoiceParser parser = new(
        new FacturXCrossIndustryInvoiceParserOptions
        {
            Logger = environment.Equals("development", StringComparison.InvariantCultureIgnoreCase) ? factory.CreateLogger<FacturXCrossIndustryInvoiceParser>() : null
        }
    );

    await using FileStream example = File.OpenRead(@"D:\source\repos\FacturXDotNet\FacturXDotNet.CLI\Examples\Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.xml");
    FacturXCrossIndustryInvoice result = await parser.ParseAsync(example);

    logger.LogInformation("-------------");
    logger.LogInformation("   RESULT");
    logger.LogInformation("-------------");
    logger.LogInformation("{0}", JsonSerializer.Serialize(result, jsonSerializerOptions));

    FacturXCrossIndustryInvoiceValidator validator = new();
    FacturXValidationResult validationResult = validator.GetValidationResult(result);

    logger.LogInformation("-------------");
    logger.LogInformation(" VALIDATION");
    logger.LogInformation("-------------");

    logger.LogError("- Failed: ({0})", validationResult.Failed.Count);
    foreach (FacturXBusinessRule rule in validationResult.Failed)
    {
        logger.LogError("  - KO [{Profile}] {Code}: {Description}", rule.Profiles.GetMinProfile(), rule.Name, rule.Description);
    }

    logger.LogInformation("- Passed: ({0})", validationResult.Passed.Count);
    foreach (FacturXBusinessRule rule in validationResult.Passed)
    {
        logger.LogInformation("  - OK [{Profile}] {Code}: {Description}", rule.Profiles.GetMinProfile(), rule.Name, rule.Description);
    }

    logger.LogInformation("- Skipped: ({0})", validationResult.Skipped.Count);
    foreach (FacturXBusinessRule rule in validationResult.Skipped)
    {
        logger.LogInformation("  - ?? [{Profile}] {Code}: {Description}", rule.Profiles.GetMinProfile(), rule.Name, rule.Description);
    }


}
catch (Exception exn)
{
    logger.LogCritical(exn, "Unhandled exception.");
}
