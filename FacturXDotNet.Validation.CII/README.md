# FacturX.NET - CII validation

## Design Choices

### 0. The schematron rules have been reimplemented in C#

For performance reasons, the validation does not use the schematron file provided by the Factur-X standard. Instead, it implements the same set of rules in C#.