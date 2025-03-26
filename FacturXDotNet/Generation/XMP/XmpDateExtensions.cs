namespace FacturXDotNet.Generation.XMP;

static class XmpDateExtensions
{
    public static string FormatXmpDate(this DateTimeOffset date)
    {
        if (date.TimeOfDay == TimeSpan.Zero)
        {
            return date.ToString("yyyy-MM-dd");
        }

        if (date.Offset == TimeSpan.Zero)
        {
            DateTime dateTime = date.UtcDateTime;
            if (dateTime.Millisecond == 0)
            {
                return dateTime.ToString("yyyy-MM-ddTHH:mm:ssK");
            }

            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.ffffffK");
        }

        if (date.Millisecond == 0)
        {
            return date.ToString("yyyy-MM-ddTHH:mm:ssK");
        }

        return date.ToString("yyyy-MM-ddTHH:mm:ss.ffffffK");
    }
}
