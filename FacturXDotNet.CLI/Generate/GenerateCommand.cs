using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics;
using FacturXDotNet.CLI.Internals;
using FacturXDotNet.CLI.Internals.Exceptions;
using FacturXDotNet.CLI.Validate;
using FacturXDotNet.Generation;
using FacturXDotNet.Generation.PDF;
using FacturXDotNet.Models;
using FacturXDotNet.Validation;
using Humanizer;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace FacturXDotNet.CLI.Generate;

class GenerateCommand() : CommandBase<GenerateCommandOptions>(
    "generate",
    "Generates a Factur-X PDF.",
    [],
    [
        BasePdfOption,
        CiiOption,
        CiiAttachmentNameOption,
        AttachmentsOption,
        OutputPathOption,
        SkipValidationOption,
        TreatWarningsAsErrorsOption,
        ProfileOption,
        RulesToSkipOption
    ]
)
{
    const string DefaultCiiAttachment = "factur-x.xml";

    static readonly Option<FileInfo> BasePdfOption;
    static readonly Option<FileInfo> CiiOption;
    static readonly Option<string> CiiAttachmentNameOption;
    static readonly Option<IEnumerable<FileInfo>> AttachmentsOption;
    static readonly Option<FileInfo> OutputPathOption;
    static readonly Option<bool> SkipValidationOption;
    static readonly Option<bool> TreatWarningsAsErrorsOption;
    static readonly Option<FacturXProfile> ProfileOption;
    static readonly Option<IEnumerable<string>> RulesToSkipOption;

    static GenerateCommand()
    {
        BasePdfOption = new Option<FileInfo>("--pdf")
        {
            Required = true,
            Description = "The path to the PDF that will be used as base.",
            HelpName = "path",
            Validators = { Validators.FileExists }
        };
        CiiOption = new Option<FileInfo>("--cii")
        {
            Required = true,
            Description = "The path to the CII file to use as structured data.",
            HelpName = "path",
            Validators = { Validators.FileExists }
        };
        CiiAttachmentNameOption = new Option<string>("--cii-name")
        {
            Description = "The name of the CII attachment in the result.",
            HelpName = "name",
            DefaultValueFactory = _ => DefaultCiiAttachment
        };
        AttachmentsOption = new Option<IEnumerable<FileInfo>>("--attach")
        {
            Description = "Additional files to attach to the result.",
            HelpName = "path",
            Validators = { Validators.FileExists }
        };
        OutputPathOption = new Option<FileInfo>("--output-path", "-o")
        {
            Description = "The path to the output file.",
            HelpName = "path"
        };
        SkipValidationOption = new Option<bool>("--skip-validation")
        {
            Description = "Do not validate the generated Factur-X PDF.",
            DefaultValueFactory = _ => false
        };
        TreatWarningsAsErrorsOption = new Option<bool>("--warnings-as-errors")
        {
            Description = "Treat warnings as errors.",
            DefaultValueFactory = _ => false
        };
        ProfileOption = new Option<FacturXProfile>("--profile", "-p")
        {
            Description = "The profile to use for validation. If set, the profile will override the one specified in the Factur-X file."
        };
        RulesToSkipOption = new Option<IEnumerable<string>>("--skip-rule", "-s")
        {
            Description = "The business rules that should be skipped. Example: --skip-rule \"BR-01\" --skip-rule \"BR-02\"",
            Arity = ArgumentArity.ZeroOrMore
        };
    }

    protected override GenerateCommandOptions ParseOptions(CommandResult result)
    {
        FileInfo basePdf = result.GetValue(BasePdfOption) ?? throw new RequiredOptionMissingException(BasePdfOption);
        return new GenerateCommandOptions
        {
            BasePdf = basePdf,
            Cii = result.GetValue(CiiOption) ?? throw new RequiredOptionMissingException(CiiOption),
            CiiAttachmentName = result.GetValue(CiiAttachmentNameOption),
            Attachments = result.GetValue(AttachmentsOption) ?? [],
            OutputPath = result.GetValue(OutputPathOption)?.FullName,
            SkipValidation = result.GetResult(SkipValidationOption) is not null && result.GetValue(SkipValidationOption),
            TreatWarningsAsErrors = result.GetValue(TreatWarningsAsErrorsOption),
            Profile = result.GetValue(ProfileOption),
            RulesToSkip = result.GetValue(RulesToSkipOption)?.ToList() ?? []
        };
    }

    protected override async Task<int> RunImplAsync(GenerateCommandOptions options, CancellationToken cancellationToken = default)
    {
        ShowOptions(options);
        AnsiConsole.WriteLine();

        string ciiAttachmentName = options.CiiAttachmentName ?? DefaultCiiAttachment;

        FacturXDocumentBuilder? builder = null;
        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Default)
            .StartAsync(
                "Reading input files...",
                async ctx =>
                {
                    Stopwatch sw = new();
                    sw.Start();

                    builder = FacturXDocument.Create();

                    ctx.Status($"Reading input files ({options.BasePdf.FullName})...");

                    builder.WithBasePdfFile(options.BasePdf.FullName);

                    ctx.Status($"Reading input files ({options.Cii.FullName})...");

                    builder.WithCrossIndustryInvoiceFile(options.Cii.FullName, ciiAttachmentName);

                    foreach (FileInfo attachment in options.Attachments)
                    {
                        ctx.Status($"Reading input files ({attachment.FullName})...");

                        await using FileStream attachmentStream = attachment.OpenRead();
                        byte[] attachmentContent = new byte[attachmentStream.Length];
                        await attachmentStream.ReadExactlyAsync(attachmentContent, cancellationToken);

                        builder.WithAttachment(new PdfAttachmentData(attachment.Name, attachmentContent));
                    }

                    sw.Stop();

                    AnsiConsole.MarkupLine($":check_mark: The input files have been read in {sw.Elapsed.Humanize()}.");
                }
            );

        if (builder is null)
        {
            throw new Exception("This will never happen.");
        }

        FacturXDocument? document = null;
        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Default)
            .StartAsync(
                "Generating the Factur-X document...",
                async _ =>
                {
                    Stopwatch sw = new();
                    sw.Start();

                    document = await builder.BuildAsync();

                    sw.Stop();

                    AnsiConsole.MarkupLine($":check_mark: The document has been generated in {sw.Elapsed.Humanize()}.");
                }
            );

        if (document is null)
        {
            throw new Exception("This will never happen.");
        }

        bool generate;
        if (options.SkipValidation)
        {
            generate = true;
        }
        else
        {
            FacturXValidationResult result = await ValidateCommand.ValidateAsync(
                document,
                new ValidateCommandOptions
                {
                    CiiAttachment = ciiAttachmentName,
                    TreatWarningsAsErrors = options.TreatWarningsAsErrors,
                    Profile = options.Profile,
                    RulesToSkip = options.RulesToSkip
                },
                cancellationToken
            );

            AnsiConsole.WriteLine();

            generate = result.Success;
        }

        if (!generate)
        {
            AnsiConsole.MarkupLine("[red]:cross_mark:[/] The document has not been exported.");
            return 1;
        }

        string outputPath = options.OutputPath ?? ComputeDefaultOutputFileName(options.BasePdf);

        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Default)
            .StartAsync(
                "Exporting the Factur-X document...",
                async _ =>
                {
                    Stopwatch sw = new();
                    sw.Start();

                    await using FileStream outputFile = File.Open(outputPath, FileMode.Create);
                    await document.ExportAsync(outputFile);

                    sw.Stop();

                    AnsiConsole.MarkupLine($":check_mark: The document has been exported to [bold]{outputPath}[/] in {sw.Elapsed.Humanize()}.");
                }
            );

        return 0;
    }

    static void ShowOptions(GenerateCommandOptions options)
    {
        Grid optionsGrid = new();
        IRenderable panelContent = optionsGrid;
        optionsGrid.AddColumn();
        optionsGrid.AddColumn();

        optionsGrid.AddRow("[bold]Basd PDF[/]", options.BasePdf.FullName);
        optionsGrid.AddRow("[bold]CII XML[/]", options.Cii.FullName);

        if (!options.SkipValidation)
        {
            optionsGrid.AddRow("[bold]Validate result[/]", "True");

            if (!string.IsNullOrWhiteSpace(options.CiiAttachmentName) && options.CiiAttachmentName != DefaultCiiAttachment)
            {
                optionsGrid.AddRow("[bold]CII attachment name[/]", options.CiiAttachmentName);
            }

            if (options.Profile.HasValue && options.Profile != FacturXProfile.None)
            {
                optionsGrid.AddRow("[bold]Profile[/]", options.Profile.Value.ToString());
            }

            if (options.RulesToSkip.Count != 0)
            {
                optionsGrid.AddRow("[bold]Rules to skip[/]", string.Join(", ", options.RulesToSkip));
            }

            if (options.TreatWarningsAsErrors)
            {
                panelContent = new Rows(optionsGrid, new Markup("[darkorange]:warning: Warnings will be treated as errors.[/]"));
            }
        }

        AnsiConsole.Write(new Panel(panelContent).Header("Options").Border(BoxBorder.Rounded));
    }

    static string ComputeDefaultOutputFileName(FileInfo basePdf)
    {
        string? inputDirectory = Path.GetDirectoryName(basePdf.FullName);
        string inputName = Path.GetFileNameWithoutExtension(basePdf.FullName);
        return Path.Join(inputDirectory, $"{inputName}-facturx.pdf");
    }
}

public class GenerateCommandOptions
{
    /// <summary>
    ///     The file to use as base PDF.
    /// </summary>
    public FileInfo BasePdf { get; set; } = null!;

    /// <summary>
    ///     The file to use as CII XML.
    /// </summary>
    public FileInfo Cii { get; set; } = null!;

    /// <summary>
    ///     The name of the CII attachment.
    /// </summary>
    public string? CiiAttachmentName { get; set; }

    /// <summary>
    ///     Additional files to attach to the result.
    /// </summary>
    public IEnumerable<FileInfo> Attachments { get; set; } = null!;

    /// <summary>
    ///     The path to the output file.
    /// </summary>
    public string? OutputPath { get; set; }

    /// <summary>
    ///     Whether to validate the final result or not.
    /// </summary>
    public bool SkipValidation { get; set; }

    /// <summary>
    ///     Whether to treat warnings as errors during the validation of the final result.
    /// </summary>
    public bool TreatWarningsAsErrors { get; set; }

    /// <summary>
    ///     The profile to use for the validation of the final result. If set, the profile will override the one specified in the Factur-X file.
    /// </summary>
    public FacturXProfile? Profile { get; set; }

    /// <summary>
    ///     The business rules that should be skipped during the validation of the final result.
    /// </summary>
    public List<string> RulesToSkip { get; set; } = [];
}
