namespace FacturXDotNet.API.Features.Information.Models;

/// <summary>
///     A package used by the API project.
/// </summary>
public class PackageDto
{
    /// <summary>
    ///     The name of the package.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     The author of the package.
    /// </summary>
    public required string Author { get; set; }

    /// <summary>
    ///     The version of the package.
    /// </summary>
    public required string Version { get; set; }

    /// <summary>
    ///     The license of the package.
    /// </summary>
    public required string License { get; set; }

    /// <summary>
    ///     The link to the package.
    /// </summary>
    public required string Link { get; set; }
}
