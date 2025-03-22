using System.Diagnostics;
using System.Reflection;
using CommandLine;
using CommandLine.Text;
using FacturXDotNet.CLI.Extract;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
SerilogLoggerFactory loggerFactory = new(Log.Logger);

ILogger logger = loggerFactory.CreateLogger("Program");

try
{
    Parser parser = new(
        settings =>
        {
            settings.AutoVersion = true;
            settings.AutoHelp = true;
            settings.CaseSensitive = false;
            settings.CaseInsensitiveEnumValues = true;
            settings.HelpWriter = null;
        }
    );

    ParserResult<ExtractOption>? parsed = parser.ParseArguments<ExtractOption>(args);
    await parsed.WithParsedAsync(ExtractCommand.RunAsync);
    await parsed.WithNotParsedAsync(errs => NotParsedAsync(parsed, errs));
}
catch (Exception exn)
{
    logger.LogCritical(exn, "Unhandled exception.");
}
return;

async Task NotParsedAsync(ParserResult<ExtractOption> parsed1, IEnumerable<Error> errs)
{
    IEnumerable<Error> enumerable = errs as Error[] ?? errs.ToArray();
    if (enumerable.IsVersion())
    {
        await Console.Out.WriteLineAsync(GetVersion());
    }
    else if (enumerable.IsHelp())
    {
        await Console.Out.WriteLineAsync(GetHelp(parsed1));
    }
    else
    {
        await Console.Error.WriteLineAsync(GetHelp(parsed1));
    }
}

string GetVersion()
{
    Assembly assembly = Assembly.GetExecutingAssembly();
    FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
    return fileVersionInfo.ProductVersion ?? "~dev";
}

HelpText GetHelp(ParserResult<ExtractOption> parserResult)
{
    HelpText? helpText = HelpText.AutoBuild(
        parserResult,
        h =>
        {
            h.AdditionalNewLineAfterOption = false;
            h.AddEnumValuesToHelpText = true;
            h.Heading = $"FacturX.NET {GetVersion()}";
            h.Copyright = "Copyright (C) 2025 Ismail Bennani";
            return HelpText.DefaultParsingErrorsHandler(parserResult, h);
        },
        e => e
    );

    return helpText;
}
