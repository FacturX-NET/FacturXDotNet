using CommandLine;

namespace FacturXDotNet.CLI.Extract;

[Verb("extract", HelpText = "Extracts the content of a Factur-X PDF.")]
class ExtractOption
{
    const string ExtractionTargetGroupName = "Extraction target";

    [Value(0, MetaName = "PATH", Required = true, HelpText = "The path to the Factur-X PDF.")]
    public string Path { get; set; } = string.Empty;

    [Option(
        "cii",
        Group = ExtractionTargetGroupName,
        HelpText = "Extracts the content of the CII XML. Optionally specify a path, otherwise the CII XML will be saved next to the PDF with the same name."
    )]
    public string? CiiPath { get; set; }

    [Option(
        "xmp",
        Group = ExtractionTargetGroupName,
        HelpText = "Extracts the content of the XMP metadata. Optionally specify a path, otherwise the XMP metadata will be saved next to the PDF with the same name."
    )]
    public string? XmpPath { get; set; }
}
