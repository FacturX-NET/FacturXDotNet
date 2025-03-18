namespace FacturXDotNet.Models.Validation.Utils;

static class DecimalExtensions
{
    /// <summary>
    ///     Compute the number of decimals in the <see cref="decimal" /> value.
    /// </summary>
    /// <remarks>
    ///     See https://stackoverflow.com/a/15151526/26358508
    /// </remarks>
    public static byte CountDecimals(this decimal value)
    {
        // From Microsoft documentation about decimal.GetBits:
        // > The fourth element of the returned array contains the scale factor and sign. It consists of the following parts:
        // > ...
        // > - Bits 16 to 23 must contain an exponent between 0 and 28, which indicates the power of 10 to divide the integer number.
        // > ...
        // https://learn.microsoft.com/en-us/dotnet/api/system.decimal.getbits?view=net-9.0&redirectedfrom=MSDN#System_Decimal_GetBits_System_Decimal_

        int[] bits = decimal.GetBits(value);
        int scaleFactorAndSign = bits[3];
        return (byte)(scaleFactorAndSign>> 16 & 0b1111111);
    }
}
