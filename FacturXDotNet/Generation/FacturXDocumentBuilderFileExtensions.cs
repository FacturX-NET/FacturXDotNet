namespace FacturXDotNet.Generation;

/// <summary>
///     Extension methods related to using files with the <see cref="FacturXDocumentBuilder" />.
/// </summary>
public static class FacturXDocumentBuilderFileExtensions
{
    /// <summary>
    ///     Reads the base PDF file from the specified path.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="path">The path to the PDF document.</param>
    /// <param name="password">The password to open the PDF document.</param>
    /// <returns>The builder itself, for chaining.</returns>
    public static FacturXDocumentBuilder WithBasePdfFile(this FacturXDocumentBuilder builder, string path, string? password = null) =>
        builder.WithBasePdf(File.OpenRead(path), password, false);

    /// <summary>
    ///     Reads the XMP metadata file from the specified path.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="path">The path to the XMP metadata file.</param>
    /// <returns>The builder itself, for chaining.</returns>
    public static FacturXDocumentBuilder WithXmpMetadataFile(this FacturXDocumentBuilder builder, string path) => builder.WithXmpMetadata(File.OpenRead(path), false);

    /// <summary>
    ///     Reads the Cross-Industry Invoice file from the specified path.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="path">The path to the Cross-Industry Invoice XML file.</param>
    /// <param name="ciiAttachmentName">The name of the attachment containing the Cross-Industry Invoice XML file. If not specified, the default name 'factur-x.xml' will be used.</param>
    /// <returns>The builder itself, for chaining.</returns>
    public static FacturXDocumentBuilder WithCrossIndustryInvoiceFile(this FacturXDocumentBuilder builder, string path, string? ciiAttachmentName = null) =>
        builder.WithCrossIndustryInvoice(File.OpenRead(path), ciiAttachmentName, false);
}
