using FacturXDotNet.API.Features.Information.Models;
using FacturXDotNet.API.Features.Information.Services;
using Microsoft.AspNetCore.Mvc;

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
                "/dependencies",
                async ([FromServices] PackagesService packagesService) =>
                {
                    IReadOnlyCollection<Package> packages = await packagesService.ReadPackagesAsync();
                    return packages.Select(
                        p => new PackageDto
                        {
                            Name = p.PackageName,
                            Author = string.Join(", ", p.Authors),
                            Version = p.PackageVersion,
                            License = p.LicenseType,
                            Link = p.Repository.Url
                        }
                    );
                }
            )
            .WithSummary("Dependencies")
            .WithDescription("Get information about the dependencies of the API application, especially about their licenses.")
            .Produces<IReadOnlyCollection<PackageDto>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();

        return routes;
    }
}
