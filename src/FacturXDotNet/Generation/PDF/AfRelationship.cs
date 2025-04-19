namespace FacturXDotNet.Generation.PDF;

/// <summary>
///     The relationship of a file to a PDF document.
/// </summary>
/// <remarks>
///     See Section 7.11.3 of ISO 32000-2:2020 (PDF 2.0). <br />
///     https://pdfa.org/resource/iso-32000-2/
/// </remarks>
public enum AfRelationship
{
    /// <summary>
    ///     Shall be used when the relationship is not known or cannot be described using one of the other values.
    /// </summary>
    Unspecified = 0,

    /// <summary>
    ///     Shall be used if this file specification is the original source material for the associated content.
    /// </summary>
    Source,

    /// <summary>
    ///     Shall be used if this file specification represents information used to derive a visual presentation – such as for a table or a graph.
    /// </summary>
    Data,

    /// <summary>
    ///     Shall be used if this file specification is an alternative representation of content, for example audio.
    /// </summary>
    Alternative,

    /// <summary>
    ///     Shall be used if this file specification represents a supplemental representation of the original source or data that may be more easily consumable (e.g., A MathML version of
    ///     an equation).
    /// </summary>
    Supplement,

    /// <summary>
    ///     Shall be used if this file specification is an encrypted payload document that should be displayed to the user if the PDF processor has the cryptographic filter needed to
    ///     decrypt the document.
    /// </summary>
    EncryptedPayload,

    /// <summary>
    ///     Shall be used if this file specification is the data associated with the AcroForm (see 12.7.3, "Interactive form dictionary") of this PDF.
    /// </summary>
    FormData,

    /// <summary>
    ///     Shall be used if this file specification is a schema definition for the associated object (e.g. an XML schema associated with a metadata stream).
    /// </summary>
    Schema
}
