---
title: What is FacturX.NET?
editLink: true 
---

![FacturX.NET Logo](/logo.png)

The FacturX.NET library is a high-performance .NET library ([benchmarks](https://github.com/FacturX-NET/FacturXDotNet/tree/master/Benchmark)) that provides support for the
Factur-X data formats, as defined by the [FNFE](https://fnfe-mpe.org/factur-x/) (also known as ZUGFeRD in Germany).

These formats are the main formats used in the french and german electronic invoicing systems.

It comes in multiple flavors:

- a CLI tool: [nuget](https://www.nuget.org/packages/FacturXDotNet.CLI) | [source](https://github.com/FacturX-NET/FacturXDotNet/tree/master/FacturXDotNet.CLI)
- a desktop editor: (coming soon)
- a web API: [app](https://api.facturxdotnet.org) | [source](https://github.com/FacturX-NET/FacturXDotNet/tree/master/FacturXDotNet.API)
- a web app: [app](https://editor.facturxdotnet.org) | [source](https://github.com/FacturX-NET/FacturXDotNet/tree/master/FacturXDotNet.WebEditor)
- a .NET library: [nuget](https://www.nuget.org/packages/FacturXDotNet) | [source](https://github.com/FacturX-NET/FacturXDotNet/tree/master/FacturXDotNet)

## Features

All these tools provide the same set of features.

### Generation
#### [Generate a Factur-X document](/guides/generation/facturx)
#### [Generate a standard PDF](/guides/generation/standard-pdf)

### Validation
#### [Validate a Factur-X document](/guides/validation/facturx)
#### [Validate Cross-Industry Invoice data](/guides/validation/cii)

### Extraction
#### [Extract Cross-Industry Invoice data](/guides/extraction/cii)
#### [Extract XMP metadata](/guides/extraction/xmp)

## Self-hosting

The demo versions of the editor and API are intended for demonstration purposes only. They run on low-cost, limited, and insecure public cloud infrastructure.

If you plan to use these tools in a production environment, we strongly recommend self-hosting them for better performance, security, and control.

[Learn more aboute self-hosting >](/guides/self-hosting)