namespace FacturXDotNet.API.Features.Information.Models;

/// <summary>
///     Information about the current hosting environment of the API application.
/// </summary>
public class HostingInformationDto
{
    /// <summary>
    ///     Indicates whether the application is running in an unsafe environment.
    /// </summary>
    public bool UnsafeEnvironment { get; set; }
}
