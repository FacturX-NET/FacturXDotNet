namespace FacturXDotNet.CLI.Extract;

static class ExtractCommand
{
    public static async Task RunAsync(ExtractOption options)
    {
        Console.WriteLine($"Extracting {options.Path}...");
        Console.WriteLine($"CII {options.CiiPath ?? "(null)"}...");
        Console.WriteLine($"XMP {options.XmpPath ?? "(null)"}...");
    }
}
