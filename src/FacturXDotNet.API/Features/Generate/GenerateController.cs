using System.Text.Json;
using System.Text.Json.Serialization;
using FacturXDotNet.API.Features.Generate.Models;
using FacturXDotNet.API.Features.Generate.Services;
using FacturXDotNet.Generation.CII;
using FacturXDotNet.Generation.FacturX;
using FacturXDotNet.Generation.PDF;
using FacturXDotNet.Generation.XMP;
using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using PdfSharp.Pdf;

namespace FacturXDotNet.API.Features.Generate;

static class GenerateController
{
    static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web) { Converters = { new JsonStringEnumConverter() } };

    public static RouteGroupBuilder MapGenerateEndpoints(this RouteGroupBuilder routes)
    {
        routes.MapPost("/facturx", PostFacturX)
            .WithSummary("Generate FacturX Document")
            .WithDescription("Generate a FacturX Document from a base PDF and Cross-Industry Invoice data.")
            .Produces<IFormFile>(StatusCodes.Status200OK, "application/pdf")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();

        routes.MapPost("/cii", PostCii)
            .WithSummary("Generate Cross-Industry Invoice")
            .WithDescription("Generate a Cross-Industry Invoice XML file from structured data.")
            .Produces<IFormFile>(StatusCodes.Status200OK, "text/xml")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();

        routes.MapGet("/pdf/models", GetPdfModels)
            .WithSummary("Get PDF models")
            .WithDescription("Get the available PDF models that can be used to generate a PDF image from Cross-Industry Invoice data.")
            .Produces<IReadOnlyCollection<string>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();


        routes.MapPost("/pdf/{model}", PostPdf)
            .WithSummary("Generate PDF")
            .WithDescription("Generate a PDF file from the Cross-Industry Invoice structured data.")
            .Produces<IFormFile>(StatusCodes.Status200OK, "application/pdf")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();

        return routes;
    }

    static async Task<IResult> PostFacturX(
        HttpContext httpContext,
        [FromForm] IFormFile? xmp,
        [FromForm] IFormFile pdf,
        [FromForm] IFormFile cii,
        [FromForm] AttachmentDto[]? attachments,
        CancellationToken cancellationToken
    )
    {
        FacturXDocumentBuilder builder = FacturXDocument.Create();

        if (xmp != null)
        {
            await using Stream xmpJsonStream = xmp.OpenReadStream();
            XmpMetadata? xmpMetadata = await JsonSerializer.DeserializeAsync<XmpMetadata>(xmpJsonStream, JsonSerializerOptions, cancellationToken);
            if (xmpMetadata == null)
            {
                return Results.BadRequest("Invalid XMP metadata.");
            }

            await using MemoryStream xmpStream = new();
            XmpMetadataWriter xmpWriter = new();
            await xmpWriter.WriteAsync(xmpStream, xmpMetadata);
            xmpStream.Seek(0, SeekOrigin.Begin);
            builder.PostProcess(opt => { opt.XmpMetadata(metadata => metadata.FillValues(xmpMetadata)); });
        }

        await using Stream ciiJsonStream = cii.OpenReadStream();
        CrossIndustryInvoice? crossIndustryInvoice = await JsonSerializer.DeserializeAsync<CrossIndustryInvoice>(ciiJsonStream, JsonSerializerOptions, cancellationToken);
        if (crossIndustryInvoice == null)
        {
            return Results.BadRequest("Invalid Cross-Industry Invoice data.");
        }

        await using MemoryStream ciiStream = new();
        CrossIndustryInvoiceWriter ciiWriter = new();
        await ciiWriter.WriteAsync(ciiStream, crossIndustryInvoice);
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

    static async Task<IResult> PostCii(CrossIndustryInvoice crossIndustryInvoice, CancellationToken cancellationToken = default)
    {
        MemoryStream stream = new();
        CrossIndustryInvoiceWriter writer = new();
        await writer.WriteAsync(stream, crossIndustryInvoice);
        stream.Seek(0, SeekOrigin.Begin);

        return Results.File(stream, "text/xml", "factur-x.xml", DateTimeOffset.Now);
    }

    static IReadOnlyCollection<string> GetPdfModels([FromServices] GeneratePdfImageService generatePdfImageService) => generatePdfImageService.GetAvailableModels();

    static async Task<IResult> PostPdf(
        string model,
        CrossIndustryInvoice cii,
        [FromServices] GeneratePdfImageService generatePdfImageService,
        CancellationToken cancellationToken = default
    )
    {
        IPdfGenerator? generator = generatePdfImageService.GetPdfGenerator(model);
        if (generator == null)
        {
            return Results.NotFound("Requested PDF model not found.");
        }

        using PdfDocument pdfDocument = generator.Build(cii);

        MemoryStream pdfStream = new();
        await pdfDocument.SaveAsync(pdfStream);
        pdfStream.Seek(0, SeekOrigin.Begin);

        string? invoiceNumber = cii.ExchangedDocument?.Id;
        string? sellerName = cii.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name;
        string? buyerName = cii.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name;

        return Results.File(pdfStream, "application/pdf", $"Invoice {invoiceNumber} - {sellerName} - {buyerName}.pdf", DateTimeOffset.Now);
    }
}
