namespace FacturXDotNet;

/// <summary>
///     Code list for the format of a date, when the date only contains the day, month and year.
/// </summary>
/// <remarks>
///     This enumeration only has one value because there is only one possible code when the date only contains the day, month and year. However, this enumeration is still useful to
///     make the code more readable.
/// </remarks>
public enum DateOnlyFormat
{
    DateOnly = 102
}

public static class DateOnlyFormatMappingExtensions
{
    public static int ToDateOnlyFormat(this DateOnlyFormat value) =>
        value switch
        {
            DateOnlyFormat.DateOnly => 102,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

    public static DateOnlyFormat? ToDateOnlyFormatOrNull(this int value) =>
        value switch
        {
            102 => DateOnlyFormat.DateOnly,
            _ => null
        };

    public static DateOnlyFormat ToDateOnlyFormat(this int value) => value.ToDateOnlyFormatOrNull() ?? throw new ArgumentOutOfRangeException(nameof(DateOnlyFormat), value, null);
}
