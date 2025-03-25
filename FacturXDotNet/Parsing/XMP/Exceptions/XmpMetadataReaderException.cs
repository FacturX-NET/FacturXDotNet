using FacturXDotNet.Exceptions;

namespace FacturXDotNet.Parsing.XMP.Exceptions;

/// <summary>
///     Represent an exception that occurs during the parsing of a <see cref="XmpMetadata" />.
/// </summary>
public class XmpMetadataReaderException(string message, Exception? innerException = null) : FacturXDotNetException(message, innerException)
{
    /// <summary>
    ///     Represent an exception that occurs while reading a <see cref="CrossIndustryInvoice" />.
    /// </summary>
    /// <param name="path">The XPath where the exception occurs</param>
    /// <param name="line">The line of the file where the exception occurs</param>
    /// <param name="column">The column of the file where the exception occurs</param>
    /// <param name="message">The message of the exception</param>
    public static XmpMetadataReaderException ParsingError(ReadOnlySpan<char> path, int line, int column, string message) =>
        new($"At '{path}' (line {line}, column {column}): {message.TrimEnd('.')}.");

    /// <summary>
    ///     Represent an exception that occurs while reading a <see cref="CrossIndustryInvoice" />.
    /// </summary>
    /// <param name="path">The XPath where the exception occurs</param>
    /// <param name="line">The line of the file where the exception occurs</param>
    /// <param name="column">The column of the file where the exception occurs</param>
    /// <param name="exception">The exception that occurred</param>
    public static XmpMetadataReaderException ParsingError(ReadOnlySpan<char> path, int line, int column, Exception exception) =>
        new($"At '{path}' (line {line}, column {column}): {exception.Message.TrimEnd('.')}.", exception);
}
