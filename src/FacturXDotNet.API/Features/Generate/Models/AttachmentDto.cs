namespace FacturXDotNet.API.Features.Generate.Models;

/// <summary>
///     An attachment of a FacturX document.
/// </summary>
public class AttachmentDto
{
    /// <summary>
    ///     The content of the attachment.
    /// </summary>
    public required IFormFile File { get; set; }

    /// <summary>
    ///     The description of the attachment.
    /// </summary>
    public string? Description { get; set; }
}
