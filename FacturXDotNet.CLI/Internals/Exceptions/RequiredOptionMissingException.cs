using System.CommandLine;

namespace FacturXDotNet.CLI.Internals.Exceptions;

public class RequiredOptionMissingException(Option option) : Exception(GenerateMessage(option))
{
    static string GenerateMessage(Option option) => $"The option {option.Name} is required but was not provided.";
}