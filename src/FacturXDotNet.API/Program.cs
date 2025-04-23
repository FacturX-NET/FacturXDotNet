using System.Text.Json.Serialization;
using FacturXDotNet.API;
using FacturXDotNet.API.Configuration;
using FacturXDotNet.API.Extensions;
using FacturXDotNet.API.Features.Extract;
using FacturXDotNet.API.Features.Generate;
using FacturXDotNet.API.Features.Information;
using FacturXDotNet.API.Features.Information.Services;
using FacturXDotNet.API.Features.Validate;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    builder.Services.AddSerilog();

    string serviceName = builder.Configuration.GetValue<string>("ServiceName", "default");
    Log.Information("Service named {ServiceName}", serviceName);

    builder.Services.Configure<AppConfiguration>(builder.Configuration);
    builder.Services.AddCors(
        opt => opt.AddDefaultPolicy(p => p.DisallowCredentials().AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("Content-Disposition"))
    );
    builder.Services.ConfigureHttpJsonOptions(options => { options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

    string host = builder.Configuration.GetSection("Hosting").GetValue<string>("Host") ?? "http://localhost";
    string basePath = builder.Configuration.GetSection("Hosting").GetValue<string>("BasePath") ?? "";
    Uri serverUrl = new(
        (host.EndsWith('/') ? host[..^1] : host)
        + (basePath == ""
            ? "/"
            : basePath.StartsWith('/')
                ? basePath.EndsWith('/') ? basePath : $"{basePath}/"
                : basePath.EndsWith('/')
                    ? $"/{basePath}"
                    : $"/{basePath}/")
    );
    Log.Information("Service hosted at {ServerUrl}", serverUrl);

    builder.Services.AddHealthChecks();
    builder.Services.AddOpenApi(
        opt =>
        {
            opt.AddDocumentTransformer(
                (doc, _, _) =>
                {
                    doc.Info.Title = "FacturX.NET";
                    doc.Info.Version = BuildInformation.Version;
                    doc.Info.Description = $"""
                                            FacturX.NET API - Work in progress. <br/>
                                            Built on {BuildInformation.BuildDate:D}
                                            """;
                    doc.Info.License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://github.com/FacturX-NET/FacturXDotNet/blob/main/LICENSE") };
                    doc.Info.Contact = new OpenApiContact
                        { Name = "Ismail Bennani", Email = "contact@facturxdotnet.org", Url = new Uri("https://github.com/FacturX-NET/FacturXDotNet/issues") };

                    doc.Servers.Clear();
                    doc.Servers.Add(new OpenApiServer { Url = serverUrl.ToString() });

                    return Task.CompletedTask;
                }
            );
        }
    );
    builder.Services.AddEndpointsApiExplorer();

    string? otlpEndpoint = builder.Configuration.GetSection("Observability")?.GetValue<string>("OtlpEndpoint");
    if (otlpEndpoint != null)
    {
        Uri otlpUri = new(otlpEndpoint);
        Log.Information("Service exports OpenTelemetry data to OTP at {Endpoint}", otlpUri);
        builder.AddObservability(serviceName, otlpUri);
    }

    builder.Services.AddTransient<PackagesService>();

    WebApplication app = builder.Build();

    IOptions<AppConfiguration> configuration = app.Services.GetRequiredService<IOptions<AppConfiguration>>();
    if (!string.IsNullOrWhiteSpace(configuration.Value.Hosting.BasePath))
    {
        app.UsePathBase(configuration.Value.Hosting.BasePath);
    }

    app.UseCors();

    app.MapHealthChecks("/health");
    Log.Information("Service health check at {HealthCheckUrl}", new Uri(serverUrl, "health"));

    app.MapOpenApi("/openapi/v1.json");
    Log.Information("Service serves OpenAPI specification at {OpenApiUrl}", new Uri(serverUrl, "openapi/v1.json"));

    Uri scalarUrl = new(serverUrl, "scalar");
    app.MapScalarApiReference();
    Log.Information("Service serves Scalar UI at {ScalarUrl}", scalarUrl);
    app.MapGet("/", () => Results.LocalRedirect($"{configuration.Value.Hosting.BasePath}/scalar")).ExcludeFromDescription();
    Log.Information("Service redirects / to Scalar UI at {ScalarUrl}", scalarUrl);

    app.MapGroup("/info").MapInformationEndpoints().WithTags("Information");
    app.MapGroup("/generate").MapGenerateEndpoints().WithTags("Generate");
    app.MapGroup("/extract").MapExtractEndpoints().WithTags("Extract");
    app.MapGroup("/validate").MapValidateEndpoints().WithTags("Validate");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
