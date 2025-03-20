namespace FacturXDotNet;

/// <summary>
///     <b>Dublin Core namespace</b> - The Dublin Core namespace provides a set of commonly used properties. The names and usage shall be as defined in the Dublin Core Metadata
///     Element Set, created by the Dublin Core Metadata Initiative (DCMI).
/// </summary>
/// <remarks>See https://developer.adobe.com/xmp/docs/XMPNamespaces/dc/</remarks>
/// <URI>http://purl.org/dc/elements/1.1/</URI>
/// <Prefix>dc</Prefix>
public class XmpDublinCoreMetadata
{
    /// <summary>
    ///     <para>
    ///         <b>DCMI definition</b>: An entity responsible for making contributions to the resource.
    ///     </para>
    ///     <para>
    ///         <b>DCMI comment</b>: Examples of a contributor include a person, an organization, or a service. Typically, the name of a contributor should be used to indicate the entity.
    ///     </para>
    ///     <para>
    ///         <b>XMP addition</b>: XMP usage is a list of contributors. These contributors should not include those listed in dc:creator.
    ///     </para>
    /// </summary>
    /// <XmpTag>dc:contributor</XmpTag>
    public HashSet<string> Contributor { get; set; } = [];

    /// <summary>
    ///     <para>
    ///         <b>DCMI definition</b>: The spatial or temporal topic of the resource, the spatial applicability of the resource, or the jurisdiction under which the resource is relevant.
    ///     </para>
    ///     <para>
    ///         <b>XMP addition</b>: XMP usage is the extent or scope of the resource.
    ///     </para>
    /// </summary>
    /// <XmpTag>dc:coverage</XmpTag>
    public string? Coverage { get; set; }

    /// <summary>
    ///     <para>
    ///         <b>DCMI definition</b>:  An entity primarily responsible for making the resource.
    ///     </para>
    ///     <para>
    ///         <b>DCMI comment</b>: Examples of a creator include a person, an organization, or a service. Typically, the name of a creator should be used to indicate the entity.
    ///     </para>
    ///     <para>
    ///         <b>XMP addition</b>: XMP usage is a list of creators. Entities should be listed in order of decreasing precedence, if such order is significant.
    ///     </para>
    /// </summary>
    /// <XmpTag>dc:creator</XmpTag>
    public List<string> Creator { get; set; } = [];

    /// <summary>
    ///     <para>
    ///         <b>DCMI definition</b>: A point or period of time associated with an event in the life cycle of the resource.
    ///     </para>
    /// </summary>
    /// <XmpTag>dc:date</XmpTag>
    public List<DateTime> Date { get; set; } = [];

    /// <summary>
    ///     <para>
    ///         <b>DCMI definition</b>: An account of the resource.
    ///     </para>
    ///     <para>
    ///         <b>XMP addition</b>: XMP usage is a list of textual descriptions of the content of the resource, given in various languages.
    ///     </para>
    /// </summary>
    /// <XmpTag>dc:description</XmpTag>
    public List<string> Description { get; set; } = [];

    /// <summary>
    ///     <para>
    ///         <b>DCMI definition</b>: The file format, physical medium, or dimensions of the resource.
    ///     </para>
    ///     <para>
    ///         <b>DCMI comment</b>: Examples of dimensions include size and duration. Recommended best practice is to use a controlled vocabulary such as the list of Internet Media Types
    ///         [MIME].
    ///     </para>
    ///     <para>
    ///         <b>XMP addition</b>:  XMP usage is a MIME type. Dimensions would be stored using a media-specific property, beyond the scope of this document.
    ///     </para>
    /// </summary>
    /// <XmpTag>dc:format</XmpTag>
    public string? Format { get; set; }

    /// <summary>
    ///     <para>
    ///         <b>DCMI definition</b>: An unambiguous reference to the resource within a given context.
    ///     </para>
    ///     <para>
    ///         <b>DCMI comment</b>: Recommended best practice is to identify the resource by means of a string conforming to a formal identification system.
    ///     </para>
    /// </summary>
    /// <XmpTag>dc:identifier</XmpTag>
    public string? Identifier { get; set; }

    /// <summary>
    ///     <para>
    ///         <b>DCMI definition</b>: A language of the resource.
    ///     </para>
    ///     <para>
    ///         <b>XMP addition</b>: XMP usage is a list of languages used in the content of the resource.
    ///     </para>
    /// </summary>
    /// <XmpTag>dc:language</XmpTag>
    public HashSet<string> Language { get; set; } = [];

    /// <summary>
    ///     <para>
    ///         <b>DCMI definition</b>: An entity responsible for making the resource available.
    ///     </para>
    ///     <para>
    ///         <b>DCMI comment</b>: Examples of a publisher include a person, an organization, or a service. Typically, the name of a publisher should be used to indicate the entity.
    ///     </para>
    ///     <para>
    ///         <b>XMP addition</b>: XMP usage is a list of publishers.
    ///     </para>
    /// </summary>
    /// <XmpTag>dc:publisher</XmpTag>
    public HashSet<string> Publisher { get; set; } = [];

    /// <summary>
    ///     <para>
    ///         <b>DCMI definition</b>: A related resource.
    ///     </para>
    ///     <para>
    ///         <b>DCMI comment</b>: Recommended best practice is to identify the related resource by means of a string conforming to a formal identification system.
    ///     </para>
    ///     <para>
    ///         <b>XMP addition</b>: XMP usage is a list of related resources.
    ///     </para>
    /// </summary>
    /// <XmpTag>dc:relation</XmpTag>
    public HashSet<string> Relation { get; set; } = [];

    /// <summary>
    ///     <para>
    ///         <b>DCMI definition</b>: Information about rights held in and over the resource.
    ///     </para>
    ///     <para>
    ///         <b>DCMI comment</b>: Typically, rights information includes a statement about various property rights associated with the resource, including intellectual property rights.
    ///     </para>
    ///     <para>
    ///         <b>XMP addition</b>: XMP usage is a list of informal rights statements, given in various languages.
    ///     </para>
    /// </summary>
    /// <XmpTag>dc:rights</XmpTag>
    public List<string> Rights { get; set; } = [];

    /// <summary>
    ///     <para>
    ///         <b>DCMI definition</b>: A related resource from which the described resource is derived.
    ///     </para>
    ///     <para>
    ///         <b>DCMI comment</b>: The described resource may be derived from the related resource in whole or in part. Recommended best practice is to identify the related resource by
    ///         means of a string conforming to a formal identification system.
    ///     </para>
    /// </summary>
    /// <XmpTag>dc:source</XmpTag>
    public string? Source { get; set; }

    /// <summary>
    ///     <para>
    ///         <b>DCMI definition</b>: The topic of the resource.
    ///     </para>
    ///     <para>
    ///         <b>DCMI comment</b>: Typically, the subject will be represented using keywords, key phrases, or classification codes. Recommended best practice is to use a controlled
    ///         vocabulary. To describe the spatial or temporal topic of the resource, use the dc:coverage element.
    ///     </para>
    ///     <para>
    ///         <b>XMP addition</b>: XMP usage is a list of descriptive phrases or keywords that specify the content of the resource.
    ///     </para>
    /// </summary>
    /// <XmpTag>dc:subject</XmpTag>
    public HashSet<string> Subject { get; set; } = [];

    /// <summary>
    ///     <para>
    ///         <b>DCMI definition</b>: A name given to the resource.
    ///     </para>
    ///     <para>
    ///         <b>DCMI comment</b>: Typically, a title will be a name by which the resource is formally known.
    ///     </para>
    ///     <para>
    ///         <b>XMP addition</b>: XMP usage is a title or name, given in various languages.
    ///     </para>
    /// </summary>
    /// <XmpTag>dc:title</XmpTag>
    public List<string> Title { get; set; } = [];

    /// <summary>
    ///     <para>
    ///         <b>DCMI definition</b>: The nature or genre of the resource.
    ///     </para>
    ///     <para>
    ///         <b>DCMI comment</b>: Recommended best practice is to use a controlled vocabulary such as the DCMI Type Vocabulary [DCMITYPE]. To describe the file format, physical medium,
    ///         or dimensions of the resource, use the dc:format element.
    ///     </para>
    ///     <para>
    ///         <b>XMP addition</b>: See the dc:format entry for clarification of the XMP usage of that element.
    ///     </para>
    /// </summary>
    /// <XmpTag>dc:type</XmpTag>
    public HashSet<string> Type { get; set; } = [];
}
