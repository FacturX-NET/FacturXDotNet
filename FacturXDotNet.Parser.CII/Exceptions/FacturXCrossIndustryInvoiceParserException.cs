using FacturXDotNet.Models;

namespace FacturXDotNet.Parser.CII.Exceptions;

/// <summary>
///     Represent an exception that occurs during the parsing of a <see cref="FacturXCrossIndustryInvoice" />.
/// </summary>
public abstract class FacturXCrossIndustryInvoiceParserException(string message, Exception? innerException = null) : Exception(message, innerException)
{
}
