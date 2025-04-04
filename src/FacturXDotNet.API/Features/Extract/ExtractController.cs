using FacturXDotNet.Models.CII;
using FacturXDotNet.Parsing.CII;

namespace FacturXDotNet.API.Features.Extract;

static class ExtractController
{
    public static RouteGroupBuilder MapExtractEndpoints(this RouteGroupBuilder routes)
    {
        routes.MapPost(
                "/cii",
                async (IFormFile xml) =>
                {
                    await using Stream xmlStream = xml.OpenReadStream();
                    CrossIndustryInvoiceReader ciiReader = new();
                    return ciiReader.Read(xmlStream);
                }
            )
            .WithSummary("Extract Cross-Industry Invoice")
            .WithDescription("Extract Cross-Industry Invoice data from an XML file.")
            .Produces<CrossIndustryInvoice>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();

        return routes;
    }
}
