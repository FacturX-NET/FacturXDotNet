using System.Text.Json;
using System.Text.Json.Serialization;
using FacturXDotNet.Generation.CII;
using FacturXDotNet.Generation.FacturX;
using FacturXDotNet.Generation.PDF;
using FacturXDotNet.Models.CII;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace FacturXDotNet.API.Features.Generate;

static class GenerateController
{
    static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web) { Converters = { new JsonStringEnumConverter() } };

    public static RouteGroupBuilder MapGenerateEndpoints(this RouteGroupBuilder routes)
    {
        routes.MapPost(
                "/facturx",
                async (
                    HttpContext httpContext,
                    [FromForm] IFormFile pdf,
                    [FromForm] IFormFile cii,
                    [FromForm] AttachmentDto[] attachments,
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

                    int i = 0;
                    while (true)
                    {
                        IFormFile? attachment = httpContext.Request.Form.Files.GetFile($"attachments[{i}].file");
                        if (attachment == null)
                        {
                            break;
                        }

                        await using Stream attachmentStream = attachment.OpenReadStream();
                        byte[] content = new byte[attachmentStream.Length];
                        await attachmentStream.ReadExactlyAsync(content, cancellationToken);
                        string? description = httpContext.Request.Form.TryGetValue($"attachments[{i}].description", out StringValues value) ? value.ToString() : null;

                        builder.WithAttachment(
                            new PdfAttachmentData(attachment.FileName, content)
                            {
                                Description = description,
                                Relationship = AfRelationship.Data,
                                MimeType = attachment.ContentType
                            }
                        );

                        i++;
                    }

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

/// <summary>
///     An attachment of a FacturX document.
/// </summary>
public class AttachmentDto
{
    /// <summary>
    ///     The content of the attachment.
    /// </summary>
    public required IFormFile File { get; set; }

    /// <summary>
    ///     The description of the attachment.
    /// </summary>
    public string? Description { get; set; }
}
