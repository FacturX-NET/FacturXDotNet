namespace FacturXDotNet.Models.CII;

/// <summary>
///     Code list for the format of a date, when the date only contains the day, month and year.
/// </summary>
/// <remarks>
///     This enumeration only has one value because there is only one possible code when the date only contains the day, month and year. However, this enumeration is still useful to
///     make the code more readable.
/// </remarks>
public enum DateOnlyFormat
{
    /// <summary>
    ///     Date that contains the day, month and year.
    /// </summary>
    DateOnly = 102
}

/// <summary>
///     Mapping methods for the <see cref="DateOnlyFormat" /> enumeration.
/// </summary>
public static class DateOnlyFormatMappingExtensions
{
    /// <summary>
    ///     Convert the <see cref="DateOnlyFormat" /> enumeration to its integer representation.
    /// </summary>
    public static int ToDateOnlyFormat(this DateOnlyFormat value) =>
        value switch
        {
            DateOnlyFormat.DateOnly => 102,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

    /// <summary>
    ///     Convert the integer to its <see cref="DateOnlyFormat" /> representation.
    /// </summary>
    /// <seealso cref="ToDateOnlyFormat(int)" />
    public static DateOnlyFormat? ToDateOnlyFormatOrNull(this int value) =>
        value switch
        {
            102 => DateOnlyFormat.DateOnly,
            _ => null
        };

    /// <summary>
    ///     Convert the integer to its <see cref="DateOnlyFormat" /> representation.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the integer does not match any <see cref="DateOnlyFormat" /> value.</exception>
    public static DateOnlyFormat ToDateOnlyFormat(this int value) => value.ToDateOnlyFormatOrNull() ?? throw new ArgumentOutOfRangeException(nameof(DateOnlyFormat), value, null);
}
