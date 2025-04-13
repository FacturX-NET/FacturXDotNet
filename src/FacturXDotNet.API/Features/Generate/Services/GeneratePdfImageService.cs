using FacturXDotNet.Generation.PDF;

namespace FacturXDotNet.API.Features.Generate.Services;

/// <summary>
///     Service responsible for managing and providing access to PDF generator implementations.
/// </summary>
public class GeneratePdfImageService
{
    readonly Dictionary<string, IPdfGenerator> _pdfGenerators = new();

    /// <summary>
    ///     Registers a PDF generator with a specified name into the internal collection.
    /// </summary>
    /// <param name="name">The name associated with the PDF generator to be registered.</param>
    /// <param name="generator">The PDF generator implementing the IPdfGenerator interface.</param>
    public void RegisterPdfGenerator(string name, IPdfGenerator generator) => _pdfGenerators.Add(name, generator);

    /// <summary>
    ///     Retrieves a collection of available model names associated with the registered PDF generators.
    /// </summary>
    /// <returns>The read-only collection of names representing the available models in the internal collection.</returns>
    public IReadOnlyCollection<string> GetAvailableModels() => _pdfGenerators.Keys;

    /// <summary>
    ///     Retrieves the PDF generator associated with the specified model name from the internal collection.
    /// </summary>
    /// <param name="modelName">The name of the model associated with the desired PDF generator.</param>
    /// <returns>The PDF generator instance if found; otherwise, null.</returns>
    public IPdfGenerator? GetPdfGenerator(string modelName) => _pdfGenerators.GetValueOrDefault(modelName);
}
