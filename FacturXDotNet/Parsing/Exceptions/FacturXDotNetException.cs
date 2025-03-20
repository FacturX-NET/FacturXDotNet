namespace FacturXDotNet.Parsing.Exceptions;

/// <summary>
///     An exception that is thrown when an error occurs in the FacturXDotNet library.
/// </summary>
public class FacturXDotNetException(string message, Exception? innerException = null) : Exception(message, innerException)
{
}
