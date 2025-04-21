using System.CommandLine;
using System.Text;

namespace FacturXDotNet.CLI.MarkdownHelp;

/// <remarks>
///     Copied from https://github.com/dotnet/command-line-api/blob/main/src/System.CommandLine/Help/HelpBuilder.cs
/// </remarks>
static class MarkdownHelpUtils
{
    public static string GetUsage(Command command)
    {
        return string.Join(" ", GetUsageParts().Where(x => !string.IsNullOrWhiteSpace(x)));

        IEnumerable<string> GetUsageParts()
        {
            bool displayOptionTitle = false;

            IEnumerable<Command> parentCommands = RecurseWhileNotNull(command, c => c.Parents.OfType<Command>().FirstOrDefault()).Reverse();

            foreach (Command parentCommand in parentCommands)
            {
                if (!displayOptionTitle)
                {
                    displayOptionTitle = parentCommand.Options.Count > 0 && parentCommand.Options.Any(x => x.Recursive && !x.Hidden);
                }

                yield return parentCommand.Name;

                if (parentCommand.Arguments.Count > 0)
                {
                    yield return FormatArgumentUsage(parentCommand.Arguments);
                }
            }

            bool hasCommandWithHelp = command.Subcommands.Count > 0 && command.Subcommands.Any(x => !x.Hidden);

            if (hasCommandWithHelp)
            {
                yield return "[command]";
            }

            displayOptionTitle = displayOptionTitle || command.Options.Count > 0 && command.Options.Any(x => !x.Hidden);

            if (displayOptionTitle)
            {
                yield return "[options]";
            }

            if (!command.TreatUnmatchedTokensAsErrors)
            {
                yield return "[[--] <additional arguments>...]]";
            }
        }
    }

    static string FormatArgumentUsage(IList<Argument> arguments)
    {
        StringBuilder sb = new(arguments.Count * 100);

        List<char>? end = default;

        for (int i = 0; i < arguments.Count; i++)
        {
            Argument argument = arguments[i];
            if (argument.Hidden)
            {
                continue;
            }

            string arityIndicator = argument.Arity.MaximumNumberOfValues > 1 ? "..." : "";

            bool isOptional = IsOptional(argument);

            if (isOptional)
            {
                sb.Append($"[<{argument.Name}>{arityIndicator}");
                (end ??= new List<char>()).Add(']');
            }
            else
            {
                sb.Append($"<{argument.Name}>{arityIndicator}");
            }

            sb.Append(' ');
        }

        if (sb.Length > 0)
        {
            sb.Length--;

            if (end is { })
            {
                while (end.Count > 0)
                {
                    sb.Append(end[^1]);
                    end.RemoveAt(end.Count - 1);
                }
            }
        }

        return sb.ToString();

        bool IsOptional(Argument argument)
        {
            return argument.Arity.MinimumNumberOfValues == 0;
        }
    }

    static IEnumerable<T> RecurseWhileNotNull<T>(T? source, Func<T, T?> next) where T: class
    {
        while (source is not null)
        {
            yield return source;
            source = next(source);
        }
    }
}
