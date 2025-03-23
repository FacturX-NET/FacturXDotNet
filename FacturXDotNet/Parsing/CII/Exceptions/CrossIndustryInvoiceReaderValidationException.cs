namespace FacturXDotNet.Parsing.CII.Exceptions;

/// <summary>
///     The result that was built by the reader is invalid.
/// </summary>
public class CrossIndustryInvoiceReaderValidationException(params IEnumerable<string> errors) : CrossIndustryInvoiceReaderException(BuildErrorMessage(errors))
{
    static string BuildErrorMessage(IEnumerable<string> errors)
    {
        List<string> errorsList = errors.ToList();
        return errorsList.Count switch
        {
            0 => "The document is not a valid Cross-Industry Invoice.",
            1 => $"The document is not a valid Cross-Industry Invoice: {errorsList[0].TrimEnd('.')}.",
            _ => $"The document is not a valid Cross-Industry Invoice, see details below.{string.Join(string.Empty, errorsList.Select(e => $"{Environment.NewLine}- {e}"))}"
        };
    }
}
