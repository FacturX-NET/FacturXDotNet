namespace FacturXDotNet.Parsers.XMP.Exceptions;

/// <summary>
///     Represent an exception that occurs while parsing a <see cref="XmpMetadata" />.
/// </summary>
public class XmpMetadataParsingException : XmpMetadataParserException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="XmpMetadataParsingException" /> class.
    /// </summary>
    public XmpMetadataParsingException(int line, int column, string message) : base(BuildErrorMessage(line, column, message))
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="XmpMetadataParsingException" /> class.
    /// </summary>
    public XmpMetadataParsingException(int line, int column, Exception innerException) : base(BuildErrorMessage(line, column, innerException.Message), innerException)
    {
    }

    static string BuildErrorMessage(int line, int column, string message) => $"At line {line}, column {column}: {message.TrimEnd('.')}.";
}
