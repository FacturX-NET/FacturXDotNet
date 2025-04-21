using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;
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

        await writer.WriteLineAsync("---");
        await writer.WriteLineAsync("title: CLI");
        await writer.WriteLineAsync("editLink: false");
        await writer.WriteLineAsync("prev: false");
        await writer.WriteLineAsync("next: false");
        await writer.WriteLineAsync("---");
        await writer.WriteLineAsync("");

        await writer.WriteLineAsync($"# {AssemblyTitle}");
        await writer.WriteLineAsync($"**Version**: {AssemblyVersion}\\");
        await writer.WriteLineAsync($"**Copyright**: {AssemblyCopyright}");

        await WriteCommandHelpAsync(writer, command);
    }

    static async Task WriteSubCommandFileAsync(Command command, string path)
    {
        await using FileStream stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
        stream.SetLength(0);

        await using StreamWriter writer = new(stream);

        await writer.WriteLineAsync("---");
        await writer.WriteLineAsync($"title: CLI {command.Name}");
        await writer.WriteLineAsync("editLink: false");
        await writer.WriteLineAsync("prev: false");
        await writer.WriteLineAsync("next: false");
        await writer.WriteLineAsync("---");
        await writer.WriteLineAsync("");

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
            foreach (Argument argument in command.Arguments.Where(o => !o.Hidden))
            {
                await WriteArgumentHelpAsync(writer, argument);
            }
            await writer.WriteLineAsync();
        }

        if (command.Options.Count > 0)
        {
            await writer.WriteLineAsync("## Options");
            foreach (Option option in command.Options.Where(o => !o.Hidden))
            {
                await WriteOptionHelpAsync(writer, option);
            }
            await writer.WriteLineAsync();
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

    static async Task WriteArgumentHelpAsync(StreamWriter writer, Argument argument) =>
        await WriteSymbolHelpAsync(
            writer,
            new SymbolHelpArgs
            {
                Name = $"<{argument.Name}>",
                Description = argument.Description,
                Type = argument.ValueType,
                DefaultValue = argument.HasDefaultValue ? argument.GetDefaultValue() : null
            }
        );

    static async Task WriteOptionHelpAsync(StreamWriter writer, Option option) =>
        await WriteSymbolHelpAsync(
            writer,
            new SymbolHelpArgs
            {
                Name = option.Name,
                HelpName = option.HelpName,
                Description = option.Description,
                Required = option.Required,
                NameAliases = option.Aliases,
                Type = option.ValueType,
                DefaultValue = option.HasDefaultValue ? option.GetDefaultValue() : null
            }
        );

    static async Task WriteSymbolHelpAsync(StreamWriter writer, SymbolHelpArgs symbolHelpArgs)
    {
        StringBuilder nameBuilder = new();

        if (!string.IsNullOrWhiteSpace(symbolHelpArgs.HelpName))
        {
            nameBuilder.Append($"`{symbolHelpArgs.Name} <{symbolHelpArgs.HelpName}>`");
        }
        else
        {
            nameBuilder.Append($"`{symbolHelpArgs.Name}`");
        }


        if (symbolHelpArgs.Required)
        {
            await writer.WriteAsync($"- {nameBuilder} **(required)**");
        }
        else
        {
            await writer.WriteAsync($"- {nameBuilder}");
        }

        if (!string.IsNullOrWhiteSpace(symbolHelpArgs.Description))
        {
            await writer.WriteAsync($"\\\n{symbolHelpArgs.Description}");
        }

        if (symbolHelpArgs.NameAliases.Count > 0)
        {
            await writer.WriteAsync($"\\\n**Aliases**: {string.Join(", ", symbolHelpArgs.NameAliases.Select(a => $"`{a}`"))}");
        }

        string[] types = TypeToStrings(symbolHelpArgs.Type);
        await writer.WriteAsync($"\\\n**Type**: `{string.Join(" | ", types)}`");

        if (symbolHelpArgs.DefaultValue != null)
        {
            // heuristic: if there are multiple types, this is an enum, and we must lowercase the value
            string? defaultValueString = types.Length > 0 ? symbolHelpArgs.DefaultValue.ToString()?.ToLowerInvariant() : symbolHelpArgs.DefaultValue.ToString();
            await writer.WriteAsync($"\\\n**Default**: `{defaultValueString}`");
        }

        await writer.WriteLineAsync("\n");
    }

    static bool IsEnumerableType(Type type, [NotNullWhen(true)] out Type? elementType)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            elementType = type.GetGenericArguments()[0];
            return true;
        }

        Type? enumerableInterface = type.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        if (enumerableInterface != null)
        {
            elementType = enumerableInterface.GetGenericArguments()[0];
            return true;
        }

        elementType = null;
        return false;
    }

    static string[] TypeToStrings(Type type)
    {
        if (IsEnumerableType(type, out Type? elementType))
        {
            if (elementType == typeof(char))
            {
                return SimpleTypeToString(typeof(string));
            }

            return TypeToStrings(elementType);
        }

        if (type.IsEnum)
        {
            return EnumTypeToString(type).ToArray();
        }

        return SimpleTypeToString(type);
    }

    static string[] SimpleTypeToString(Type type)
    {
        if (type == typeof(string) || type == typeof(FileInfo))
        {
            return ["string"];
        }

        if (type == typeof(bool))
        {
            return ["true", "false"];
        }

        return [type.ToString()];
    }

    static IEnumerable<string> EnumTypeToString(Type type) => Enum.GetValues(type).OfType<object>().Select(v => v.ToString()?.ToLowerInvariant() ?? "");

    protected override MarkdownHelpOptions ParseOptions(CommandResult result) =>
        new()
        {
            OutputPath = result.GetValue(OutputPathOption)
        };

    class SymbolHelpArgs
    {
        public required string Name { get; set; }
        public required Type Type { get; set; }
        public string? HelpName { get; set; }
        public string? Description { get; set; }
        public bool Required { get; set; }
        public ICollection<string> NameAliases { get; set; } = [];
        public object? DefaultValue { get; set; }
    }
}

class MarkdownHelpOptions
{
    /// <summary>
    ///     The path to the output file.
    /// </summary>
    public string? OutputPath { get; set; }
}
