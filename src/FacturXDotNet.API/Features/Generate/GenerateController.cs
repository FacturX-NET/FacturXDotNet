using CommunityToolkit.HighPerformance;
using FacturXDotNet.Generation.FacturX;

namespace FacturXDotNet.API.Features.Generate;

static class GenerateController
{
    public static void MapGenerateEndpoints(this IEndpointRouteBuilder app) =>
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
}
