using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics;
using FacturXDotNet.CLI.Internals.Exceptions;
using FacturXDotNet.Models;
using FacturXDotNet.Validation;
using FacturXDotNet.Validation.BusinessRules;
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
        CiiAttachmentOption = new Option<string>("--cii-name")
        {
            Description = "The name of the CII attachment.",
            HelpName = "name",
            DefaultValueFactory = _ => DefaultCiiAttachment
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
            Description = "The business rules that should be skipped. Example: --skip-rule \"BR-1\" --skip-rule \"BR-2\"",
            Arity = ArgumentArity.ZeroOrMore
        };
    }

    protected override ValidateCommandOptions ParseOptions(CommandResult result) =>
        new()
        {
            Path = result.GetValue(PathArgument) ?? throw new RequiredArgumentMissingException(PathArgument),
            CiiAttachment = result.GetValue(CiiAttachmentOption),
            TreatWarningsAsErrors = result.GetValue(TreatWarningsAsErrorsOption),
            Profile = result.GetValue(ProfileOption),
            RulesToSkip = result.GetValue(RulesToSkipOption)?.ToList() ?? [],
            Verbosity = result.GetValue(GlobalOptions.VerbosityOption)
        };

    protected override async Task<int> RunImplAsync(ValidateCommandOptions options, CancellationToken cancellationToken = default)
    {
        ShowOptions(options);
        AnsiConsole.WriteLine();

        FacturXDocument facturX = await AnsiConsole.Status()
            .Spinner(Spinner.Known.Default)
            .StartAsync(
                "Parsing...",
                async _ =>
                {
                    Stopwatch sw = new();
                    sw.Start();

                    await using FileStream stream = options.Path.OpenRead();
                    FacturXDocument result = await FacturXDocument.LoadFromStream(stream, cancellationToken);

                    sw.Stop();

                    AnsiConsole.MarkupLine($":check_mark: The document has been parsed in {sw.Elapsed.Humanize()}.");
                    return result;
                }
            );

        FacturXValidationResult result = await ValidateAsync(facturX, options, cancellationToken);
        return result.Success ? 0 : 1;
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

    public static async Task<FacturXValidationResult> ValidateAsync(FacturXDocument document, ValidateCommandOptions options, CancellationToken cancellationToken)
    {
        FacturXValidationOptions validationOptions = new()
        {
            TreatWarningsAsErrors = options.TreatWarningsAsErrors,
            ProfileOverride = options.Profile
        };
        validationOptions.RulesToSkip.AddRange(options.RulesToSkip);

        FacturXValidationResult result;
        if (options.Verbosity >= Verbosity.Detailed)
        {
            validationOptions.CheckCallback = ruleResult =>
            {
                string icon = ruleResult.Status switch
                {
                    BusinessRuleValidationStatus.Passed => ":check_mark:",
                    BusinessRuleValidationStatus.Failed => ":cross_mark:",
                    BusinessRuleValidationStatus.Skipped => "•",
                    _ => throw new ArgumentOutOfRangeException()
                };

                string color = ruleResult.Status is BusinessRuleValidationStatus.Passed
                    ? "green"
                    : ruleResult.HasFailed
                        ? "red"
                        : "grey";

                AnsiConsole.MarkupLine($"[{color}]{icon}[/] {ruleResult.Rule.Format().EscapeMarkup()}");

                foreach (BusinessRuleDetail detail in ruleResult.Details)
                {
                    switch (detail.Severity)
                    {
                        case BusinessRuleDetailSeverity.Trace when options.Verbosity >= Verbosity.Diagnostic:
                            AnsiConsole.MarkupLine($"  \u21b3 [grey]{detail.Message.EscapeMarkup()}[/]");
                            break;
                        case BusinessRuleDetailSeverity.Information:
                            AnsiConsole.MarkupLine($"  \u21b3 {detail.Message.EscapeMarkup()}");
                            break;
                        case BusinessRuleDetailSeverity.Warning:
                            AnsiConsole.MarkupLine($"  \u21b3 [darkorange]:warning:[/] {detail.Message.EscapeMarkup()}");
                            break;
                        case BusinessRuleDetailSeverity.Fatal:
                            AnsiConsole.MarkupLine($"  \u21b3 [red]:cross_mark:[/] {detail.Message.EscapeMarkup()}");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            };

            Stopwatch sw = new();
            sw.Start();

            FacturXValidator validator = new(validationOptions);
            result = await validator.GetValidationResultAsync(document, options.CiiAttachment, cancellationToken: cancellationToken);

            sw.Stop();

            AnsiConsole.MarkupLine($":check_mark: The document has been checked in {sw.Elapsed.Humanize()}.");
            AnsiConsole.WriteLine();
        }
        else
        {
            result = default;
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Default)
                .StartAsync(
                    "Validating...",
                    async _ =>
                    {

                        Stopwatch sw = new();
                        sw.Start();

                        FacturXValidator validator = new(validationOptions);
                        result = await validator.GetValidationResultAsync(document, options.CiiAttachment, cancellationToken: cancellationToken);

                        sw.Stop();

                        AnsiConsole.MarkupLine($":check_mark: The document has been checked in {sw.Elapsed.Humanize()}.");
                        AnsiConsole.WriteLine();
                    }
                );
        }

        ShowFinalResult(document, result);

        return result;
    }

    static void ShowFinalResult(FacturXDocument _, FacturXValidationResult result)
    {
        FacturXProfile documentProfile = result.ExpectedProfile;
        FacturXProfile detectedProfile = result.ValidProfiles.GetMaxProfile();
        if (result.Success)
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
    ///     The verbosity level for the output.
    /// </summary>
    public Verbosity Verbosity { get; set; }
}
