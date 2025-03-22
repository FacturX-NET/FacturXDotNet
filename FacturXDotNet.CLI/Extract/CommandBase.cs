using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine.Parsing;

namespace FacturXDotNet.CLI.Extract;

abstract class CommandBase<TOption>(string name, string description, IReadOnlyList<Argument>? arguments = null, IReadOnlyList<Option>? options = null)
{
    public string Name { get; } = name;
    public string Description { get; } = description;
    public IReadOnlyList<Argument> Arguments => arguments ?? [];
    public IReadOnlyList<Option> Options => options ?? [];

    public Command GetCommand()
    {
        Command command = new(Name, Description);

        foreach (Argument argument in Arguments)
        {
            command.Add(argument);
        }

        foreach (Option option in Options)
        {
            command.Add(option);
        }

        command.Action = CommandHandler.Create<TOption, CancellationToken>(RunAsync);
        command.Validators.Add(Validate);

        return command;
    }

    public abstract Task RunAsync(TOption opt, CancellationToken cancellationToken = default);
    protected virtual void Validate(CommandResult option) { }
}

static class CommandBaseExtensions
{
    public static void AddCommand<T>(this RootCommand rootCommand, CommandBase<T> command) => rootCommand.Subcommands.Add(command.GetCommand());
}
