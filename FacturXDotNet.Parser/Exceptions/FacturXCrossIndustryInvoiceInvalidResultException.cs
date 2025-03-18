namespace FacturXDotNet.Parser.Exceptions;

/// <summary>
///     The result that was built by the parser is invalid.
/// </summary>
public class FacturXCrossIndustryInvoiceInvalidResultException(params IEnumerable<string> errors) : FacturXCrossIndustryInvoiceParserException(BuildErrorMessage(errors))
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
