```
FacturX.NET CLI v1.0.0+4470971efa42b446a00d5a16d69ce7b8aebc59c7
Copyright © 2025 Ismail Bennani

Usage:
  FacturXDotNet.CLI [command] [options]

Options:
  --version       Show version information
  -?, -h, --help  Show help and usage information

Commands:
  extract <path>   Extracts the content of a Factur-X PDF.
  validate <path>  Validates the content of a Factur-X PDF.
```

# Extract

```
FacturX.NET CLI v1.0.0+4470971efa42b446a00d5a16d69ce7b8aebc59c7
Copyright © 2025 Ismail Bennani

Usage:
  FacturXDotNet.CLI extract <path> [options]

Arguments:
  <path>  The path to the Factur-X PDF.

Options:
  --cii <path>             Extracts the content of the CII XML. Optionally specify a path, otherwise the CII XML will be saved next to the PDF with the same name.
  --cii-attachment <name>  The name of the CII attachment. [default: factur-x.xml]
  --xmp <path>             Extracts the content of the XMP metadata. Optionally specify a path, otherwise the XMP metadata will be saved next to the PDF with the same name.
  -?, -h, --help           Show help and usage information
```

# Validate

```
FacturX.NET CLI v1.0.0+4470971efa42b446a00d5a16d69ce7b8aebc59c7
Copyright © 2025 Ismail Bennani

Usage:
  FacturXDotNet.CLI validate <path> [options]

Arguments:
  <path>  The path to the Factur-X PDF.

Options:
  --cii-attachment <name>                                      The name of the CII attachment. [default: factur-x.xml]
  --warnings-as-errors                                         Treat warnings as errors.
  -p, --profile <Basic|BasicWl|En16931|Extended|Minimum|None>  The profile to use for validation. If set, the profile will override the one specified in the Factur-X file.
  -s, --skip-rule                                              The business rules that should be skipped. Example: --skip-rule "BR-DE-1" --skip-rule "BR-DE-2"
  -?, -h, --help                                               Show help and usage information
```