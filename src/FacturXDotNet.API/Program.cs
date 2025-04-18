using System.Text.Json.Serialization;
using FacturXDotNet.API;
using FacturXDotNet.API.Configuration;
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

    builder.Services.Configure<AppConfiguration>(builder.Configuration);
    builder.Services.AddCors(
        opt => opt.AddDefaultPolicy(p => p.DisallowCredentials().AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("Content-Disposition"))
    );
    builder.Services.ConfigureHttpJsonOptions(options => { options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

    string host = builder.Configuration.GetSection("Hosting").GetValue<string>("Host") ?? "http://localhost";
    string basePath = builder.Configuration.GetSection("Hosting").GetValue<string>("BasePath") ?? "";
    string serverUrl = (host.EndsWith('/') ? host[..^1] : host)
                       + (basePath == ""
                           ? ""
                           : basePath.StartsWith('/')
                               ? basePath
                               : $"/{basePath}");

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
                        { Name = "Ismail Bennani", Email = "facturx.net@gmail.com", Url = new Uri("https://github.com/FacturX-NET/FacturXDotNet/issues") };

                    doc.Servers.Clear();
                    doc.Servers.Add(new OpenApiServer { Url = serverUrl });

                    return Task.CompletedTask;
                }
            );
        }
    );
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddTransient<PackagesService>();

    WebApplication app = builder.Build();

    IOptions<AppConfiguration> configuration = app.Services.GetRequiredService<IOptions<AppConfiguration>>();
    if (!string.IsNullOrWhiteSpace(configuration.Value.Hosting.BasePath))
    {
        app.UsePathBase(configuration.Value.Hosting.BasePath);
    }

    app.UseCors();

    app.MapOpenApi();
    app.MapScalarApiReference();

    app.MapGet("/", () => Results.LocalRedirect($"{configuration.Value.Hosting.BasePath}/scalar")).ExcludeFromDescription();
    app.MapHealthChecks("/health");
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
