using FacturXDotNet.Parsing.Exceptions;

namespace FacturXDotNet.Parsing.CII.Exceptions;

/// <summary>
///     Represent an exception that occurs during the parsing of a <see cref="CrossIndustryInvoice" />.
/// </summary>
public class CrossIndustryInvoiceReaderException(string message, Exception? innerException = null) : FacturXDotNetException(message, innerException)
{
    /// <summary>
    ///     Represent an exception that occurs while reading a <see cref="CrossIndustryInvoice" />.
    /// </summary>
    /// <param name="path">The XPath where the exception occurs</param>
    /// <param name="line">The line of the file where the exception occurs</param>
    /// <param name="column">The column of the file where the exception occurs</param>
    /// <param name="message">The message of the exception</param>
    public static CrossIndustryInvoiceReaderException ParsingError(ReadOnlySpan<char> path, int line, int column, string message) =>
        new($"At '{path}' (line {line}, column {column}): {message.TrimEnd('.')}.");

    /// <summary>
    ///     Represent an exception that occurs while reading a <see cref="CrossIndustryInvoice" />.
    /// </summary>
    /// <param name="path">The XPath where the exception occurs</param>
    /// <param name="line">The line of the file where the exception occurs</param>
    /// <param name="column">The column of the file where the exception occurs</param>
    /// <param name="exception">The exception that occurred</param>
    public static CrossIndustryInvoiceReaderException ParsingError(ReadOnlySpan<char> path, int line, int column, Exception exception) =>
        new($"At '{path}' (line {line}, column {column}): {exception.Message.TrimEnd('.')}.", exception);

    /// <summary>
    ///     Represent an exception that occurs while validating the <see cref="CrossIndustryInvoice" /> that has been read.
    /// </summary>
    /// <param name="errors">The errors that occurred during the validation</param>
    public static CrossIndustryInvoiceReaderException ValidationError(params IEnumerable<string> errors)
    {
        List<string> errorsList = errors.ToList();
        string errorMessage = errorsList.Count switch
        {
            0 => "The document is not a valid Cross-Industry Invoice.",
            1 => $"The document is not a valid Cross-Industry Invoice: {errorsList[0].TrimEnd('.')}.",
            _ => $"The document is not a valid Cross-Industry Invoice, see details below.{string.Join(string.Empty, errorsList.Select(e => $"{Environment.NewLine}- {e}"))}"
        };

        return new CrossIndustryInvoiceReaderException(errorMessage);
    }
}
