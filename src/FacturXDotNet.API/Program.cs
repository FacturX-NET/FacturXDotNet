using FacturXDotNet.API.Features.Extract;
using FacturXDotNet.API.Features.Generate;
using FacturXDotNet.API.Features.Information;
using Scalar.AspNetCore;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    builder.Services.AddSerilog();

    builder.Services.AddCors(opt => opt.AddDefaultPolicy(p => p.DisallowCredentials().AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
    builder.Services.AddHealthChecks();
    builder.Services.AddOpenApi();

    WebApplication app = builder.Build();

    app.UseCors();

    app.MapScalarApiReference();
    app.MapOpenApi();

    app.MapGet("/", () => Results.LocalRedirect("/scalar")).ExcludeFromDescription();
    app.MapHealthChecks("/health").WithTags("Health");
    app.MapGroup("/").MapInformationEndpoints().WithTags("Information");
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
