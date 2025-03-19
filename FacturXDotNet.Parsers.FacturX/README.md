# FacturX.NET - FacturX parser

The main goal of this library is to provide a parser for Factur-X files, based on the [Factur-X standard](https://fnfe-mpe.org/factur-x/) version.

### Why is this library separated from [FacturX.NET - CII Parser](../FacturXDotNet.Parser.CII)?

Handling PDF files is notoriously difficult. The most used libraries in the .NET ecosystem all come with drawbacks:
- PdfSharp does not allow easy manipulation of attachments
- iText is only free under the AGPL license
- GDPicturePdf is not free
- ...

This library uses iText because the project is open-source anyway, and the AGPL license is not a problem. However, it might be a problem for other people. By separating the Factur-X and CII parsers, users can choose to use only the CII parser if they want to avoid the headache, or if they have already another PDF library at their disposal to extract the CII XML attachment.

If you know of an open-source library under a more permissive license that can extract attachments from PDF files, please let us know!