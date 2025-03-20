﻿using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.CII.Schematron;

/// <summary>
///     Provides configuration options for the <see cref="CrossIndustryInvoiceSchematronValidator" />.
/// </summary>
public class CrossIndustryInvoiceSchematronValidationOptions
{
    /// <summary>
    ///     Gets or sets an optional profile override for validation.
    /// </summary>
    /// <remarks>
    ///     If set, this profile will be used for validation instead of the profile specified in the invoice.
    ///     This allows enforcing validation against a specific Factur-X profile regardless of the invoice's declared profile.
    ///     If <c>null</c>, the validator uses the profile defined in the invoice itself.
    /// </remarks>
    public FacturXProfile? ProfileOverride { get; set; } = null;

    /// <summary>
    ///     The list of rules to skip during validation.
    /// </summary>
    public List<string> RulesToSkip { get; } = [];
}
