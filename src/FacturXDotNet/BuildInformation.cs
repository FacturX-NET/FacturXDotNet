using System.Reflection;
using Semver;

namespace FacturXDotNet;

static class BuildInformation
{
    static BuildInformation()
    {
        Assembly assembly = typeof(BuildInformation).Assembly;
        string versionString = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? assembly.GetName().Version?.ToString() ?? "~dev";
        Version = SemVersion.TryParse(versionString, out SemVersion? version) ? version : new SemVersion(0, 0, 0, [], ["dev"]);
    }

    public static SemVersion Version { get; }
}
