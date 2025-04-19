namespace FacturXDotNet.Generation.FacturX;

/// <summary>
///     The action to take when an attachment with the same name already exists in the base document.
/// </summary>
public enum FacturXDocumentBuilderAttachmentConflictResolution
{
    /// <summary>
    ///     Overwrite the existing attachment with the new one.
    /// </summary>
    Overwrite,

    /// <summary>
    ///     Keep the existing attachment and do not add the new one.
    /// </summary>
    KeepOld,

    /// <summary>
    ///     Keep the existing attachment and add the new one with the same name.
    /// </summary>
    KeepBoth,

    /// <summary>
    ///     Throw an exception when an attachment with the same name already exists.
    /// </summary>
    Throw
}
