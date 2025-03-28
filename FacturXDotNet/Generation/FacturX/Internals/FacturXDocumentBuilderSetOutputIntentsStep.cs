using FacturXDotNet.Generation.PDF.Internals;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;

namespace FacturXDotNet.Generation.FacturX.Internals;

/// <remarks>
///     See section 14.11.5 of ISO 32000-2:2020 (PDF 2.0)
///     https://pdfa.org/resource/iso-32000-2/
/// </remarks>
static class FacturXDocumentBuilderSetOutputIntentsStep
{
    const string PdfAOutputIntentSubtype = "/GTS_PDFA1";
    const string PdfAOutputIntentOutputCondition = "sRGB";
    const string PdfAOutputIntentOutputConditionIdentifier = "sRGB IEC61966-2.1";
    const string PdfAOutputIntentRegistryName = "http://www.color.org";

    static ReadOnlyMemory<byte>? _sRgbIccProfileCached;

    public static async Task RunAsync(PdfDocument document, FacturXDocumentBuildArgs args)
    {
        if (args.OverwriteOutputIntents)
        {
            RemoveOutputIntentsIfExists(document);
        }

        PdfArray? outputIntents = document.Internals.Catalog.Elements.GetArray("/OutputIntents");
        if (outputIntents is null)
        {
            outputIntents = new PdfArray();
            document.Internals.Catalog.Elements["/OutputIntents"] = outputIntents;
        }

        PdfDictionary? outputIntent = outputIntents.Elements.OfType<PdfReference>()
            .Select(r => r.Value)
            .OfType<PdfDictionary>()
            .FirstOrDefault(i => i.Elements.GetName("/S") == PdfAOutputIntentSubtype);

        if (outputIntent is null)
        {
            outputIntent = new PdfDictionary();
            outputIntent.Elements.Add("/Type", new PdfName("/OutputIntent"));
            outputIntent.Elements.Add("/S", new PdfName(PdfAOutputIntentSubtype));
            outputIntent.Elements.Add("/OutputCondition", new PdfString(PdfAOutputIntentOutputCondition));
            outputIntent.Elements.Add("/OutputConditionIdentifier", new PdfString(PdfAOutputIntentOutputConditionIdentifier));
            outputIntent.Elements.Add("/RegistryName", new PdfString(PdfAOutputIntentRegistryName));
            document.Internals.AddObject(outputIntent);
            outputIntents.Elements.Add(outputIntent.ReferenceNotNull);

            PdfDictionary rgbProfile = new();
            document.Internals.AddObject(rgbProfile);
            outputIntent.Elements.Add("/DestOutputProfile", rgbProfile.ReferenceNotNull);

            rgbProfile.Elements.Add("/N", new PdfInteger(3));
            await AddIccProfileStreamAsync(rgbProfile);
        }
    }

    static void RemoveOutputIntentsIfExists(PdfDocument document)
    {
        PdfArray? outputIntents = document.Internals.Catalog.Elements.GetArray("/OutputIntents");
        if (outputIntents == null)
        {
            return;
        }

        foreach (PdfReference outputIntent in outputIntents.OfType<PdfReference>())
        {
            PdfDictionary? destOutputProfile = outputIntent.Value is PdfDictionary dict ? dict.Elements.GetDictionary("/DestOutputProfile") : null;
            if (destOutputProfile is not null)
            {
                document.Internals.RemoveObject(destOutputProfile);
            }

            document.Internals.RemoveObject(outputIntent.Value);
        }

        document.Internals.Catalog.Elements.Remove("/OutputIntents");
    }

    static async Task AddIccProfileStreamAsync(PdfDictionary rgbProfile)
    {
        if (_sRgbIccProfileCached == null)
        {
            Stream? iccProfileStream = typeof(FacturXDocumentBuilderSetOutputIntentsStep).Assembly.GetManifestResourceStream("FacturXDotNet.Resources.sRGB2014.icc");
            if (iccProfileStream is null)
            {
                throw new InvalidOperationException("Could not find sRGB ICC profile to use.");
            }

            await using Stream _ = iccProfileStream;

            byte[] content = new byte[(int)iccProfileStream.Length];
            await iccProfileStream.ReadExactlyAsync(content);

            _sRgbIccProfileCached = content;
        }

        rgbProfile.WriteFlateEncodedData(_sRgbIccProfileCached.Value.Span);
    }
}
