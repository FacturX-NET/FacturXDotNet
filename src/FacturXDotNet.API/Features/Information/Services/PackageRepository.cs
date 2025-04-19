namespace FacturXDotNet.API.Features.Information.Services;

class PackageRepository : IEquatable<PackageRepository>
{
    public required string Type { get; init; }
    public required string Url { get; init; }
    public required string Commit { get; init; }

    public bool Equals(PackageRepository? other)
    {
        if (other is null)
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        return Type == other.Type && Url == other.Url && Commit == other.Commit;
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
        return Equals((PackageRepository)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Type, Url, Commit);

    public static bool operator ==(PackageRepository? left, PackageRepository? right) => Equals(left, right);

    public static bool operator !=(PackageRepository? left, PackageRepository? right) => !Equals(left, right);
}
