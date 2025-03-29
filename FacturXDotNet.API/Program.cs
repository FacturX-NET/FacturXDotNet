using FacturXDotNet.API.Features.Generate;
using Scalar.AspNetCore;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    builder.Services.AddSerilog();

    builder.Services.AddCors(opt => opt.AddDefaultPolicy(p => p.DisallowCredentials().AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

    builder.Services.AddOpenApi();

    WebApplication app = builder.Build();

    app.UseCors();

    app.MapScalarApiReference();
    app.MapOpenApi();

    app.MapGet("/", () => Results.LocalRedirect("/scalar")).ExcludeFromDescription();
    app.MapGenerateEndpoints();

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
