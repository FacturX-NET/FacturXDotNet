// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using FacturXDotNet.Models;
using FacturXDotNet.Parser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = factory.CreateLogger("Program");

IConfigurationRoot configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
string environment = configuration.GetValue<string>("ENVIRONMENT") ?? string.Empty;

try
{
    FacturXCrossIndustryInvoiceParser parser = new(
        new FacturXCrossIndustryInvoiceParserOptions
            { Logger = environment.Equals("development", StringComparison.InvariantCultureIgnoreCase) ? factory.CreateLogger<FacturXCrossIndustryInvoiceParser>() : null }
    );
    await using FileStream example = File.OpenRead(@"D:\source\repos\FacturXDotNet\FacturXDotNet.CLI\Examples\Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.xml");
    FacturXCrossIndustryInvoice result = await parser.ParseAsync(example);

    Console.WriteLine("RESULT");
    Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true }));
}
catch (Exception exn)
{
    logger.LogCritical(exn, "Unhandled exception.");
}
