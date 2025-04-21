using System.CommandLine;
using System.CommandLine.Parsing;
using System.Reflection;
using System.Text;
using Spectre.Console;

namespace FacturXDotNet.CLI.MarkdownHelp;

class MarkdownHelpCommand(RootCommand rootCommand) : CommandBase<MarkdownHelpOptions>("help-as-md", "Produce help and usage information as markdown files.", [], [OutputPathOption])
{
    const string DefaultOutputPath = "help-as-md";

    static readonly Option<string> OutputPathOption;

    static readonly string AssemblyTitle;
    static readonly string? AssemblyCopyright;
    static readonly string AssemblyVersion;

    static MarkdownHelpCommand()
    {
        OutputPathOption = new Option<string>("--output-path", "-o")
        {
            Description = "The path to the output file.",
            HelpName = "path",
            DefaultValueFactory = _ => DefaultOutputPath
        };

        Assembly assembly = typeof(Program).Assembly;
        AssemblyName assemblyName = assembly.GetName();
        AssemblyTitle = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? "FacturX.NET";
        AssemblyCopyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;
        AssemblyVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? assemblyName.Version?.ToString() ?? "~dev";
    }

    public override Command GetCommand()
    {
        Command result = base.GetCommand();
        result.Hidden = true;
        return result;
    }

    protected override async Task<int> RunImplAsync(MarkdownHelpOptions options, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(options.OutputPath))
        {
            if (Directory.Exists(options.OutputPath))
            {
                Directory.Delete(options.OutputPath, true);
            }

            if (!Directory.Exists(options.OutputPath))
            {
                Directory.CreateDirectory(options.OutputPath);
            }
        }

        string path = options.OutputPath ?? ".";
        AnsiConsole.WriteLine("Generating markdown files at {0}...", path);

        string rootCommandPath = Path.Join(path, $"{rootCommand.Name}.md");
        AnsiConsole.WriteLine("Generating markdown file for root command {0} at {1}...", rootCommand.Name, rootCommandPath);
        await WriteRootCommandFileAsync(rootCommand, rootCommandPath);

        const string subCommandsPath = "subcommands";
        Directory.CreateDirectory(Path.Join(path, subCommandsPath));

        foreach (Command subcommand in rootCommand.Subcommands.Where(c => !c.Hidden))
        {
            string subCommandPath = Path.Join(path, subCommandsPath, $"{subcommand.Name}.md");
            AnsiConsole.WriteLine("Generating markdown file for subcommand {0} at {1}...", subcommand.Name, subCommandPath);
            await WriteSubCommandFileAsync(subcommand, subCommandPath);
        }

        return 0;
    }

    static async Task WriteRootCommandFileAsync(RootCommand command, string path)
    {
        await using FileStream stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
        stream.SetLength(0);

        await using StreamWriter writer = new(stream);

        await writer.WriteLineAsync($"# {AssemblyTitle}");
        await writer.WriteLineAsync($"**Version**: {AssemblyVersion}\\");
        await writer.WriteLineAsync($"**Copyright**: {AssemblyCopyright}\\");

        await WriteCommandHelpAsync(writer, command);
    }

    static async Task WriteSubCommandFileAsync(Command command, string path)
    {
        await using FileStream stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
        stream.SetLength(0);

        await using StreamWriter writer = new(stream);

        await writer.WriteLineAsync($"# {command.Name}");
        await writer.WriteLineAsync($"{command.Description}");

        await WriteCommandHelpAsync(writer, command);
    }

    static async Task WriteCommandHelpAsync(StreamWriter writer, Command command)
    {
        await writer.WriteLineAsync("## Usage");
        await writer.WriteLineAsync("```");
        await writer.WriteLineAsync(MarkdownHelpUtils.GetUsage(command));
        await writer.WriteLineAsync("```");

        if (command.Arguments.Count > 0)
        {
            await writer.WriteLineAsync("## Arguments");
            await writer.WriteLineAsync("| Argument | Type | Description |");
            await writer.WriteLineAsync("| :--- | :--- | :--- |");
            foreach (Argument argument in command.Arguments.Where(o => !o.Hidden))
            {
                await WriteArgumentHelpAsync(writer, argument);
            }
        }

        if (command.Options.Count > 0)
        {
            await writer.WriteLineAsync("## Options");
            await writer.WriteLineAsync("| Option | Type | Description |");
            await writer.WriteLineAsync("| :--- | :--- | :--- |");
            foreach (Option option in command.Options.Where(o => !o.Hidden))
            {
                await WriteOptionHelpAsync(writer, option);
            }
        }

        if (command.Subcommands.Count > 0)
        {
            await writer.WriteLineAsync("## Sub-commands");
            foreach (Command subcommand in command.Subcommands.Where(o => !o.Hidden))
            {
                await WriteSubCommandHelpAsync(writer, subcommand);
            }
        }
    }

    static async Task WriteArgumentHelpAsync(StreamWriter writer, Argument argument) =>
        await writer.WriteLineAsync($"| {argument.Name} | {TypeToString(argument.ValueType)} | {argument.Description} |");

    static async Task WriteOptionHelpAsync(StreamWriter writer, Option option)
    {
        StringBuilder nameBuilder = new();
        nameBuilder.Append(option.Name);
        nameBuilder.Append(string.Join(string.Empty, option.Aliases.Select(a => $", {a}")));

        await writer.WriteLineAsync($"| {nameBuilder} | {TypeToString(option.ValueType)} | {option.Description} |");
    }

    static async Task WriteSubCommandHelpAsync(StreamWriter writer, Command subcommand)
    {
        string subcommandTitle = subcommand.Description ?? subcommand.Name;
        subcommandTitle = subcommandTitle.EndsWith('.') ? subcommandTitle[..^1] : subcommandTitle;
        await writer.WriteLineAsync($"### [{subcommandTitle}](./subcommands/{subcommand.Name})");

        if (subcommand.Aliases.Count > 0)
        {
            await writer.WriteLineAsync($"Aliases: {string.Join(", ", subcommand.Aliases)}");
        }
    }

    static string TypeToString(Type type)
    {
        if (type.IsEnum)
        {
            return EnumTypeToString(type);
        }

        if (type == typeof(string))
        {
            return "string";
        }

        if (type == typeof(bool))
        {
            return "\"true\", \"false\"";
        }

        return type.ToString();
    }

    static string EnumTypeToString(Type type)
    {
        IEnumerable<object> values = Enum.GetValues(type).OfType<object>();
        return string.Join(", ", values.Select(v => $"\"{v.ToString()?.ToLowerInvariant()}\""));
    }

    protected override MarkdownHelpOptions ParseOptions(CommandResult result) =>
        new()
        {
            OutputPath = result.GetValue(OutputPathOption)
        };
}

class MarkdownHelpOptions
{
    /// <summary>
    ///     The path to the output file.
    /// </summary>
    public string? OutputPath { get; set; }
}
