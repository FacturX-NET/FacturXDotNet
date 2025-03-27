```
FacturX.NET CLI v0.1.0-alpha.4+82dc4ab997370e5b21e2a89d0357faa95c7bb8fe
Copyright © 2025 Ismail Bennani

Usage:
  facturx [command] [options]

Options:
  --version       Show version information
  -?, -h, --help  Show help and usage information

Commands:
  extract <path>   Extracts the content of a Factur-X PDF.
  validate <path>  Validates the content of a Factur-X PDF.
```

# Generate

```
FacturX.NET CLI v0.1.0-alpha.4+82dc4ab997370e5b21e2a89d0357faa95c7bb8fe
Copyright © 2025 Ismail Bennani

Usage:
  facturx generate [options]

Options:
  --pdf <path> (REQUIRED)                                                  The path to the PDF that will be used as base.
  --cii <path> (REQUIRED)                                                  The path to the CII file to use as structured data.
  --cii-name <name>                                                        The name of the CII attachment in the result. [default: factur-x.xml]
  --attach <path>                                                          Additional files to attach to the result.
  -o, --output-path <path>                                                 The path to the output file.
  --skip-validation                                                        Do not validate the generated Factur-X PDF. [default: False]
  --warnings-as-errors                                                     Treat warnings as errors. [default: False]
  -p, --profile <Basic|BasicWl|En16931|Extended|Minimum|None>              The profile to use for validation. If set, the profile will override the one specified in the Factur-X file.
  -s, --skip-rule                                                          The business rules that should be skipped. Example: --skip-rule "BR-01" --skip-rule "BR-02"
  -v, --verbosity <d|detailed|diag|diagnostic|m|minimal|n|normal|q|quiet>  Set the verbosity level. [default: Normal]
  -?, -h, --help                                                           Show help and usage information
```

Example
```
facturx generate --pdf "0.MINIMUM\Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf" --cii "0.MINIMUM\Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.xml" 
```

Result
```
╭─Options──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────╮
│ Basd PDF         D:\source\repos\FacturXDotNet\Specification 1.0.07.2\5. FACTUR-X 1.07.2 - Examples\0.MINIMUM\Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf │
│ CII XML          D:\source\repos\FacturXDotNet\Specification 1.0.07.2\5. FACTUR-X 1.07.2 - Examples\0.MINIMUM\Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.xml │
│ Validate result  True                                                                                                                                                        │
╰──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────╯

✔ The input files have been read in 1 millisecond.
✔ The document has been generated in 70 milliseconds.
✔ The document has been checked in 70 milliseconds.

✔ The document is valid.
✔ Document profile: Minimum.
✔ Detected profile: Extended.

✔ The document has been exported to D:\source\repos\FacturXDotNet\Specification 1.0.07.2\5. FACTUR-X 1.07.2 - Examples\0.MINIMUM\Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM-facturx.pdf in 3 milliseconds.

```

# Validate

```
FacturX.NET CLI v0.1.0-alpha.4+82dc4ab997370e5b21e2a89d0357faa95c7bb8fe
Copyright © 2025 Ismail Bennani

Usage:
  facturx validate <path> [options]

Arguments:
  <path>  The path to the Factur-X PDF.

Options:
  --cii-attachment <name>                                      The name of the CII attachment. [default: factur-x.xml]
  --warnings-as-errors                                         Treat warnings as errors.
  -p, --profile <Basic|BasicWl|En16931|Extended|Minimum|None>  The profile to use for validation. If set, the profile will override the one specified in the Factur-X file.
  -s, --skip-rule                                              The business rules that should be skipped. Example: --skip-rule "BR-01" --skip-rule "BR-02"
  -?, -h, --help                                               Show help and usage information
```

Example
```
facturx validate "0.MINIMUM\Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIEN_MINIMUM.pdf"
```

Result
```
╭─Options──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────╮
│ Document             D:\source\repos\FacturXDotNet\Specification 1.0.07.2\5. FACTUR-X 1.07.2 - Examples\0.MINIMUM\Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf │
╰──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────╯

✔ The document has been parsed in 122 milliseconds.
✔ The document has been checked in 36 milliseconds.

✔ The document is valid.
✔ Document profile: Minimum.
```

# Extract

```
FacturX.NET CLI v0.1.0-alpha.4+82dc4ab997370e5b21e2a89d0357faa95c7bb8fe
Copyright © 2025 Ismail Bennani

Usage:
  facturx extract <path> [options]

Arguments:
  <path>  The path to the Factur-X PDF.

Options:
  --cii <path>             Extracts the content of the CII XML. Optionally specify a path, otherwise the CII XML will be saved next to the PDF with the same name.
  --cii-attachment <name>  The name of the CII attachment. [default: factur-x.xml]
  --xmp <path>             Extracts the content of the XMP metadata. Optionally specify a path, otherwise the XMP metadata will be saved next to the PDF with the same name.
  -?, -h, --help           Show help and usage information
```

Example:
```
facturx validate "0.MINIMUM\Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIEN_MINIMUM.pdf" --cii --xmp
```

Result
```
╭─Options─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────╮
│ Document    D:\source\repos\FacturXDotNet\Specification 1.0.07.2\5. FACTUR-X 1.07.2 - Examples\0.MINIMUM\Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf │
│ Export CII  True                                                                                                                                                        │
╰─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────╯

✔ Extracted CII XML to 'D:\source\repos\FacturXDotNet\Specification 1.0.07.2\5. FACTUR-X 1.07.2 - Examples\0.MINIMUM\Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.xml' in 75 milliseconds.
✔ Extracted XMP metadata to 'D:\source\repos\FacturXDotNet\Specification 1.0.07.2\5. FACTUR-X 1.07.2 - Examples\0.MINIMUM\Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.xmp' in 5 milliseconds.

```