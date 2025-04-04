using System.Reflection;
using FacturXDotNet.API.Attributes;
using FacturXDotNet.API.Features.Information.Models;

namespace FacturXDotNet.API.Features.Information;

static class InformationController
{
    public static RouteGroupBuilder MapInformationEndpoints(this RouteGroupBuilder routes)
    {
        routes.MapPost(
                "/build",
                () =>
                {
                    Assembly assembly = typeof(InformationController).Assembly;
                    string? version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? assembly.GetName().Version?.ToString();
                    DateTimeOffset buildDate = assembly.GetCustomAttribute<BuildDateAttribute>()?.BuildDate ?? DateTimeOffset.UtcNow;

                    return new BuildInformationDto
                    {
                        Version = version ?? "~dev",
                        BuildDate = buildDate.LocalDateTime
                    };
                }
            )
            .WithSummary("Build information")
            .WithDescription("Get information about the current build of the API.")
            .Produces<BuildInformationDto>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();

        return routes;
    }
}
