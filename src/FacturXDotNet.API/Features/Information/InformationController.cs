using FacturXDotNet.API.Configuration;
using FacturXDotNet.API.Features.Information.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FacturXDotNet.API.Features.Information;

static class InformationController
{
    public static RouteGroupBuilder MapInformationEndpoints(this RouteGroupBuilder routes)
    {
        routes.MapGet(
                "/build",
                () => new BuildInformationDto
                {
                    Version = BuildInformation.Version,
                    BuildDate = BuildInformation.BuildDate.LocalDateTime
                }
            )
            .WithSummary("Build")
            .WithDescription("Get information about the current build of the API.")
            .Produces<BuildInformationDto>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();

        routes.MapGet(
                "/hosting",
                ([FromServices] IOptionsSnapshot<AppConfiguration> configuration) => new HostingInformationDto
                {
                    UnsafeEnvironment = configuration.Value.Hosting.UnsafeEnvironment
                }
            )
            .WithSummary("Hosting")
            .WithDescription("Get information about the current hosting environment of the API application.")
            .Produces<HostingInformationDto>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();

        routes.MapGet(
                "/sbom",
                () =>
                {
                    string path = Path.GetFullPath(Path.Join("Resources", "api.bom.json"));
                    if (!File.Exists(path))
                    {
                        return Results.InternalServerError("Could not find SBOM file.");
                    }

                    return Results.File(path, "application/json", "FacturXDotNet-API.sbom.json", enableRangeProcessing: true);
                }
            )
            .WithSummary("SBOM")
            .WithDescription("Get the JSON BOM of the API in the CycloneDX format.")
            .Produces<IFormFile>(StatusCodes.Status200OK, "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();

        return routes;
    }
}
