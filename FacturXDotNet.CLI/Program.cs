using System.CommandLine;
using System.Globalization;
using FacturXDotNet.CLI.Extract;

try
{
    // force error messages to be in English
    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

    RootCommand rootCommand = new("facturx extract facture.pdf --cii --xmp");
    rootCommand.AddCommand(new ExtractCommand());

    await new CommandLineConfiguration(rootCommand)
    {
        EnablePosixBundling = true
    }.InvokeAsync(args);

    return 0;
}
catch (Exception exn)
{
    await Console.Error.WriteLineAsync(exn.ToString());
    return 1;
}
finally
{
    await Console.Out.FlushAsync();
    await Console.Error.FlushAsync();
}
