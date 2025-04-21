using System.CommandLine;
using System.CommandLine.Help;
using System.Reflection;
using FacturXDotNet.CLI.Extract;
using FacturXDotNet.CLI.Generate;
using FacturXDotNet.CLI.MarkdownHelp;
using FacturXDotNet.CLI.Validate;

namespace FacturXDotNet.CLI;

public static class CommandLineConfigurationBuilder
{
    public static CommandLineConfiguration Build()
    {
        RootCommand rootCommand = new(
            """
            facturx generate --pdf image.pdf --cii facture.xml --validate");
            facturx validate facture.pdf"
            facturx extract facture.pdf --cii --xmp"
            """
        );

        CommandLineConfiguration configuration = new(rootCommand)
        {
            EnableDefaultExceptionHandler = true,
            EnablePosixBundling = true
        };


        HelpBuilder helpBuilder = GetCustomHelpBuilder();
        Option helpOption = rootCommand.Options.Single(opt => opt is HelpOption);
        rootCommand.Options.Remove(helpOption);
        rootCommand.Add(new HelpOption { Action = new HelpAction { Builder = helpBuilder } });

        rootCommand.Add(new GenerateCommand().GetCommand());
        rootCommand.Add(new ValidateCommand().GetCommand());
        rootCommand.Add(new ExtractCommand().GetCommand());
        rootCommand.Add(new MarkdownHelpCommand(rootCommand).GetCommand());

        return configuration;
    }

    static HelpBuilder GetCustomHelpBuilder()
    {
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
                        if (copyright is not null)
                        {
                            context.Output.WriteLine(copyright);
                        }
                        return true;
                    }
                )
        );
        return helpBuilder;
    }
}
