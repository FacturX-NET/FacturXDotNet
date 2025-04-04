using FacturXDotNet.Generation.CII;
using FacturXDotNet.Models.CII;

namespace FacturXDotNet.API.Features.Generate;

static class GenerateController
{
    public static RouteGroupBuilder MapGenerateEndpoints(this RouteGroupBuilder routes)
    {
        routes.MapPost(
                "/cii",
                async (CrossIndustryInvoice crossIndustryInvoice) =>
                {
                    MemoryStream stream = new();
                    CrossIndustryInvoiceWriter writer = new();
                    await writer.WriteAsync(stream, crossIndustryInvoice);
                    stream.Seek(0, SeekOrigin.Begin);

                    return Results.File(stream, "text/xml", "factur-x.xml", DateTimeOffset.Now);
                }
            )
            .WithSummary("Generate Cross-Industry Invoice")
            .WithDescription("Generate a Cross-Industry Invoice XML file from structured data.")
            .Produces<IFormFile>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();

        return routes;
    }
}
