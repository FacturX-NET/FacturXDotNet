namespace FacturXDotNet.Parsers.CII.Exceptions;

/// <summary>
///     Represent an exception that occurs during the initialization of a <see cref="CrossIndustryInvoiceParser" />.
/// </summary>
public class CrossIndustryInvoiceParserInitializationException(string message) : CrossIndustryInvoiceParserException(message)
{
}
