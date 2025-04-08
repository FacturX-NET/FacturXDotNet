namespace FacturXDotNet.API.Features.Information.Services;

class Package : IEquatable<Package>
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

    public bool Equals(Package? other)
    {
        if (other is null)
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        return PackageName == other.PackageName
               && PackageVersion == other.PackageVersion
               && PackageUrl == other.PackageUrl
               && Copyright == other.Copyright
               && Authors.SequenceEqual(other.Authors)
               && Description == other.Description
               && LicenseUrl == other.LicenseUrl
               && LicenseType == other.LicenseType
               && Repository == other.Repository;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }
        if (ReferenceEquals(this, obj))
        {
            return true;
        }
        if (obj.GetType() != GetType())
        {
            return false;
        }
        return Equals((Package)obj);
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(PackageName);
        hashCode.Add(PackageVersion);
        hashCode.Add(PackageUrl);
        hashCode.Add(Copyright);
        foreach (string author in Authors)
        {
            hashCode.Add(author);
        }
        hashCode.Add(Description);
        hashCode.Add(LicenseUrl);
        hashCode.Add(LicenseType);
        hashCode.Add(Repository.GetHashCode());
        return hashCode.ToHashCode();
    }

    public static bool operator ==(Package? left, Package? right) => Equals(left, right);

    public static bool operator !=(Package? left, Package? right) => !Equals(left, right);
}
