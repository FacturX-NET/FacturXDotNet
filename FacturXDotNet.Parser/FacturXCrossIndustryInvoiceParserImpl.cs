using System.Xml;
using FacturXDotNet.Parser.Exceptions;

namespace FacturXDotNet.Parser;

class FacturXCrossIndustryInvoiceParserImpl<T>
{
    readonly Dictionary<string, Action<T, ReadOnlyMemory<char>>> _valueHandlers = [];
    readonly Dictionary<string, Action<T>> _elementHandlers = [];
    Action<T, ReadOnlyMemory<char>, ReadOnlyMemory<char>>? _fallbackValueHandler;

    /// <summary>
    ///     Register a handler that will be called when the parser encounters the value of the element at the provided path.
    /// </summary>
    public void RegisterValueHandler(string path, Action<T, ReadOnlyMemory<char>> handler)
    {
        if (!_valueHandlers.TryAdd(path, handler))
        {
            throw new FacturXCrossIndustryInvoiceParserInitializationException($"Duplicate handler for path: {path}.");
        }
    }

    /// <summary>
    ///     Register a handler that will be called when the parser encounters a value that is not handled by other handlers.
    /// </summary>
    public void RegisterFallbackValueHandler(Action<T, ReadOnlyMemory<char>, ReadOnlyMemory<char>> handler)
    {
        if (_fallbackValueHandler != null)
        {
            throw new FacturXCrossIndustryInvoiceParserInitializationException("Duplicate fallback value handler.");
        }

        _fallbackValueHandler = handler;
    }

    /// <summary>
    ///     Register a handler that will be called when the parser encounters the element at the provided path.
    /// </summary>
    public void RegisterElementHandler(string path, Action<T> handler)
    {
        if (!_elementHandlers.TryAdd(path, handler))
        {
            throw new FacturXCrossIndustryInvoiceParserInitializationException($"Duplicate handler for path: {path}.");
        }
    }

    /// <summary>
    ///     Parse the given stream into the given result.
    /// </summary>
    public async Task ParseAsync(Stream stream, T result)
    {
        using XmlReader reader = XmlReader.Create(stream, new XmlReaderSettings { Async = true, CloseInput = false });

        Stack<ReadOnlyMemory<char>> path = [];
        while (reader.MoveToNextAttribute() || await reader.ReadAsync())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                {
                    ReadOnlyMemory<char> parentPath = path.TryPeek(out ReadOnlyMemory<char> p) ? p : ReadOnlyMemory<char>.Empty;
                    string newPath = $"{parentPath}/{reader.Name}";
                    ReadOnlyMemory<char> newPathAsMemory = newPath.AsMemory();
                    path.Push(newPathAsMemory);

                    CallbackElement(result, newPathAsMemory);

                    if (reader.IsEmptyElement)
                    {
                        path.Pop();
                    }

                    break;
                }
                case XmlNodeType.EndElement:
                {
                    path.Pop();
                    break;
                }
                case XmlNodeType.Text:
                {
                    ReadOnlyMemory<char> parentPath = path.TryPeek(out ReadOnlyMemory<char> p) ? p : ReadOnlyMemory<char>.Empty;
                    string value = await reader.GetValueAsync();
                    Callback(result, parentPath, value.AsMemory());
                    break;
                }
                case XmlNodeType.Attribute:
                {
                    ReadOnlyMemory<char> parentPath = path.TryPeek(out ReadOnlyMemory<char> p) ? p : ReadOnlyMemory<char>.Empty;
                    string attributePath = $"{parentPath}@{reader.Name}";
                    string value = await reader.GetValueAsync();
                    Callback(result, attributePath.AsMemory(), value.AsMemory());
                    break;
                }
                case XmlNodeType.CDATA:
                case XmlNodeType.None:
                case XmlNodeType.EntityReference:
                case XmlNodeType.Entity:
                case XmlNodeType.ProcessingInstruction:
                case XmlNodeType.Comment:
                case XmlNodeType.Document:
                case XmlNodeType.DocumentType:
                case XmlNodeType.DocumentFragment:
                case XmlNodeType.Notation:
                case XmlNodeType.Whitespace:
                case XmlNodeType.SignificantWhitespace:
                case XmlNodeType.EndEntity:
                case XmlNodeType.XmlDeclaration:
                default:
                    continue;
            }
        }
    }

    void Callback(T result, ReadOnlyMemory<char> callbackName, ReadOnlyMemory<char> value)
    {
        try
        {
            if (_valueHandlers.TryGetValue(callbackName.ToString(), out Action<T, ReadOnlyMemory<char>>? handler))
            {
                handler(result, value);
            }
            else
            {
                _fallbackValueHandler?.Invoke(result, callbackName, value);
            }
        }
        catch (Exception exn)
        {
            throw CreateException(exn, callbackName, value);
        }
    }


    void CallbackElement(T result, ReadOnlyMemory<char> callbackName)
    {
        try
        {
            if (_elementHandlers.TryGetValue(callbackName.ToString(), out Action<T>? handler))
            {
                handler(result);
            }
        }
        catch (Exception exn)
        {
            throw CreateException(exn, callbackName);
        }
    }

    static FacturXCrossIndustryInvoiceParsingException CreateException(Exception exn, ReadOnlyMemory<char> callbackName) => new(callbackName.Span, exn);

    static FacturXCrossIndustryInvoiceParsingException CreateException(Exception exn, ReadOnlyMemory<char> callbackName, ReadOnlyMemory<char> value) =>
        new(callbackName.Span, value.Span, exn);
}
