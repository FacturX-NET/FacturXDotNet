using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics;
using FacturXDotNet.CLI.Internals.Exceptions;
using Humanizer;
using Spectre.Console;

namespace FacturXDotNet.CLI.Extract;

class ExtractCommand() : CommandBase<ExtractCommandOptions>("extract", "Extracts the content of a Factur-X PDF.", [PathArgument], [CiiOption, CiiAttachmentOption, XmpOption])
{
    const string DefaultCiiAttachment = "factur-x.xml";

    static readonly Argument<FileInfo> PathArgument;
    static readonly Option<string> CiiOption;
    static readonly Option<string> CiiAttachmentOption;
    static readonly Option<string> XmpOption;

    static ExtractCommand()
    {
        PathArgument = new Argument<FileInfo>("path")
        {
            Description = "The path to the Factur-X PDF.",
            Validators =
            {
                result =>
                {
                    FileInfo path = result.GetValueOrDefault<FileInfo>();
                    if (!path.Exists)
                    {
                        result.AddError($"Could not find file '{path.FullName}'.");
                    }
                }
            }
        };
        CiiOption = new Option<string>("--cii")
        {
            Description = "Extracts the content of the CII XML. Optionally specify a path, otherwise the CII XML will be saved next to the PDF with the same name.",
            HelpName = "path",
            Arity = ArgumentArity.ZeroOrOne
        };
        CiiAttachmentOption = new Option<string>("--cii-attachment")
        {
            Description = "The name of the CII attachment.",
            HelpName = "name",
            DefaultValueFactory = _ => DefaultCiiAttachment
        };
        XmpOption = new Option<string>("--xmp")
        {
            Description = "Extracts the content of the XMP metadata. Optionally specify a path, otherwise the XMP metadata will be saved next to the PDF with the same name.",
            HelpName = "path",
            Arity = ArgumentArity.ZeroOrOne
        };
    }

    protected override ExtractCommandOptions ParseOptions(CommandResult result) =>
        new()
        {
            Path = result.GetValue(PathArgument) ?? throw new RequiredArgumentMissingException(PathArgument),
            Cii = result.GetResult(CiiOption) is not null ? result.GetValue(CiiOption) ?? "" : null,
            CiiAttachment = result.GetValue(CiiAttachmentOption),
            Xmp = result.GetResult(XmpOption) is not null ? result.GetValue(XmpOption) ?? "" : null
        };

    protected override void ValidateOptions(CommandResult result, ExtractCommandOptions options)
    {
        if (options is { ExportCii: false, ExportXmp: false })
        {
            result.AddError("At least one of --cii or --xmp must be specified.");
        }
    }

    protected override async Task<int> RunImplAsync(ExtractCommandOptions options, CancellationToken cancellationToken = default)
    {
        ShowOptions(options);
        AnsiConsole.WriteLine();

        if (options.ExportCii)
        {
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Default)
                .StartAsync(
                    "Exporting CII XML...",
                    async _ =>
                    {
                        Stopwatch sw = new();
                        sw.Start();

                        string outputPath = string.IsNullOrWhiteSpace(options.Cii) ? Path.ChangeExtension(options.Path.FullName, ".xml") : options.Cii;
                        await ExtractCii(options.Path, options.CiiAttachment, outputPath, cancellationToken);

                        sw.Stop();

                        AnsiConsole.MarkupLine($"[green]:check_mark:[/] Extracted CII XML to '[bold]{outputPath}[/]' in {sw.Elapsed.Humanize()}.");
                    }
                );
        }

        if (options.ExportXmp)
        {
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Default)
                .StartAsync(
                    "Exporting XMP metadata...",
                    async _ =>
                    {
                        Stopwatch sw = new();
                        sw.Start();

                        string outputPath = string.IsNullOrWhiteSpace(options.Xmp) ? Path.ChangeExtension(options.Path.FullName, ".xmp") : options.Xmp;
                        await ExtractXmp(options.Path, outputPath, cancellationToken);

                        sw.Stop();

                        AnsiConsole.MarkupLine($"[green]:check_mark:[/] Extracted XMP metadata to '[bold]{outputPath}[/]' in {sw.Elapsed.Humanize()}.");
                    }
                );
        }

        return 0;
    }

    static async Task ExtractCii(FileInfo pdfPath, string? ciiAttachmentName, string outputPath, CancellationToken cancellationToken)
    {
        await using FileStream stream = pdfPath.OpenRead();
        FacturXDocument document = await FacturXDocument.LoadFromStream(stream, cancellationToken);

        CrossIndustryInvoiceAttachment? ciiAttachment = await document.GetCrossIndustryInvoiceAttachmentAsync(ciiAttachmentName, cancellationToken: cancellationToken);
        if (ciiAttachment is null)
        {
            return;
        }

        await using FileStream xmpFile = File.Open(outputPath, FileMode.Create);
        await ciiAttachment.CopyToAsync(xmpFile, cancellationToken: cancellationToken);
    }

    static async Task ExtractXmp(FileInfo pdfPath, string outputPath, CancellationToken cancellationToken)
    {
        await using FileStream stream = pdfPath.OpenRead();
        FacturXDocument document = await FacturXDocument.LoadFromStream(stream, cancellationToken);

        await using Stream xmpStream = await document.GetXmpMetadataStreamAsync(cancellationToken: cancellationToken);

        await using FileStream xmpFile = File.Open(outputPath, FileMode.Create);
        await xmpStream.CopyToAsync(xmpFile, cancellationToken);
    }

    static void ShowOptions(ExtractCommandOptions options)
    {
        Grid optionsGrid = new();
        optionsGrid.AddColumn();
        optionsGrid.AddColumn();

        optionsGrid.AddRow("[bold]Document[/]", options.Path.FullName);

        if (options.ExportCii)
        {
            if (!string.IsNullOrWhiteSpace(options.CiiAttachment) && options.CiiAttachment != DefaultCiiAttachment)
            {
                optionsGrid.AddRow("[bold]CII attachment name[/]", options.CiiAttachment);
            }

            optionsGrid.AddRow("[bold]Export CII[/]", "True");
        }

        AnsiConsole.Write(new Panel(optionsGrid).Header("Options").Border(BoxBorder.Rounded));
    }
}

public class ExtractCommandOptions
{
    /// <summary>
    ///     The path to the Factur-X PDF.
    /// </summary>
    public FileInfo Path { get; set; } = null!;

    /// <summary>
    ///     True if the CII XML should be extracted.
    /// </summary>
    public bool ExportCii => Cii is not null;

    /// <summary>
    ///     The path to extract the CII XML to. If empty, the CII XML will be saved next to the PDF with the same name. If null, the CII XML will not be extracted.
    /// </summary>
    public string? Cii { get; set; }

    /// <summary>
    ///     The name of the CII attachment.
    /// </summary>
    public string? CiiAttachment { get; set; }

    /// <summary>
    ///     True if the XMP metadata should be extracted.
    /// </summary>
    public bool ExportXmp => Xmp is not null;

    /// <summary>
    ///     The path to extract the XMP metadata to. If empty, the XMP metadata will be saved next to the PDF with the same name. If null, the XMP metadata will not be extracted.
    /// </summary>
    public string? Xmp { get; set; }
}
