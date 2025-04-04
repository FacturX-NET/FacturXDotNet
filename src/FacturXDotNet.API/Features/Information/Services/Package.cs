namespace FacturXDotNet.API.Features.Information.Services;

class Package
{
    public required string PackageName { get; init; }
    public required string PackageVersion { get; init; }
    public required string PackageUrl { get; init; }
    public required string Copyright { get; init; }
    public required IReadOnlyCollection<string> Authors { get; init; }
    public required string Description { get; init; }
    public required string LicenseUrl { get; init; }
    public required string LicenseType { get; init; }
    public required PackageRepository Repository { get; init; }
}
