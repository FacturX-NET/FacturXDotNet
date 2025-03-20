namespace FacturXDotNet.Parsers.CII.Exceptions;

/// <summary>
///     The result that was built by the parser is invalid.
/// </summary>
public class CrossIndustryInvoiceInvalidResultException(params IEnumerable<string> errors) : CrossIndustryInvoiceParserException(BuildErrorMessage(errors))
{
    static string BuildErrorMessage(IEnumerable<string> errors)
    {
        List<string> errorsList = errors.ToList();
        return errorsList.Count switch
        {
            0 => "The document is not a valid Factur-X document.",
            1 => $"The document is not a valid Factur-X document: {errorsList[0].TrimEnd('.')}.",
            _ => $"The document is not a valid Factur-X document, see details below.{string.Join(string.Empty, errorsList.Select(e => $"{Environment.NewLine}- {e}"))}"
        };
    }
}
