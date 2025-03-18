using FacturXDotNet.Models;

namespace FacturXDotNet.Parser.Exceptions;

/// <summary>
///     Represent an exception that occurs while parsing a <see cref="FacturXCrossIndustryInvoice" />.
/// </summary>
public class FacturXCrossIndustryInvoiceParsingException : FacturXCrossIndustryInvoiceParserException
{
    public FacturXCrossIndustryInvoiceParsingException(ReadOnlySpan<char> path, ReadOnlySpan<char> value, Exception innerException) : base(
        BuildErrorMessage(path, value, innerException),
        innerException
    )
    {
    }

    public FacturXCrossIndustryInvoiceParsingException(ReadOnlySpan<char> path, Exception innerException) : base(BuildErrorMessage(path, innerException), innerException)
    {
    }

    static string BuildErrorMessage(ReadOnlySpan<char> path, Exception innerException) => $"At '{path}': {innerException.Message}.";
    static string BuildErrorMessage(ReadOnlySpan<char> path, ReadOnlySpan<char> value, Exception innerException) => $"At '{path}': {innerException.Message} (value was '{value}').";
}
