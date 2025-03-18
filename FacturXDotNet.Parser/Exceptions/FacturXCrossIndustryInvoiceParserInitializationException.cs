namespace FacturXDotNet.Parser.Exceptions;

/// <summary>
///     Represent an exception that occurs during the initialization of a <see cref="FacturXCrossIndustryInvoiceParser" />.
/// </summary>
public class FacturXCrossIndustryInvoiceParserInitializationException(string message) : FacturXCrossIndustryInvoiceParserException(message)
{
}
