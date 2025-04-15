using PdfSharp.Drawing;
using PdfSharp.Fonts;

namespace FacturXDotNet.Generation.PDF.Generators.Standard;

class RobotoFontResolver : IFontResolver
{
    public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
    {
        if (familyName != "Roboto")
        {
            return null;
        }

        string fontName = italic
            ? bold ? "Roboto-BoldItalic" : "Roboto-Regular"
            : bold
                ? "Roboto-Bold"
                : "Roboto-Regular";

        return new FontResolverInfo(fontName, XStyleSimulations.None);
    }

    byte[]? IFontResolver.GetFont(string faceName) => ReadFont(faceName);

    static byte[]? ReadFont(string faceName)
    {
        Stream? fontStream = typeof(RobotoFontResolver).Assembly.GetManifestResourceStream($"FacturXDotNet.Resources.Fonts.Roboto.{faceName}.ttf");
        if (fontStream == null)
        {
            return null;
        }

        using Stream _ = fontStream;
        byte[] result = new byte[fontStream.Length];
        fontStream.ReadExactly(result);

        return result;
    }
}
