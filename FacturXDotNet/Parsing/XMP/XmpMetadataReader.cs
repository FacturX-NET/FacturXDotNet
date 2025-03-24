﻿using System.Text.RegularExpressions;
using TurboXml;

namespace FacturXDotNet.Parsing.XMP;

/// <summary>
///     Read a <see cref="XmpMetadata" /> from an XML stream.
/// </summary>
public partial class XmpMetadataReader(XmpMetadataReaderOptions? options = null)
{
    readonly XmpMetadataReaderOptions _options = options ?? new XmpMetadataReaderOptions();

    /// <summary>
    ///     Parse the given stream into a <see cref="XmpMetadata" />.
    /// </summary>
    public XmpMetadata Read(Stream stream)
    {
        long position = stream.Position;
        Span<byte> firstChars = stackalloc byte[5];
        stream.ReadExactly(firstChars);
        stream.Seek(position, SeekOrigin.Begin);

        MemoryStream? transformedStream = null;
        try
        {
            if (firstChars[0] == '<' && (firstChars[1] != '?' || firstChars[2] != 'x' || firstChars[3] != 'm' || firstChars[4] != 'l'))
            {
                // TODO: avoid these two extra copies, it is only required because TurboXML doesn't support the <?xpacket...?> processing instructions
                // an issue has been opened to address this: https://github.com/xoofx/TurboXml/issues/6
                // I need to fix this in the library, but it will take some time

                using StreamReader reader = new(stream, leaveOpen: true);
                string content = reader.ReadToEnd();
                string transformedContent = PacketInstructions().Replace(content, string.Empty);

                transformedStream = new MemoryStream(transformedContent.Length + 54);
                using StreamWriter writer = new(transformedStream, leaveOpen: true);
                writer.Write($"""<?xml version="1.0" encoding="UTF-8" standalone="no"?>{Environment.NewLine}""");
                writer.Write(transformedContent);
                writer.Flush();
                transformedStream.Seek(0, SeekOrigin.Begin);

                stream = transformedStream;
            }

            XmpMetadata result = new();
            XmpMetadataXmlReadHandler handler = new(result, _options.Logger);

            XmlParser.Parse(stream, ref handler);

            return result;
        }
        finally
        {
            transformedStream?.Dispose();
        }
    }

    [GeneratedRegex("\\s*<\\?xpacket.*?\\?>\\s*", RegexOptions.ExplicitCapture | RegexOptions.Compiled)]
    private static partial Regex PacketInstructions();
}
