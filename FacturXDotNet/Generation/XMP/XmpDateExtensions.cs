namespace FacturXDotNet.Generation.XMP;

static class XmpDateExtensions
{
    public static string FormatXmpDate(this DateTime date)
    {
        if (date.TimeOfDay == TimeSpan.Zero)
        {
            return date.ToString("yyyy-MM-dd");
        }

        if (date is { Millisecond: 0, Microsecond: 0 })
        {
            return date.ToString("yyyy-MM-ddTHH:mm:sszzz");
        }

        if (date is { Millisecond: 0 })
        {
            return date.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz");
        }

        return date.ToString("yyyy-MM-ddTHH:mm:ss.ffffffzzz");
    }
}
