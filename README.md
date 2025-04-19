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

#### API
Try me at [https://api.facturxdotnet.org](https://api.facturxdotnet.org/scalar/#tag/generate/POST/generate/facturx)

The `POST /generate/facturx` endpoint builds a Factur-X document using the given PDF file (binary), CII data (json), XMP metadata (json, optional), and attachments (binary, optional). 

Usage
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
