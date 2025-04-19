![FacturX.NET logo](assets/Logo/logo.png)

[![NuGet - Library](https://img.shields.io/nuget/v/FacturXDotNet
)](https://www.nuget.org/packages/FacturXDotNet/)
[![NuGet - CLI](https://img.shields.io/nuget/v/FacturXDotNet.CLI?label=tool
)](https://www.nuget.org/packages/FacturXDotNet.CLI/)

[![Build & Test](https://github.com/FacturX-NET/FacturXDotNet/actions/workflows/ci-main.yml/badge.svg)](https://github.com/FacturX-NET/FacturXDotNet/actions/workflows/ci-main.yml)

> [!IMPORTANT]
> **The library is still a work in progress. If you stumbled upon this somehow, you are lost!**

The FacturX.NET library is a high-performance .NET library ([benchmarks](https://github.com/FacturX-NET/FacturXDotNet/tree/master/Benchmark)) that provides support for the
Factur-X data formats, as defined by the [FNFE](https://fnfe-mpe.org/factur-x/) (also known as ZUGFeRD in Germany).

These formats are the main formats used in the french and german electronic invoicing systems.

It comes in multiple flavors:

- a CLI tool: [nuget](https://www.nuget.org/packages/FacturXDotNet.CLI) | [source](https://github.com/FacturX-NET/FacturXDotNet/tree/master/FacturXDotNet.CLI)
- a desktop editor: (coming soon)
- a web API: [app](https://api.facturxdotnet.org) | [source](https://github.com/FacturX-NET/FacturXDotNet/tree/master/FacturXDotNet.API)
- a web app: [app](https://editor.facturxdotnet.org) | [source](https://github.com/FacturX-NET/FacturXDotNet/tree/master/FacturXDotNet.WebEditor)
- a .NET library: [nuget](https://www.nuget.org/packages/FacturXDotNet) | [source](https://github.com/FacturX-NET/FacturXDotNet/tree/master/FacturXDotNet)

All these tools provide the same set of [features](#features).

If you spot a mistake or believe something could be improved, feel free to **open an issue** or even **contribute a fix**! Your feedback and contributions help keep the project
accurate and reliable.

# Features

## Generation

### Generate a Factur-X document

A Factur-X document is a PDF containing
- One or multiple pages that are the visual representation of the invoice
- An XML attachment containing the invoice data in the Cross-Industry Invoice format
- XMP metadata that must at least give information about the conformance level to the PDF/A standard and to the EN16931 standard
- Other attachments

#### API: `POST /generate/facturx`
Test it live: [https://api.facturxdotnet.org](https://api.facturxdotnet.org/scalar/#tag/generate/POST/generate/facturx)

The `POST /generate/facturx` endpoint generates a Factur-X document by combining the following inputs:
- A binary PDF file (required)
- CII (Cross Industry Invoice) data in JSON format (required)
- XMP metadata in JSON format (optional): if omitted, minimal metadata will be generated automatically
- Additional attachments as binary files (optional)

**Usage**
```shell
curl https://api.facturxdotnet.org/generate/facturx \
  --request POST \
  --header 'Content-Type: multipart/form-data;boundary=----549a116c9bd650be51ba2de6c4869b49' \
  --data '
  ------549a116c9bd650be51ba2de6c4869b49
  Content-Disposition: form-data; name="pdf"; filename="base.pdf"
  Content-Type: application/pdf
  
  ...PDF binary data...
  ------549a116c9bd650be51ba2de6c4869b49
  Content-Disposition: form-data; name="cii"; filename="factur-x.xml"
  Content-Type: application/json
  
  ...CII JSON data...
  ------549a116c9bd650be51ba2de6c4869b49
  Content-Disposition: form-data; name="xmp"; filename="blob"
  Content-Type: application/json
  
  ...XMP JSON data...
  ------549a116c9bd650be51ba2de6c4869b49
  Content-Disposition: form-data; name="attachments[0].file"; filename="attachment.ext"
  Content-Type: application/octet-stream
  
  ...attachment binary data...
  ------549a116c9bd650be51ba2de6c4869b49
  Content-Disposition: form-data; name="attachments[1].description"
  Content-Type: text/plain

  ...attachment description text...
  ------549a116c9bd650be51ba2de6c4869b49
  '

```


#### CLI: `generate`

The `generate` sub-command creates a Factur-X file by combining a PDF file with a Cross-Industry Invoice data file.
By default, it validates the CII data before generating the output to ensure compliance and correctness.

**Example**
```
facturx generate --pdf path/to/pdf-file.pdf --cii path/to/cii-file.xml --output-path /path/to/facturx.pdf
```

**Usage**
```
FacturX.NET CLI v0.2.1-alpha+790b140996d1c1bef17d6f725e0d7fd776df34b1
Copyright © 2025 Ismail Bennani

Usage:
  facturx generate [options]

Options:
  --pdf <path> (REQUIRED)                                                  The path to the PDF that will be used as base.
  --cii <path> (REQUIRED)                                                  The path to the CII file to use as structured data.
  --cii-name <name>                                                        The name of the CII attachment in the result. [default: factur-x.xml]
  --attach <path>                                                          Additional files to attach to the result.
  --author <name>                                                          The name of the author of the document. This will be added to the metadata of the output file.
  -o, --output-path <path>                                                 The path to the output file.
  --skip-validation                                                        Do not validate the generated Factur-X PDF. [default: False]
  --warnings-as-errors                                                     Treat warnings as errors. [default: False]
  -p, --profile <Basic|BasicWl|En16931|Extended|Minimum|None>              The profile to use for validation. If set, the profile will override the one specified in the Factur-X file.
  -s, --skip-rule                                                          The business rules that should be skipped. Example: --skip-rule "BR-01" --skip-rule "BR-02"
  -v, --verbosity <d|detailed|diag|diagnostic|m|minimal|n|normal|q|quiet>  Set the verbosity level. [default: Normal]
  -?, -h, --help                                                           Show help and usage information
```