namespace FacturXDotNet.Parsers.XMP.Exceptions;

/// <summary>
///     Represent an exception that occurs during the parsing of a <see cref="XmpMetadata" />.
/// </summary>
public abstract class XmpMetadataParserException(string message, Exception? innerException = null) : Exception(message, innerException)
{
}
