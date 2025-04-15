using System.Text.Json.Serialization;
using FacturXDotNet.API;
using FacturXDotNet.API.Configuration;
using FacturXDotNet.API.Features.Extract;
using FacturXDotNet.API.Features.Generate;
using FacturXDotNet.API.Features.Generate.Services;
using FacturXDotNet.API.Features.Information;
using FacturXDotNet.API.Features.Information.Services;
using FacturXDotNet.API.Features.Validate;
using FacturXDotNet.Generation.PDF.Generators.Standard;
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

                    return Task.CompletedTask;
                }
            );
        }
    );
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSingleton<GeneratePdfImageService>();
    builder.Services.AddTransient<PackagesService>();

    WebApplication app = builder.Build();

    GeneratePdfImageService generatePdfImageService = app.Services.GetRequiredService<GeneratePdfImageService>();
    generatePdfImageService.RegisterPdfGenerator("standard", new StandardPdfGenerator());

    app.UseCors();

    app.MapOpenApi();
    app.MapScalarApiReference();

    app.MapGet("/", () => Results.LocalRedirect("/scalar")).ExcludeFromDescription();
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
