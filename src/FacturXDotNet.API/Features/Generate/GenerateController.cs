using System.Text.Json;
using System.Text.Json.Serialization;
using FacturXDotNet.Generation.CII;
using FacturXDotNet.Generation.FacturX;
using FacturXDotNet.Models.CII;
using Microsoft.AspNetCore.Mvc;

namespace FacturXDotNet.API.Features.Generate;

static class GenerateController
{
    static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web) { Converters = { new JsonStringEnumConverter() } };

    public static RouteGroupBuilder MapGenerateEndpoints(this RouteGroupBuilder routes)
    {
        routes.MapPost(
                "/facturx",
                async (
                    [FromForm] IFormFile pdf,
                    [FromForm] IFormFile cii,
                    [FromForm] IReadOnlyCollection<IFormFile>[] attachments,
                    CancellationToken cancellationToken = default
                ) =>
                {
                    FacturXDocumentBuilder builder = FacturXDocument.Create();

                    await using Stream ciiJsonStream = cii.OpenReadStream();
                    CrossIndustryInvoice? crossIndustryInvoice =
                        await JsonSerializer.DeserializeAsync<CrossIndustryInvoice>(ciiJsonStream, JsonSerializerOptions, cancellationToken);
                    if (crossIndustryInvoice == null)
                    {
                        return Results.BadRequest("Invalid Cross-Industry Invoice data.");
                    }

                    await using MemoryStream ciiStream = new();
                    CrossIndustryInvoiceWriter writer = new();
                    await writer.WriteAsync(ciiStream, crossIndustryInvoice);
                    ciiStream.Seek(0, SeekOrigin.Begin);
                    builder.WithCrossIndustryInvoice(ciiStream);

                    await using Stream pdfStream = pdf.OpenReadStream();
                    builder.WithBasePdf(pdfStream);

                    FacturXDocument facturX = await builder.BuildAsync();
                    MemoryStream facturXStream = new();
                    await facturX.ExportAsync(facturXStream);
                    facturXStream.Seek(0, SeekOrigin.Begin);

                    return Results.File(facturXStream, "application/pdf", pdf.FileName, DateTimeOffset.Now);
                }
            )
            .WithSummary("Generate FacturX Document")
            .WithDescription("Generate a FacturX Document from a base PDF and Cross-Industry Invoice data.")
            .Produces<IFormFile>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();

        routes.MapPost(
                "/cii",
                async (CrossIndustryInvoice crossIndustryInvoice, CancellationToken cancellationToken = default) =>
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
