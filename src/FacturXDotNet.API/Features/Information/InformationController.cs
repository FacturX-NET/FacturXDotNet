using FacturXDotNet.API.Configuration;
using FacturXDotNet.API.Features.Information.Models;
using FacturXDotNet.API.Features.Information.Services;
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
                "/dependencies",
                async ([FromServices] PackagesService packagesService, CancellationToken cancellationToken = default) =>
                {
                    IReadOnlyCollection<Package> packages = await packagesService.ReadPackagesAsync(cancellationToken);

                    List<PackageDto> result = packages.Select(
                            p => new PackageDto
                            {
                                Name = p.PackageName,
                                Author = string.Join(", ", p.Authors),
                                Version = p.PackageVersion,
                                License = p.LicenseType,
                                Link = p.Repository.Url
                            }
                        )
                        .ToList();

                    // add the dotnet-project-licenses, which is the tool that is used to extract the licenses file used above
                    result.Add(
                        new PackageDto
                        {
                            Name = "dotnet-project-licenses",
                            Author = "Tom Chavakis,  Lexy2,  senslen",
                            Version = "2.7.1",
                            License = "Apache-2.0",
                            Link = "https://github.com/tomchavakis/nuget-license"
                        }
                    );

                    return result;
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
