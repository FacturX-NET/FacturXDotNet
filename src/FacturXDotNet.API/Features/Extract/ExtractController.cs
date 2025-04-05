using FacturXDotNet.Models.CII;
using FacturXDotNet.Parsing.CII;
using Microsoft.AspNetCore.Mvc;

namespace FacturXDotNet.API.Features.Extract;

static class ExtractController
{
    public static RouteGroupBuilder MapExtractEndpoints(this RouteGroupBuilder routes)
    {
        routes.MapPost(
                "/cii",
                async ([FromForm] IFormFile file, CancellationToken cancellationToken = default) =>
                {
                    string extension = Path.GetExtension(file.FileName);
                    if (extension == ".pdf")
                    {
                        await using Stream pdfStream = file.OpenReadStream();
                        FacturXDocument document = await FacturXDocument.LoadFromStream(pdfStream, cancellationToken);

                        CrossIndustryInvoiceAttachment? ciiAttachment = await document.GetCrossIndustryInvoiceAttachmentAsync(cancellationToken: cancellationToken);
                        if (ciiAttachment is null)
                        {
                            return Results.BadRequest("No Cross-Industry Invoice attachment found in the PDF.");
                        }

                        CrossIndustryInvoice crossIndustryInvoice = await ciiAttachment.GetCrossIndustryInvoiceAsync(cancellationToken: cancellationToken);
                        return Results.Ok(crossIndustryInvoice);
                    }

                    if (extension == ".xml")
                    {
                        await using Stream xmlStream = file.OpenReadStream();
                        CrossIndustryInvoiceReader ciiReader = new();
                        CrossIndustryInvoice crossIndustryInvoice = ciiReader.Read(xmlStream);
                        return Results.Ok(crossIndustryInvoice);
                    }

                    return Results.BadRequest($"Invalid file type: {extension}. Only PDF and XML files are supported.");
                }
            )
            .WithSummary("Extract Cross-Industry Invoice")
            .WithDescription("Extract Cross-Industry Invoice data from a FacturX PDF file or an XML file.")
            .Produces<CrossIndustryInvoice>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();

        routes.MapPost(
                "/attachments",
                async ([FromForm] IFormFile file, bool skipCrossIndustryInvoice, CancellationToken cancellationToken = default) =>
                {
                    await using Stream pdfStream = file.OpenReadStream();
                    FacturXDocument document = await FacturXDocument.LoadFromStream(pdfStream, cancellationToken);

                    var result = new List<IFormFile>();
                    
                    await foreach (var attachment in document.GetAttachmentsAsync(cancellationToken: cancellationToken))
                    {
                        result.Add(Results.Par);
                    }
                    
                    CrossIndustryInvoiceAttachment? ciiAttachment = await document.GetCrossIndustryInvoiceAttachmentAsync(cancellationToken: cancellationToken);
                    if (ciiAttachment is null)
                    {
                        return Results.BadRequest("No Cross-Industry Invoice attachment found in the PDF.");
                    }

                    CrossIndustryInvoice crossIndustryInvoice = await ciiAttachment.GetCrossIndustryInvoiceAsync(cancellationToken: cancellationToken);
                    return Results.Ok(crossIndustryInvoice);

                    return Results.BadRequest($"Invalid file type: {extension}. Only PDF and XML files are supported.");
                }
            )
            .WithSummary("Extract Cross-Industry Invoice")
            .WithDescription("Extract Cross-Industry Invoice data from a FacturX PDF file or an XML file.")
            .Produces<IFormFile[]>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();

        return routes;
    }
}
