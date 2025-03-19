namespace FacturXDotNet.Parsers.CII.Exceptions;

/// <summary>
///     The result that was built by the parser is invalid.
/// </summary>
public class CrossIndustryInvoiceInvalidResultException(params IEnumerable<string> errors) : CrossIndustryInvoiceParserException(BuildErrorMessage(errors))
{
    static string BuildErrorMessage(IEnumerable<string> errors)
    {
        List<string> errorsList = errors.ToList();
        if (errorsList.Count == 0)
        {
            return "The document is not a valid Factur-X document.";
        }

        if (errorsList.Count == 1)
        {
            return $"The document is not a valid Factur-X document: {errorsList[0]}.";
        }

        return $"The document is not a valid Factur-X document, see details below.{string.Join(string.Empty, errorsList.Select(e => $"{Environment.NewLine}- {e}"))}";
    }
}
