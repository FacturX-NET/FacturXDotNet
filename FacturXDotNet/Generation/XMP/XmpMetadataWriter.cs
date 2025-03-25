using System.Text;
using System.Xml;
using FacturXDotNet.Models.XMP;

namespace FacturXDotNet.Generation.XMP;

/// <summary>
///     Writes XMP metadata to a stream.
/// </summary>
public class XmpMetadataWriter(XmpMetadataWriterOptions? options = null)
{
    readonly XmpMetadataWriterOptions _options = options ?? new XmpMetadataWriterOptions();

    readonly XmpPdfAIdentificationMetadataWriter _xmpPdfAIdentificationMetadataWriter = new();
    readonly XmpBasicMetadataWriter _xmpBasicMetadataWriter = new();
    readonly XmpPdfMetadataWriter _xmpPdfMetadataWriter = new();
    readonly XmpDublinCoreMetadataWriter _xmpDublinCoreMetadataWriter = new();
    readonly XmpPdfAExtensionsMetadataWriter _xmpPdfAExtensionsMetadataWriter = new();
    readonly XmpFacturXMetadataWriter _xmpFacturXMetadataWriter = new();

    /// <summary>
    ///     Writes the XMP metadata to the specified stream.
    /// </summary>
    /// <param name="stream">The stream to write the XMP metadata to.</param>
    /// <param name="xmp">The XMP metadata to write.</param>
    public async Task WriteAsync(Stream stream, XmpMetadata xmp)
    {
        await using XmlWriter writer = XmlWriter.Create(
            stream,
            new XmlWriterSettings { Encoding = Encoding.UTF8, Async = true, Indent = true, ConformanceLevel = ConformanceLevel.Fragment }
        );

        // XMP Specification page 11
        // https://archimedespalimpsest.net/Documents/External/XMP/XMPSpecificationPart3.pdf
        //
        // The header is an XML processing instruction of the form:
        //
        // <?xpacket ... ?>
        //
        // The processing instruction contains information about the packet in the form of XML attributes. There are
        // two required attributes: begin and id, in that order. Other attributes can follow in any order; unrecognized
        // attributes should be ignored. Attributes must be separated by exactly one ASCII space (U+0020) character.
        // 
        // Attribute: begin
        //   This required attribute indicates the beginning of a new packet. Its value is the Unicode zero-width
        //   non-breaking space character U+FEFF, in the appropriate encoding (UTF-8, UTF-16, or UTF-32). It serves as
        //   a byte-order marker, where the character is written in the natural order of the application (consistent with
        //   the byte order of the XML data encoding).
        //   For backward compatibility with earlier versions of the XMP packet specification, the value of this attribute
        //   can be the empty string, indicating UTF-8 encoding.
        //   “Scanning files for XMP packets” on page 13 describes how an XMP packet processor should read a single
        //   byte at a time until it has successfully determined the byte order and encoding.
        // Attribute: id
        //   The required id attribute must follow begin. For all packets defined by this version of the syntax, the value
        //   of id is the following string: W5M0MpCehiHzreSzNTczkc9d
        // Attribute: bytes
        //   This attribute is deprecated.
        //   The optional bytes attribute specifies the total length of the packet in bytes, which was intended to allow
        //   faster scanning of XMP packets. It was of minimal actual value, and would not work properly in text files.
        // Attribute: encoding
        //   This attribute is deprecated.
        //   The optional encoding attribute is identical to the encoding attribute in the XML declaration (see
        //   productions [23] and [80] in the XML specification). It was intended to specify the character encoding of
        //   the packet, but is redundant with the information from the begin attribute

        await writer.WriteProcessingInstructionAsync("xpacket", "begin='\uFEFF' id='W5M0MpCehiHzreSzNTczkc9d'");

        await writer.WriteStartElementAsync("x", "xmpmeta", "adobe:ns:meta/");
        await writer.WriteStartElementAsync("rdf", "RDF", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");

        if (xmp.PdfAIdentification != null)
        {
            await _xmpPdfAIdentificationMetadataWriter.WriteAsync(writer, xmp.PdfAIdentification);
        }

        if (xmp.DublinCore != null)
        {
            await _xmpDublinCoreMetadataWriter.WriteAsync(writer, xmp.DublinCore);
        }

        if (xmp.Pdf != null)
        {
            await _xmpPdfMetadataWriter.WriteAsync(writer, xmp.Pdf);
        }

        if (xmp.Basic != null)
        {
            await _xmpBasicMetadataWriter.WriteAsync(writer, xmp.Basic);
        }

        if (xmp.PdfAExtensions != null)
        {
            await _xmpPdfAExtensionsMetadataWriter.WriteAsync(writer, xmp.PdfAExtensions);
        }

        if (xmp.FacturX != null)
        {
            await _xmpFacturXMetadataWriter.WriteAsync(writer, xmp.FacturX);
        }

        await writer.WriteEndElementAsync();
        await writer.WriteEndElementAsync();

        // XMP Specification page 12
        // https://archimedespalimpsest.net/Documents/External/XMP/XMPSpecificationPart3.pdf
        //
        // This required processing instruction indicates the end of the XMP packet.
        //
        // <?xpacket end='w'?>
        //
        // Attribute: end
        //   The end attribute is required, and must be the first attribute. Other unrecognized attributes can follow, but
        //   should be ignored. Attributes must be separated by exactly one ASCII space (U+0020) character.
        //   The value of end indicates whether applications that do not understand the containing file format are
        //   allowed to update the XMP packet:
        //   ➤ The value r means the packet is “read-only” and must not be updated in place. This would be used for
        //      example if a file contained an overall checksum that included the embedded XMP.
        //      The use of the value r does restrict the behavior of applications that understand the file format and
        //      are capable of properly rewriting the file.
        //   ➤ The value w means the packet can be updated in place, if there is enough space. The overall length of
        //      the packet must not be changed; padding should be adjusted accordingly. The original encoding and
        //      byte order must be preserved, to avoid breaking text files containing XMP or violating other
        //      constraints of the original application.

        await writer.WriteProcessingInstructionAsync("xpacket", "end='w'");
        await writer.FlushAsync();
    }
}
