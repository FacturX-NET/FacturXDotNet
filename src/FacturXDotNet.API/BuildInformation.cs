using System.Reflection;
using FacturXDotNet.API.Attributes;

namespace FacturXDotNet.API;

static class BuildInformation
{
    static BuildInformation()
    {
        Assembly assembly = typeof(BuildInformation).Assembly;
        Version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? assembly.GetName().Version?.ToString() ?? "~dev";
        BuildDate = assembly.GetCustomAttribute<BuildDateAttribute>()?.BuildDate ?? DateTimeOffset.UtcNow;
    }

    public static string Version { get; }
    public static DateTimeOffset BuildDate { get; }
}
