using System.Text.Json;

namespace FacturXDotNet.API.Features.Information.Services;

class PackagesService
{
    const string LicencesFileName = "licenses.json";

    public async Task<IReadOnlyCollection<Package>> ReadPackagesAsync(CancellationToken cancellationToken = default)
    {
        string path = Path.Join("Resources", LicencesFileName);
        if (!File.Exists(path))
        {
            throw new InvalidOperationException("Could not find licenses file.");
        }

        await using FileStream stream = File.OpenRead(path);
        IReadOnlyCollection<Package>? licenses = await JsonSerializer.DeserializeAsync<IReadOnlyCollection<Package>>(stream, cancellationToken: cancellationToken);
        if (licenses == null)
        {
            throw new InvalidOperationException("Could not read licenses file.");
        }

        return licenses.Distinct().ToArray();
    }
}
