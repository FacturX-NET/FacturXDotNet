namespace FacturXDotNet.Generation.FacturX;

/// <summary>
///     Represents the options for configuring an attachment during Factur-X document generation.
/// </summary>
public class AttachmentOptions
{
    /// <summary>
    ///     The approach to resolve naming conflicts for attachments when an attachment with the same name already exists in the base document.
    /// </summary>
    public FacturXDocumentBuilderAttachmentConflictResolution ConflictResolution { get; set; } = FacturXDocumentBuilderAttachmentConflictResolution.Overwrite;
}
