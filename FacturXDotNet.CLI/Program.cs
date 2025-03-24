using System.CommandLine;
using System.Globalization;
using CommunityToolkit.HighPerformance;
using FacturXDotNet;
using FacturXDotNet.CLI;
using FacturXDotNet.Generation.XMP;
using Spectre.Console;

try
{
    const string inputPath = @"D:\source\repos\BenchmarkFacturX\Specification 1.0.07.2\5. FACTUR-X 1.07.2 - Examples\3.EN16931\tmp_out.pdf";
    const string outputPath = @"D:\source\repos\BenchmarkFacturX\Specification 1.0.07.2\5. FACTUR-X 1.07.2 - Examples\3.EN16931\tmp_outt.pdf";

    FacturXDocument facturXDocument = await FacturXDocument.LoadFromFileAsync(inputPath);

    CrossIndustryInvoiceAttachment? ciiAttachment = await facturXDocument.GetCrossIndustryInvoiceAttachmentAsync();
    ReadOnlyMemory<byte> ciiContent = await ciiAttachment!.ReadAsync();

    XmpMetadata? metadata = await facturXDocument.GetXmpMetadataAsync();
    metadata!.DublinCore!.Title = ["hoho!!"];
    metadata!.DublinCore!.Creator = ["hehe!!"];
    await using MemoryStream fakeStream = new();
    await new XmpMetadataWriter().WriteAsync(fakeStream, metadata);
    fakeStream.Seek(0, SeekOrigin.Begin);

    //await using Stream xmpMetadataStream = await facturXDocument.GetXmpMetadataStreamAsync();

    FacturXDocument result = await FacturXDocument.Create()
        .WithBasePdf(facturXDocument.Data.AsStream())
        .WithCrossIndustryInvoice(ciiContent.AsStream())
        .WithXmpMetadata(fakeStream)
        .BuildAsync();

    await using FileStream output = File.Open(outputPath, FileMode.Create);
    await result.ExportAsync(output);

    return 0;

    // force error messages to be in English
    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

    CommandLineConfiguration configuration = CommandLineConfigurationBuilder.Build();

#if DEBUG
    configuration.ThrowIfInvalid();
#endif

    return await configuration.InvokeAsync(args);
}
catch (Exception exn)
{
    AnsiConsole.WriteException(exn, ExceptionFormats.ShortenEverything);
    return 1;
}
finally
{
    await Console.Out.FlushAsync();
    await Console.Error.FlushAsync();
}
