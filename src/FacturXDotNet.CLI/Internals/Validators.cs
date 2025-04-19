using System.CommandLine.Parsing;

namespace FacturXDotNet.CLI.Internals;

class Validators
{
    public static void FileExists(OptionResult result)
    {
        FileInfo? path = result.GetValueOrDefault<FileInfo>();
        if (path == null)
        {
            result.AddError("The path has not been provided.");
        }
        else if (!path.Exists)
        {
            result.AddError($"Could not find file '{path.FullName}'.");
        }
    }
}
