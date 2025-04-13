using FacturXDotNet.Models.CII;
using FacturXDotNet.Validation;
using FacturXDotNet.Validation.BusinessRules.CII;

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

                    if (report.Success)
                    {
                        return Results.Ok();
                    }

                    Dictionary<string, List<string>> result = new();

                    foreach (BusinessRuleValidationResult failure in report.Rules.Where(r => r.HasFailed))
                    {
                        if (failure.Rule is not CrossIndustryInvoiceBusinessRule ciiRule)
                        {
                            continue;
                        }

                        foreach (string fieldInvolved in ciiRule.FieldsInvolved)
                        {
                            if (!result.TryGetValue(fieldInvolved, out List<string>? failedRules))
                            {
                                failedRules = new List<string>();
                                result[fieldInvolved] = failedRules;
                            }

                            failedRules.Add(ciiRule.Name);
                        }
                    }

                    return Results.ValidationProblem(
                        result.Select(kv => new KeyValuePair<string, string[]>(kv.Key, kv.Value.ToArray())),
                        null,
                        null,
                        StatusCodes.Status400BadRequest
                    );
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
