using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text.Json;

namespace FacturXDotNet.CLI.Extract;

class ExtractCommand() : CommandBase<ExtractCommandOption>("extract", "Extracts the content of a Factur-X PDF.", [PathArgument], [CiiOption, XmpOption])
{
    static readonly Argument<FileInfo> PathArgument;
    static readonly Option<string> CiiOption;
    static readonly Option<string> XmpOption;

    static ExtractCommand()
    {
        PathArgument = new Argument<FileInfo>("path") { Description = "The path to the Factur-X PDF." };
        CiiOption = new Option<string>("--cii")
        {
            Description = "Extracts the content of the CII XML. Optionally specify a path, otherwise the CII XML will be saved next to the PDF with the same name.",
            Arity = ArgumentArity.ZeroOrOne
        };
        XmpOption = new Option<string>("--xmp")
        {
            Description = "Extracts the content of the XMP metadata. Optionally specify a path, otherwise the XMP metadata will be saved next to the PDF with the same name.",
            Arity = ArgumentArity.ZeroOrOne
        };
    }

    public override async Task RunAsync(ExtractCommandOption opt, CancellationToken cancellationToken = default) =>
        Console.Write(
            JsonSerializer.Serialize(
                new
                {
                    Path = opt.Path.FullName,
                    opt.ExportCii,
                    opt.CiiPath,
                    opt.ExportXmp,
                    opt.XmpPath
                },
                new JsonSerializerOptions(JsonSerializerOptions.Web) { WriteIndented = true }
            )
        );

    protected override void Validate(CommandResult option)
    {
        bool exportCii = option.GetResult(CiiOption) is not null;
        bool exportXmp = option.GetResult(XmpOption) is not null;

        if (!exportCii && !exportXmp)
        {
            option.AddError("At least one of --cii or --xmp must be specified.");
        }
    }
}

public class ExtractCommandOption
{
    public FileInfo Path { get; set; } = null!;
    public bool ExportCii { get; set; }
    public string? CiiPath { get; set; }
    public bool ExportXmp { get; set; }
    public string? XmpPath { get; set; }
}
