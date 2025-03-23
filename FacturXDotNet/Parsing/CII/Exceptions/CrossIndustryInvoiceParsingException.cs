namespace FacturXDotNet.Parsing.CII.Exceptions;

/// <summary>
///     Represent an exception that occurs while reading a <see cref="CrossIndustryInvoice" />.
/// </summary>
public class CrossIndustryInvoiceParsingException : CrossIndustryInvoiceReaderException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CrossIndustryInvoiceParsingException" /> class.
    /// </summary>
    public CrossIndustryInvoiceParsingException(ReadOnlySpan<char> path, int line, int column, string message) : base(BuildErrorMessage(path, line, column, message))
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CrossIndustryInvoiceParsingException" /> class.
    /// </summary>
    public CrossIndustryInvoiceParsingException(ReadOnlySpan<char> path, int line, int column, Exception innerException) : base(
        BuildErrorMessage(path, line, column, innerException.Message),
        innerException
    )
    {
    }

    static string BuildErrorMessage(ReadOnlySpan<char> path, int line, int column, string message) => $"At '{path}' (line {line}, column {column}): {message.TrimEnd('.')}.";
}
