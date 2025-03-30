using System.CommandLine;

namespace FacturXDotNet.CLI.Internals.Exceptions;

public class RequiredArgumentMissingException(Argument argument) : Exception(GenerateMessage(argument))
{
    static string GenerateMessage(Argument argument) => $"The argument {argument.Name} is required but was not provided.";
}
