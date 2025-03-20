![FacturX.NET logo](Assets/Logo/logo.png)

The FacturX.NET library is a .NET library that provides support for the FacturX data formats, as defined by the [FNFE](https://fnfe-mpe.org/factur-x/) (also known as ZUGFeRD in Germany).

These formats are the main formats used in the french and german electronic invoicing systems.

## CLI

TODO

## Editor

TODO

## Libraries

Every feature of the project can be used through code by including the appropriate nuget.

### Manipulate FacturX data

The [FacturX.NET](https://github.com/ismailbennani/FacturXDotNet/tree/master/FacturXDotNet) library provides a set of classes that can be used to represent both 
- the Cross-Industry Invoice (CII) data: the structured data of a FacturX invoice is stored. A FacturX file is a PDF file that contains a CII file as an attachment, that is called `factur-x.xml`.  
- the XMP metadata: a set of metadata that must be embedded in the PDF file.

### Read FacturX files

The [FacturX.NET - CII parser](https://github.com/ismailbennani/FacturXDotNet/tree/master/FacturXDotNet.Parsers.CII) and the [FacturX.NET - FacturX parser](https://github.com/ismailbennani/FacturXDotNet/tree/master/FacturXDotNet.Parsers.FacturX) provide classes that parse CII XML and FacturX PDF files, respectively.
They have been split to allow more precise control over the dependencies of your project, [read more](https://github.com/ismailbennani/FacturXDotNet/tree/master/FacturXDotNet.Parsers.FacturX).

### Read FacturX files

The [FacturX.NET - CII exporter](https://github.com/ismailbennani/FacturXDotNet/tree/master/FacturXDotNet.Exporters.CII) and the [FacturX.NET - FacturX exporter](https://github.com/ismailbennani/FacturXDotNet/tree/master/FacturXDotNet.Exporters.FacturX) provide classes that write CII XML and a standard FacturX PDF files with a standard representation, respectively.
They have been split to allow more precise control over the dependencies of your project, [read more](https://github.com/ismailbennani/FacturXDotNet/tree/master/FacturXDotNet.Exporters.FacturX).

### Validate FacturX files

The [FacturX.NET - CII validation](https://github.com/ismailbennani/FacturXDotNet/tree/master/FacturXDotNet.Validation.CII) and the [FacturX.NET - FacturX validation](https://github.com/ismailbennani/FacturXDotNet/tree/master/FacturXDotNet.Validation.FacturX) provide classes that validate CII XML and FacturX PDF files, respectively.
They have been split to allow more precise control over the dependencies of your project, [read more](https://github.com/ismailbennani/FacturXDotNet/tree/master/FacturXDotNet.Validation.FacturX).