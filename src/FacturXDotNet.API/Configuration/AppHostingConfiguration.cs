namespace FacturXDotNet.API.Configuration;

class AppHostingConfiguration
{
    public string? Host { get; set; }
    public string? BasePath { get; set; }
    public bool UnsafeEnvironment { get; set; }
}
