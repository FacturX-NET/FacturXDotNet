using System.CommandLine;
using System.CommandLine.Help;
using System.Globalization;
using System.Reflection;
using FacturXDotNet.CLI.Extract;
using Spectre.Console;

try
{
    // force error messages to be in English
    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

    RootCommand rootCommand = new("facturx extract facture.pdf --cii --xmp");

    CommandLineConfiguration configuration = new(rootCommand)
    {
        EnableDefaultExceptionHandler = true,
        EnablePosixBundling = true
    };

    #region Help customization

    Assembly assembly = typeof(Program).Assembly;
    AssemblyName assemblyName = assembly.GetName();
    string title = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? "FacturX.NET";
    string? copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;
    string version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? assemblyName.Version?.ToString() ?? "~dev";

    HelpBuilder helpBuilder = new();
    helpBuilder.CustomizeLayout(
        _ => HelpBuilder.Default.GetLayout()
            .Skip(1)
            .Prepend(
                context =>
                {
                    context.Output.WriteLine($"{title} v{version}");
                    if (copyright != null)
                    {
                        context.Output.WriteLine(copyright);
                    }
                    return true;
                }
            )
    );

    Option helpOption = rootCommand.Options.Single(opt => opt is HelpOption);
    rootCommand.Options.Remove(helpOption);
    rootCommand.Add(new HelpOption { Action = new HelpAction { Builder = helpBuilder } });

    #endregion

    #region Register commands

    rootCommand.AddCommand(new ExtractCommand());

    #endregion

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
