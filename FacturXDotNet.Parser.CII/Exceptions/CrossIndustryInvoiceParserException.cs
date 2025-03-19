﻿namespace FacturXDotNet.Parser.CII.Exceptions;

/// <summary>
///     Represent an exception that occurs during the parsing of a <see cref="CrossIndustryInvoice" />.
/// </summary>
public abstract class CrossIndustryInvoiceParserException(string message, Exception? innerException = null) : Exception(message, innerException)
{
}
