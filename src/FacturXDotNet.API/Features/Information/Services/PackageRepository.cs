namespace FacturXDotNet.API.Features.Information.Services;

class PackageRepository
{
    public required string Type { get; init; }
    public required string Url { get; init; }
    public required string Commit { get; init; }
}
