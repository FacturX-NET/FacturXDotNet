using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Generation.XMP;

/// <summary>
///     A class that contains all the XMP metadata for a Factur-X document.
/// </summary>
public class FullyDefinedXmpMetadata
{
    readonly XmpMetadata _metadata;

    internal FullyDefinedXmpMetadata(XmpMetadata metadata)
    {
        _metadata = metadata;
    }

    /// <inheritdoc cref="XmpMetadata.PdfAIdentification" />
    public XmpPdfAIdentificationMetadata PdfAIdentification => _metadata.PdfAIdentification ??= new XmpPdfAIdentificationMetadata();

    /// <inheritdoc cref="XmpMetadata.Basic" />
    public XmpBasicMetadata Basic => _metadata.Basic ??= new XmpBasicMetadata();

    /// <inheritdoc cref="XmpMetadata.Pdf" />
    public XmpPdfMetadata Pdf => _metadata.Pdf ??= new XmpPdfMetadata();

    /// <inheritdoc cref="XmpMetadata.DublinCore" />
    public XmpDublinCoreMetadata DublinCore => _metadata.DublinCore ??= new XmpDublinCoreMetadata();

    /// <inheritdoc cref="XmpMetadata.PdfAExtensions" />
    public XmpPdfAExtensionsMetadata PdfAExtensions => _metadata.PdfAExtensions ??= new XmpPdfAExtensionsMetadata();

    /// <inheritdoc cref="XmpMetadata.FacturX" />
    public XmpFacturXMetadata FacturX => _metadata.FacturX ??= new XmpFacturXMetadata();

    /// <summary>
    ///     Replace the values of this instance with the values from the given XMP metadata.
    ///     The null values in the input are ignored.
    /// </summary>
    /// <remarks>Some values of the input are always ignored, such as the version of the PDF document or the Factur-X conformance level.</remarks>
    /// <param name="xmpMetadata">The XMP metadata to fill the values from.</param>
    public void FillValues(XmpMetadata xmpMetadata)
    {
        Basic.Identifier = xmpMetadata.Basic?.Identifier ?? Basic.Identifier;
        Basic.Label = xmpMetadata.Basic?.Label ?? Basic.Label;
        Basic.Rating = xmpMetadata.Basic?.Rating ?? Basic.Rating;
        Basic.BaseUrl = xmpMetadata.Basic?.BaseUrl ?? Basic.BaseUrl;
        Basic.Nickname = xmpMetadata.Basic?.Nickname ?? Basic.Nickname;
        Basic.Thumbnails = xmpMetadata.Basic?.Thumbnails ?? Basic.Thumbnails;

        Pdf.Keywords = xmpMetadata.Pdf?.Keywords ?? Pdf.Keywords;

        DublinCore.Contributor = xmpMetadata.DublinCore?.Contributor ?? DublinCore.Contributor;
        DublinCore.Coverage = xmpMetadata.DublinCore?.Coverage ?? DublinCore.Coverage;
        DublinCore.Creator = xmpMetadata.DublinCore?.Creator ?? DublinCore.Creator;
        DublinCore.Date = xmpMetadata.DublinCore?.Date ?? DublinCore.Date;
        DublinCore.Description = xmpMetadata.DublinCore?.Description ?? DublinCore.Description;
        DublinCore.Format = xmpMetadata.DublinCore?.Format ?? DublinCore.Format;
        DublinCore.Identifier = xmpMetadata.DublinCore?.Identifier ?? DublinCore.Identifier;
        DublinCore.Language = xmpMetadata.DublinCore?.Language ?? DublinCore.Language;
        DublinCore.Publisher = xmpMetadata.DublinCore?.Publisher ?? DublinCore.Publisher;
        DublinCore.Relation = xmpMetadata.DublinCore?.Relation ?? DublinCore.Relation;
        DublinCore.Rights = xmpMetadata.DublinCore?.Rights ?? DublinCore.Rights;
        DublinCore.Source = xmpMetadata.DublinCore?.Source ?? DublinCore.Source;
        DublinCore.Subject = xmpMetadata.DublinCore?.Subject ?? DublinCore.Subject;
        DublinCore.Title = xmpMetadata.DublinCore?.Title ?? DublinCore.Title;
        DublinCore.Type = xmpMetadata.DublinCore?.Type ?? DublinCore.Type;
    }
}
