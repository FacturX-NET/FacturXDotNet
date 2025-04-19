using System.CommandLine;

namespace FacturXDotNet.CLI;

static class GlobalOptions
{
    static GlobalOptions()
    {
        VerbosityOption = new Option<Verbosity>("--verbosity", "-v")
        {
            Description = "Set the verbosity level.",
            DefaultValueFactory = _ => Verbosity.Normal,
            CustomParser = result => result.Tokens[0].Value switch
            {
                "q" => Verbosity.Quiet,
                "quiet" => Verbosity.Quiet,
                "m" => Verbosity.Minimal,
                "minimal" => Verbosity.Minimal,
                "n" => Verbosity.Normal,
                "normal" => Verbosity.Normal,
                "d" => Verbosity.Detailed,
                "detailed" => Verbosity.Detailed,
                "diag" => Verbosity.Diagnostic,
                "diagnostic" => Verbosity.Diagnostic,
                _ => throw new ArgumentException($"Invalid verbosity level: {result.GetValueOrDefault<string>()}")
            }
        };
        VerbosityOption.AcceptOnlyFromAmong("q", "quiet", "m", "minimal", "n", "normal", "d", "detailed", "diag", "diagnostic");
    }

    public static Option<Verbosity> VerbosityOption { get; }
}
