using FacturXDotNet.Models.CII;
using FacturXDotNet.Models.XMP;
using FacturXDotNet.Parsing.CII;
using FacturXDotNet.Parsing.XMP;
using Microsoft.AspNetCore.Mvc;

namespace FacturXDotNet.API.Features.Extract;

static class ExtractController
{
    public static RouteGroupBuilder MapExtractEndpoints(this RouteGroupBuilder routes)
    {
        routes.MapPost(
                "/xmp",
                async ([FromForm] IFormFile file, CancellationToken cancellationToken = default) =>
                {
                    string extension = Path.GetExtension(file.FileName);
                    if (extension == ".pdf")
                    {
                        await using Stream pdfStream = file.OpenReadStream();
                        FacturXDocument document = await FacturXDocument.LoadFromStream(pdfStream, cancellationToken);

                        XmpMetadata? xmpMetadata = await document.GetXmpMetadataAsync(cancellationToken: cancellationToken);
                        if (xmpMetadata is null)
                        {
                            return Results.BadRequest("No XMP metadata found in the PDF.");
                        }

                        return Results.Ok(xmpMetadata);
                    }

                    if (extension == ".xml")
                    {
                        await using Stream xmpStream = file.OpenReadStream();
                        XmpMetadataReader xmpReader = new();
                        XmpMetadata xmpMetadata = xmpReader.Read(xmpStream);
                        return Results.Ok(xmpMetadata);
                    }

                    return Results.BadRequest($"Invalid file type: {extension}. Only PDF and XML files are supported.");
                }
            )
            .WithSummary("Extract XMP metadata")
            .WithDescription("Extract XMP metadata from a FacturX PDF file or an XML file.")
            .Produces<XmpMetadata>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();

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

        return routes;
    }
}
