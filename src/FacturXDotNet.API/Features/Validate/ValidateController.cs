using FacturXDotNet.Models.CII;
using FacturXDotNet.Validation;

namespace FacturXDotNet.API.Features.Validate;

static class ValidateController
{
    public static RouteGroupBuilder MapValidateEndpoints(this RouteGroupBuilder routes)
    {
        routes.MapPost(
                "/cii",
                (CrossIndustryInvoice cii) =>
                {
                    CrossIndustryInvoiceValidator validator = new();
                    FacturXValidationResult report = validator.GetValidationResult(cii);

                    if (!report.Success)
                    {
                        return Results.ValidationProblem(ValidationResults.BuildErrors(report), null, null, StatusCodes.Status400BadRequest);
                    }

                    return Results.Ok();
                }
            )
            .WithSummary("Validate Cross-Industry Invoice")
            .WithDescription("Validate the given.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();

        return routes;
    }
}
