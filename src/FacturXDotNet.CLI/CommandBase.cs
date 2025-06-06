using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine.Parsing;
using Spectre.Console;

namespace FacturXDotNet.CLI;

abstract class CommandBase<TOptions>(string name, string description, IReadOnlyList<Argument>? arguments = null, IReadOnlyList<Option>? options = null)
{
    public string Name { get; } = name;
    public string Description { get; } = description;
    public IReadOnlyList<Argument> Arguments => arguments ?? [];
    public IReadOnlyList<Option> Options => (options ?? []).Concat([GlobalOptions.VerbosityOption]).ToArray();

    public virtual Command GetCommand()
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

        command.Action = CommandHandler.Create(
            async (ParseResult result, CancellationToken cancellationToken) =>
            {
                try
                {
                    TOptions opt = ParseOptions(result.CommandResult);
                    return await RunImplAsync(opt, cancellationToken);
                }
                catch (Exception exception)
                {
                    AnsiConsole.WriteException(exception, ExceptionFormats.ShortenEverything);
                    return 1;
                }
            }
        );

        command.Validators.Add(
            result =>
            {
                try
                {
                    TOptions opt = ParseOptions(result);
                    ValidateOptions(result, opt);
                }
                catch (Exception exn)
                {
                    result.AddError(exn.Message);
                }
            }
        );

        return command;
    }

    protected abstract Task<int> RunImplAsync(TOptions options, CancellationToken cancellationToken = default);
    protected abstract TOptions ParseOptions(CommandResult result);
    protected virtual void ValidateOptions(CommandResult result, TOptions opt) { }
}
