using System.CommandLine;
using System.Globalization;
using FacturXDotNet.CLI;
using Spectre.Console;

try
{
    // force error messages to be in English
    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

    CommandLineConfiguration configuration = CommandLineConfigurationBuilder.Build();

#if DEBUG
    configuration.ThrowIfInvalid();
#endif

    return await configuration.InvokeAsync(args);
}
catch (Exception exn)
{
    AnsiConsole.WriteException(exn, ExceptionFormats.ShortenEverything);
    return 1;
}
finally
{
    await Console.Out.FlushAsync();
    await Console.Error.FlushAsync();
}
