using System.Security.Cryptography;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Filters;

namespace FacturXDotNet.Generation.PDF.Internals;

static class PdfDictionaryStreamExtensions
{
    public static void WriteFlateEncodedData(this PdfDictionary dict, ReadOnlySpan<byte> data, Action<PdfDictionary>? configureParams = null)
    {
        FlateDecode flateDecode = new();
        byte[] encoded = flateDecode.Encode(data.ToArray(), PdfFlateEncodeMode.BestCompression);

        dict.Elements.Add("/Filter", new PdfName("/FlateDecode"));

        PdfDictionary pdfStreamParams = new();
        pdfStreamParams.Elements.Add("/CheckSum", new PdfString(ComputeChecksum(data)));
        pdfStreamParams.Elements.Add("/Size", new PdfInteger(data.Length));
        configureParams?.Invoke(pdfStreamParams);
        dict.Elements.Add("/Params", pdfStreamParams);

        dict.CreateStream(encoded.ToArray());
    }

    static string ComputeChecksum(ReadOnlySpan<byte> data)
    {
        byte[] contentHash = MD5.HashData(data);
        return BitConverter.ToString(contentHash).Replace("-", "").ToLowerInvariant();
    }
}
