using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace FacturXDotNet.API.Extensions;

static class ObservabilityHostingExtensions
{
    public static void AddObservability(this WebApplicationBuilder builder, string serviceName, Uri otlpEndpoint)
    {
        builder.Logging.AddOpenTelemetry(
            logging =>
            {
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
            }
        );

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithMetrics(
                metrics =>
                {
                    // Metrics provider from OpenTelemetry
                    metrics.AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        // Metrics provides by ASP.NET Core in .NET 8
                        .AddMeter("Microsoft.AspNetCore.Hosting")
                        .AddMeter("Microsoft.AspNetCore.Server.Kestrel");
                }
            )
            .WithTracing(tracing => { tracing.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation(); })
            .UseOtlpExporter(OtlpExportProtocol.Grpc, otlpEndpoint);
    }
}
