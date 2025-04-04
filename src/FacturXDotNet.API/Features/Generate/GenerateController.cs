using CommunityToolkit.HighPerformance;
using FacturXDotNet.Generation.FacturX;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.API.Features.Generate;

static class GenerateController
{
    public static void MapGenerateEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/generate",
                async (IFormFile pdf, IFormFile crossIndustryInvoice) =>
                {
                    FacturXDocumentBuilder builder = FacturXDocument.Create();

                    await using Stream pdfStream = pdf.OpenReadStream();
                    builder.WithBasePdf(pdfStream);

                    await using Stream ciiStream = crossIndustryInvoice.OpenReadStream();
                    builder.WithCrossIndustryInvoice(ciiStream);

                    FacturXDocument document = await builder.BuildAsync();

                    DateTimeOffset now = DateTimeOffset.Now;
                    string name = Path.GetFileNameWithoutExtension(pdf.FileName + "-facturx.pdf");
                    Stream dataStream = document.Data.AsStream();

                    return Results.File(dataStream, "application/pdf", name, now);
                }
            )
            .WithSummary("Generate from files")
            .WithDescription("Generate a FacturX document from a PDF file and a Cross-Industry Invoice XML file.")
            .Produces<IFormFile>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();

        app.MapPost("/generate/structured", async (GenerateFacturXRequest request) => { throw new NotImplementedException(); })
            .WithSummary("Generate from structured data")
            .WithDescription("Generate a FacturX document from a PDF file, the XMP metadata and the Cross-Industry Invoice data.")
            .Produces<IFormFile>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();
    }

    public class GenerateFacturXRequest
    {
        public string PdfFileBase64 { get; set; } = string.Empty;
        public XmpMetadata XmpMetadata { get; set; }
        public CrossIndustryInvoice CrossIndustryInvoice { get; set; }
    }
}
