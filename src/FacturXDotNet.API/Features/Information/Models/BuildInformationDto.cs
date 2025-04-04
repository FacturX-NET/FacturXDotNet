namespace FacturXDotNet.API.Features.Information.Models;

/// <summary>
///     The information about the current build.
/// </summary>
public class BuildInformationDto
{
    /// <summary>
    ///     The current version of the FacturX.NET API.
    /// </summary>
    public required string Version { get; set; }

    /// <summary>
    ///     The date when the current version was built.
    /// </summary>
    public required DateTime BuildDate { get; set; }
}
