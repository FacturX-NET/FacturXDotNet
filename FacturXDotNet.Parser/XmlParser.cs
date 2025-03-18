using System.Xml;

namespace FacturXDotNet.Parser;

class XmlParser<T>
{
    readonly Dictionary<string, Action<T, ReadOnlyMemory<char>>> _handlers = [];
    Action<T, ReadOnlyMemory<char>, ReadOnlyMemory<char>>? _fallbackHandler;

    /// <summary>
    ///     Register a handler that will be called when the provided path is encountered in the file.
    /// </summary>
    public void RegisterHandler(ReadOnlyMemory<char> path, Action<T, ReadOnlyMemory<char>> callback) => _handlers[path.ToString()] = callback;

    /// <summary>
    ///     Register a handler that will be called when no other handler is found for the given path.
    /// </summary>
    public void RegisterFallbackHandler(Action<T, ReadOnlyMemory<char>, ReadOnlyMemory<char>> handler) => _fallbackHandler = handler;

    public async Task ParseAsync(Stream stream, T result)
    {
        using XmlReader reader = XmlReader.Create(stream, new XmlReaderSettings { Async = true, CloseInput = false });

        Stack<ReadOnlyMemory<char>> path = [];
        while (await reader.ReadAsync())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    ReadOnlyMemory<char> parentPath = path.TryPeek(out ReadOnlyMemory<char> p) ? p : ReadOnlyMemory<char>.Empty;
                    string newPath = $"{parentPath}/{reader.Name}";
                    path.Push(newPath.AsMemory());
                    break;
                case XmlNodeType.EndElement:
                    path.Pop();
                    break;
                case XmlNodeType.Text:
                    string textValue = await reader.GetValueAsync();
                    Callback(result, path.Peek(), textValue.AsMemory());
                    break;
                case XmlNodeType.Attribute:
                    string callbackName = $"{path.Peek()}@{reader.Name}";
                    string attributeValue = await reader.GetValueAsync();
                    Callback(result, callbackName.AsMemory(), attributeValue.AsMemory());
                    break;
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
        if (_handlers.TryGetValue(callbackName.ToString(), out Action<T, ReadOnlyMemory<char>>? handler))
        {
            handler(result, value);
        }
        else
        {
            _fallbackHandler?.Invoke(result, callbackName, value);
        }
    }
}
