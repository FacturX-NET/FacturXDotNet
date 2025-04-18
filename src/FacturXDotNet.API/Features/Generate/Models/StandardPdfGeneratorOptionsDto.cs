using FacturXDotNet.Generation.PDF.Generators.Standard;

namespace FacturXDotNet.API.Features.Generate.Models;

/// <summary>
///     Represents the options used to configure the generation of a standard PDF file.
///     Used in conjunction with other components to produce a customizable Factur-X document.
/// </summary>
public class StandardPdfGeneratorOptionsDto
{
    /// <summary>
    ///     The logo to be displayed in the generated PDF document as a byte array.
    /// </summary>
    public byte[]? Logo { get; set; }

    /// <summary>
    ///     The footer text to be displayed in the generated PDF document.
    /// </summary>
    public string? Footer { get; set; }

    /// <summary>
    ///     The language pack that contains localized resources for generating the standard PDF.
    /// </summary>
    public StandardPdfGeneratorLanguagePackDto? LanguagePack { get; set; }
}

static class StandardPdfGeneratorOptionsMappingExtensions
{
    public static StandardPdfGeneratorOptions ToStandardPdfGeneratorOptions(this StandardPdfGeneratorOptionsDto options) =>
        new()
        {
            Logo = options.Logo, Footer = options.Footer, LanguagePack = options.LanguagePack?.ToStandardPdfGeneratorLanguagePack() ?? StandardPdfGeneratorLanguagePack.Default
        };
}
