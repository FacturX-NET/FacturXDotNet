using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics;
using FacturXDotNet.Models;
using FacturXDotNet.Validation;
using Humanizer;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace FacturXDotNet.CLI.Validate;

class ValidateCommand() : CommandBase<ValidateCommandOptions>(
    "validate",
    "Validates the content of a Factur-X PDF.",
    [PathArgument],
    [
        CiiAttachmentOption,
        TreatWarningsAsErrorsOption,
        ProfileOption,
        RulesToSkipOption
    ]
)
{
    const string DefaultCiiAttachment = "factur-x.xml";

    static readonly Argument<FileInfo> PathArgument;
    static readonly Option<string> CiiAttachmentOption;
    static readonly Option<bool> TreatWarningsAsErrorsOption;
    static readonly Option<FacturXProfile> ProfileOption;
    static readonly Option<IEnumerable<string>> RulesToSkipOption;

    static ValidateCommand()
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
        CiiAttachmentOption = new Option<string>("--cii-attachment")
        {
            Description = "The name of the CII attachment.",
            HelpName = "name",
            DefaultValueFactory = _ => DefaultCiiAttachment
        };
        TreatWarningsAsErrorsOption = new Option<bool>("--warnings-as-errors")
        {
            Description = "Treat warnings as errors."
        };
        ProfileOption = new Option<FacturXProfile>("--profile", "-p")
        {
            Description = "The profile to use for validation. If set, the profile will override the one specified in the Factur-X file."
        };
        RulesToSkipOption = new Option<IEnumerable<string>>("--skip-rule", "-s")
        {
            Description = "The business rules that should be skipped. Example: --skip-rule \"BR-DE-1\" --skip-rule \"BR-DE-2\"",
            Arity = ArgumentArity.ZeroOrMore
        };
    }

    protected override ValidateCommandOptions ParseOptions(CommandResult result) =>
        new()
        {
            Path = result.GetValue(PathArgument)!,
            CiiAttachment = result.GetValue(CiiAttachmentOption),
            TreatWarningsAsErrors = result.GetValue(TreatWarningsAsErrorsOption),
            Profile = result.GetValue(ProfileOption),
            RulesToSkip = result.GetValue(RulesToSkipOption)?.ToList() ?? [],
            Verbosity = result.GetValue(GlobalOptions.VerbosityOption)
        };

    protected override async Task<int> RunImplAsync(ValidateCommandOptions options, CancellationToken cancellationToken = default)
    {
        if (options.Verbosity >= Verbosity.Minimal)
        {
            ShowOptions(options);
            AnsiConsole.WriteLine();
        }

        FacturXDocument facturX = await LoadFacturXDocument(options, cancellationToken);
        FacturXValidationReport report = await ValidateFacturXDocument(facturX, options, cancellationToken);

        if (options.Verbosity >= Verbosity.Normal)
        {
            ShowReportBreakdown(report);
        }

        if (options.Verbosity >= Verbosity.Minimal)
        {
            ShowFinalResult(report);
        }

        return report.Success ? 0 : 1;
    }

    static async Task<FacturXDocument> LoadFacturXDocument(ValidateCommandOptions options, CancellationToken cancellationToken)
    {
        FacturXDocument? facturX = null;

        if (options.Verbosity >= Verbosity.Minimal)
        {
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Default)
                .StartAsync(
                    "Loading...",
                    async _ =>
                    {
                        Stopwatch sw = new();
                        sw.Start();

                        await using FileStream stream = options.Path.OpenRead();
                        facturX = await FacturXDocument.FromStream(stream, cancellationToken);

                        sw.Stop();

                        if (options.Verbosity >= Verbosity.Normal)
                        {
                            AnsiConsole.MarkupLine($":check_mark: The document has been loaded in {sw.Elapsed.Humanize()}.");
                        }
                    }
                );
        }
        else
        {
            await using FileStream stream = options.Path.OpenRead();
            facturX = await FacturXDocument.FromStream(stream, cancellationToken);
        }

        return facturX!;
    }

    static async Task<FacturXValidationReport> ValidateFacturXDocument(FacturXDocument facturX, ValidateCommandOptions options, CancellationToken cancellationToken)
    {


        FacturXValidationOptions validationOptions = new()
        {
            TreatWarningsAsErrors = options.TreatWarningsAsErrors,
            ProfileOverride = options.Profile
        };
        validationOptions.RulesToSkip.AddRange(options.RulesToSkip);

        FacturXValidationReport report = default;
        if (options.Verbosity >= Verbosity.Detailed)
        {
            Progress<FacturXValidationProgressArgs> progress = new(
                args =>
                {
                    if (args.LastResult.HasValue)
                    {
                        if (args.LastResult.Value.HasFailed)
                        {
                            AnsiConsole.MarkupLineInterpolated($"[grey]:cross_mark: {args.LastResult.Value.Rule.Format()}[/]");
                        }
                        else
                        {
                            AnsiConsole.MarkupLineInterpolated($"[grey]:check_mark: {args.LastResult.Value.Rule.Format()}[/]");
                        }
                    }
                }
            );

            Stopwatch sw = new();
            sw.Start();

            report = await new FacturXValidator(validationOptions).ValidateAsync(facturX, options.CiiAttachment, progress: progress, cancellationToken: cancellationToken);

            sw.Stop();

            // wait a bit for progress to be printed to the console
            await Task.Delay(100, cancellationToken);

            AnsiConsole.MarkupLine($":check_mark: The document has been checked in {sw.Elapsed.Humanize()}.");
        }
        else if (options.Verbosity >= Verbosity.Minimal)
        {
            await AnsiConsole.Progress()
                .AutoClear(true)
                .StartAsync(
                    async ctx =>
                    {
                        ProgressTask progressBar = ctx.AddTask("Validation");

                        Progress<FacturXValidationProgressArgs> progress = new(
                            args =>
                            {
                                progressBar.MaxValue = args.Rules.Count;
                                progressBar.Value = args.Results.Count;

                                if (args.LastResult.HasValue)
                                {
                                    if (args.LastResult.Value.HasFailed)
                                    {
                                        progressBar.Description = $"[red]:cross_mark:[/] {args.LastResult.Value.Rule.Name.EscapeMarkup()}";
                                    }
                                    else
                                    {
                                        progressBar.Description = $"[green]:check_mark:[/] {args.LastResult.Value.Rule.Name.EscapeMarkup()}";
                                    }
                                }
                            }
                        );

                        Stopwatch sw = new();
                        sw.Start();

                        report = await new FacturXValidator(validationOptions).ValidateAsync(
                            facturX,
                            options.CiiAttachment,
                            progress: progress,
                            cancellationToken: cancellationToken
                        );

                        sw.Stop();

                        AnsiConsole.MarkupLine($":check_mark: The document has been checked in {sw.Elapsed.Humanize()}.");
                    }
                );
        }
        else
        {
            report = await new FacturXValidator(validationOptions).ValidateAsync(facturX, options.CiiAttachment, cancellationToken: cancellationToken);
        }

        return report;
    }

    static void ShowOptions(ValidateCommandOptions options)
    {
        Grid optionsGrid = new();
        optionsGrid.AddColumn();
        optionsGrid.AddColumn();

        optionsGrid.AddRow("[bold]Document[/]", options.Path.FullName);

        if (!string.IsNullOrWhiteSpace(options.CiiAttachment) && options.CiiAttachment != DefaultCiiAttachment)
        {
            optionsGrid.AddRow("[bold]CII attachment name[/]", options.CiiAttachment);
        }

        if (options.Profile.HasValue && options.Profile != FacturXProfile.None)
        {
            optionsGrid.AddRow("[bold]Profile[/]", options.Profile.Value.ToString());
        }

        if (options.RulesToSkip.Count != 0)
        {
            optionsGrid.AddRow("[bold]Rules to skip[/]", string.Join(", ", options.RulesToSkip));
        }

        if (options.Verbosity != Verbosity.Normal)
        {
            optionsGrid.AddRow("[bold]Verbosity[/]", options.Verbosity.ToString());
        }

        IRenderable panelContent;
        if (options.TreatWarningsAsErrors)
        {
            panelContent = new Rows(optionsGrid, new Markup("[darkorange]:warning: Warnings will be treated as errors.[/]"));
        }
        else
        {
            panelContent = optionsGrid;
        }

        AnsiConsole.Write(new Panel(panelContent).Header("Options").Border(BoxBorder.Rounded));
    }

    static void ShowReportBreakdown(FacturXValidationReport report)
    {
        BreakdownChart breakdown = new BreakdownChart().Width(120)
            .AddItem("Passed", report.Rules.Count(r => r.Status == BusinessRuleValidationStatus.Passed), Color.LightGreen)
            .AddItem(
                "Expected to fail, but passed",
                report.Rules.Count(r => r.ExpectedStatus is BusinessRuleExpectedValidationStatus.Failure && r.Status is BusinessRuleValidationStatus.Passed),
                Color.Green
            )
            .AddItem("Failed", report.Rules.Count(r => r.HasFailed), Color.Red)
            .AddItem(
                "Expected to fail, and failed",
                report.Rules.Count(r => r.ExpectedStatus is BusinessRuleExpectedValidationStatus.Failure && r.Status is BusinessRuleValidationStatus.Failed),
                Color.Grey78
            );

        int skipped = report.Rules.Count(r => r.Status is BusinessRuleValidationStatus.Skipped);
        if (skipped > 0)
        {
            breakdown.AddItem("Skipped", skipped, Color.Grey);
        }

        AnsiConsole.WriteLine();
        AnsiConsole.Write(breakdown);
        AnsiConsole.WriteLine();
    }

    static void ShowFinalResult(FacturXValidationReport report)
    {
        FacturXProfile documentProfile = report.ExpectedProfile;
        FacturXProfile detectedProfile = report.ValidProfiles.GetMaxProfile();
        if (report.Success)
        {
            AnsiConsole.MarkupLine("[green]:check_mark: The document is valid.[/]");
            AnsiConsole.MarkupLine($"[green]:check_mark: Document profile: [bold]{documentProfile}[/].[/]");

            if (documentProfile != detectedProfile)
            {
                AnsiConsole.MarkupLine($"[green]:check_mark: Detected profile: [bold]{detectedProfile}[/].[/]");
            }
        }
        else
        {
            AnsiConsole.MarkupLine("[red]:cross_mark: The Factur-X docuemnt is invalid.[/]");
            AnsiConsole.MarkupLine($"[red]:cross_mark: Document profile: {documentProfile}.[/]");

            if (documentProfile != detectedProfile)
            {
                AnsiConsole.MarkupLine($"[red]:cross_mark: Detected profile: {detectedProfile}.[/]");
            }
        }
    }
}

public class ValidateCommandOptions
{
    /// <summary>
    ///     The path to the Factur-X PDF.
    /// </summary>
    public FileInfo Path { get; set; } = null!;

    /// <summary>
    ///     The name of the CII attachment.
    /// </summary>
    public string? CiiAttachment { get; set; }

    /// <summary>
    ///     True if warnings should be treated as errors.
    /// </summary>
    public bool TreatWarningsAsErrors { get; set; }

    /// <summary>
    ///     The profile to use for validation. If set, the profile will override the one specified in the Factur-X file.
    /// </summary>
    public FacturXProfile? Profile { get; set; }

    /// <summary>
    ///     The business rules that should be skipped.
    /// </summary>
    public List<string> RulesToSkip { get; set; } = [];

    /// <summary>
    ///     The verbosity level of the output.
    /// </summary>
    public Verbosity Verbosity { get; set; }
}
