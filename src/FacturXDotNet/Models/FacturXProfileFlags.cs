namespace FacturXDotNet.Models;

/// <summary>
///     Represents the different Factur-X profiles as a set of flags.
///     This enumeration is the flags representation of the <see cref="FacturXProfile" /> enumeration.
/// </summary>
/// <seealso cref="FacturXProfile" />
[Flags]
public enum FacturXProfileFlags
{
    /// <summary>
    ///     No profile.
    /// </summary>
    None = 0,

    /// <summary>
    ///     The minimum profile.
    /// </summary>
    /// <seealso cref="FacturXProfile.Minimum" />
    Minimum = 1<<0,

    /// <summary>
    ///     The BASIC WL profile.
    /// </summary>
    /// <seealso cref="FacturXProfile.BasicWl" />
    BasicWl = 1<<1,

    /// <summary>
    ///     The BASIC profile.
    /// </summary>
    /// <seealso cref="FacturXProfile.Basic" />
    Basic = 1<<2,

    /// <summary>
    ///     The EN 16931 profile.
    /// </summary>
    /// <seealso cref="FacturXProfile.En16931" />
    En16931 = 1<<3,

    /// <summary>
    ///     The EXTENDED profile.
    /// </summary>
    /// <seealso cref="FacturXProfile.Extended" />
    Extended = 1<<4,

    /// <summary>
    ///     All profiles.
    /// </summary>
    All = Minimum | BasicWl | Basic | En16931 | Extended
}

/// <summary>
///     Extension methods for the <see cref="FacturXProfileFlags" /> enumeration.
/// </summary>
public static class ProfileFlagsExtensions
{
    /// <summary>
    ///     Determines whether the flags contain the specified profile.
    /// </summary>
    public static bool Match(this FacturXProfileFlags flags, FacturXProfile profile) =>
        profile switch
        {
            FacturXProfile.Minimum => flags.HasFlag(FacturXProfileFlags.Minimum),
            FacturXProfile.BasicWl => flags.HasFlag(FacturXProfileFlags.BasicWl),
            FacturXProfile.Basic => flags.HasFlag(FacturXProfileFlags.Basic),
            FacturXProfile.En16931 => flags.HasFlag(FacturXProfileFlags.En16931),
            FacturXProfile.Extended => flags.HasFlag(FacturXProfileFlags.Extended),
            _ => false
        };

    /// <summary>
    ///     Return a <see cref="FacturXProfileFlags" /> that has all the flags that are lower or equal to the current flags.
    /// </summary>
    public static FacturXProfileFlags AndLower(this FacturXProfile flags) =>
        flags switch
        {
            FacturXProfile.Extended => FacturXProfileFlags.All,
            FacturXProfile.En16931 => FacturXProfileFlags.Minimum | FacturXProfileFlags.BasicWl | FacturXProfileFlags.Basic | FacturXProfileFlags.En16931,
            FacturXProfile.Basic => FacturXProfileFlags.Minimum | FacturXProfileFlags.BasicWl | FacturXProfileFlags.Basic,
            FacturXProfile.BasicWl => FacturXProfileFlags.Minimum | FacturXProfileFlags.BasicWl,
            FacturXProfile.Minimum => FacturXProfileFlags.Minimum,
            _ => FacturXProfileFlags.None
        };

    /// <summary>
    ///     Return a <see cref="FacturXProfileFlags" /> that has all the flags that are higher or equal to the current flags.
    /// </summary>
    public static FacturXProfileFlags AndHigher(this FacturXProfile flags) =>
        flags switch
        {
            FacturXProfile.Minimum => FacturXProfileFlags.All,
            FacturXProfile.BasicWl => FacturXProfileFlags.BasicWl | FacturXProfileFlags.Basic | FacturXProfileFlags.En16931 | FacturXProfileFlags.Extended,
            FacturXProfile.Basic => FacturXProfileFlags.Basic | FacturXProfileFlags.En16931 | FacturXProfileFlags.Extended,
            FacturXProfile.En16931 => FacturXProfileFlags.En16931 | FacturXProfileFlags.Extended,
            FacturXProfile.Extended => FacturXProfileFlags.Extended,
            _ => FacturXProfileFlags.None
        };

    /// <summary>
    ///     Return the smallest flag in the given flags.
    /// </summary>
    public static FacturXProfile GetMinProfile(this FacturXProfileFlags flags)
    {
        if (flags.HasFlag(FacturXProfileFlags.Minimum))
        {
            return FacturXProfile.Minimum;
        }

        if (flags.HasFlag(FacturXProfileFlags.BasicWl))
        {
            return FacturXProfile.BasicWl;
        }

        if (flags.HasFlag(FacturXProfileFlags.Basic))
        {
            return FacturXProfile.Basic;
        }

        if (flags.HasFlag(FacturXProfileFlags.En16931))
        {
            return FacturXProfile.En16931;
        }

        if (flags.HasFlag(FacturXProfileFlags.Extended))
        {
            return FacturXProfile.Extended;
        }

        return FacturXProfile.None;
    }

    /// <summary>
    ///     Return the largest flag in the given flags.
    /// </summary>
    /// <param name="flags"></param>
    /// <returns></returns>
    public static FacturXProfile GetMaxProfile(this FacturXProfileFlags flags)
    {
        if (flags.HasFlag(FacturXProfileFlags.Extended))
        {
            return FacturXProfile.Extended;
        }

        if (flags.HasFlag(FacturXProfileFlags.En16931))
        {
            return FacturXProfile.En16931;
        }

        if (flags.HasFlag(FacturXProfileFlags.Basic))
        {
            return FacturXProfile.Basic;
        }

        if (flags.HasFlag(FacturXProfileFlags.BasicWl))
        {
            return FacturXProfile.BasicWl;
        }

        if (flags.HasFlag(FacturXProfileFlags.Minimum))
        {
            return FacturXProfile.Minimum;
        }

        return FacturXProfile.None;
    }
}
