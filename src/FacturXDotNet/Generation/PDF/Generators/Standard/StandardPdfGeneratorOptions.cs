namespace FacturXDotNet.Generation.PDF.Generators.Standard;

/// <summary>
///     Provides configuration options for generating standard PDFs.
/// </summary>
public class StandardPdfGeneratorOptions
{
    /// <summary>
    ///     The logo to be displayed in the generated PDF document as a byte array.
    /// </summary>
    public ReadOnlyMemory<byte>? Logo { get; set; }

    /// <summary>
    ///     The language pack that contains localized resources for generating the standard PDF.
    /// </summary>
    public StandardPdfGeneratorLanguagePack LanguagePack { get; set; } = StandardPdfGeneratorLanguagePack.English;
}
