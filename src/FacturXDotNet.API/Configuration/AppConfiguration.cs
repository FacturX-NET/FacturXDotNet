namespace FacturXDotNet.API.Configuration;

class AppConfiguration
{
    public string? InstanceName { get; set; } = "default";
    public AppHostingConfiguration Hosting { get; set; } = new();
    public AppObservabilityConfiguration Observability { get; set; } = new();
}
