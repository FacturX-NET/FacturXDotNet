using FacturXDotNet.Models.CII;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace FacturXDotNet.Generation.PDF.Internals.Generators;

/// <summary>
///     Internal generator used to provide an existing stream as the PDF to use as the PDF of a FacturX document.
/// </summary>
/// <param name="stream">
///     The base PDF stream to use as the starting point for generating the FacturX document.
/// </param>
/// <param name="password">
///     The password required to access the PDF, if applicable. Pass null if no password is needed.
/// </param>
/// <param name="leaveOpen">
///     Indicates whether the <paramref name="stream" /> stream should remain open after use.
/// </param>
class PdfFromFileGenerator(Stream stream, string? password = null, bool leaveOpen = true) : IPdfGenerator
{
    public PdfDocument Build(CrossIndustryInvoice _)
    {
        PdfDocument document = password is not null
            ? PdfReader.Open(stream, PdfDocumentOpenMode.Modify, args => args.Password = password)
            : PdfReader.Open(stream, PdfDocumentOpenMode.Modify);

        if (!leaveOpen)
        {
            stream.Close();
        }

        return document;
    }
}
