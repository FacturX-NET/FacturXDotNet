using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace FacturXDotNet.Extensions;

static class PdfDocumentExtensions
{
    internal static PdfDocument OpenPdfDocumentAsync(this Stream data, PdfDocumentOpenMode mode, string? password = null)
    {
        PdfDocument document;

        if (password is not null)
        {
            document = PdfReader.Open(data, mode, args => args.Password = password);
        }
        else
        {
            document = PdfReader.Open(data, mode);
        }

        return document;
    }
}
