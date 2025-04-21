using System.CommandLine;
using System.Reflection;

namespace FacturXDotNet.CLI.MarkdownHelp;

static class OptionExtensions
{
    public static object? GetDefaultValue(this Option option)
    {
        PropertyInfo argumentProperty = typeof(Option).GetProperty("Argument", BindingFlags.Instance | BindingFlags.NonPublic)
                                        ?? throw new InvalidOperationException("Could not find property 'Argument'");
        Argument argument = (Argument)(argumentProperty.GetValue(option) ?? throw new InvalidOperationException("Could not find Argument of Option"));
        return argument.GetDefaultValue();
    }
}
