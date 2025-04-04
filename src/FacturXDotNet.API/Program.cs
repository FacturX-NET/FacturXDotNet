using System.Text.Json.Serialization;
using FacturXDotNet.API;
using FacturXDotNet.API.Features.Extract;
using FacturXDotNet.API.Features.Generate;
using FacturXDotNet.API.Features.Information;
using FacturXDotNet.API.Features.Information.Services;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    builder.Services.AddSerilog();

    builder.Services.AddCors(opt => opt.AddDefaultPolicy(p => p.DisallowCredentials().AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
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
                    doc.Info.Summary = "Work with FacturX documents through a REST API.";
                    doc.Info.Description = $"""
                                            FacturX.NET API - Work in progress. <br/>
                                            Built on {BuildInformation.BuildDate:D}
                                            """;
                    doc.Info.License = new OpenApiLicense { Identifier = "MIT", Name = "MIT", Url = new Uri("https://github.com/FacturX-NET/FacturXDotNet/blob/main/LICENSE") };
                    doc.Info.Contact = new OpenApiContact
                        { Name = "Ismail Bennani", Email = "facturx.net@gmail.com", Url = new Uri("https://github.com/FacturX-NET/FacturXDotNet/issues") };

                    return Task.CompletedTask;
                }
            );
        }
    );

    builder.Services.AddTransient<PackagesService>();

    WebApplication app = builder.Build();

    app.UseCors();

    app.MapOpenApi();
    app.MapScalarApiReference();

    app.MapGet("/", () => Results.LocalRedirect("/scalar")).ExcludeFromDescription();
    app.MapHealthChecks("/health").WithTags("Health").WithOpenApi();
    app.MapGroup("/info").MapInformationEndpoints().WithTags("Information");
    app.MapGroup("/generate").MapGenerateEndpoints().WithTags("Generate");
    app.MapGroup("/extract").MapExtractEndpoints().WithTags("Extract");

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
