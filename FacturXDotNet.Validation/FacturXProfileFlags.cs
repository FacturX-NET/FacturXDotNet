namespace FacturXDotNet.Validation;

/// <summary>
///     Represents the different Factur-X profiles as a set of flags.
///     This enumeration is the flags representation of the <see cref="GuidelineSpecifiedDocumentContextParameterId" /> enumeration.
/// </summary>
/// <seealso cref="GuidelineSpecifiedDocumentContextParameterId" />
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
    /// <seealso cref="GuidelineSpecifiedDocumentContextParameterId.Minimum" />
    Minimum = 1<<0,

    /// <summary>
    ///     The BASIC WL profile.
    /// </summary>
    /// <seealso cref="GuidelineSpecifiedDocumentContextParameterId.BasicWl" />
    BasicWl = 1<<1,

    /// <summary>
    ///     The BASIC profile.
    /// </summary>
    /// <seealso cref="GuidelineSpecifiedDocumentContextParameterId.Basic" />
    Basic = 1<<2,

    /// <summary>
    ///     The EN 16931 profile.
    /// </summary>
    /// <seealso cref="GuidelineSpecifiedDocumentContextParameterId.En16931" />
    En16931 = 1<<3,

    /// <summary>
    ///     The EXTENDED profile.
    /// </summary>
    /// <seealso cref="GuidelineSpecifiedDocumentContextParameterId.Extended" />
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
    public static bool Match(this FacturXProfileFlags flags, GuidelineSpecifiedDocumentContextParameterId profile) =>
        profile switch
        {
            GuidelineSpecifiedDocumentContextParameterId.Minimum => flags.HasFlag(FacturXProfileFlags.Minimum),
            GuidelineSpecifiedDocumentContextParameterId.BasicWl => flags.HasFlag(FacturXProfileFlags.BasicWl),
            GuidelineSpecifiedDocumentContextParameterId.Basic => flags.HasFlag(FacturXProfileFlags.Basic),
            GuidelineSpecifiedDocumentContextParameterId.En16931 => flags.HasFlag(FacturXProfileFlags.En16931),
            GuidelineSpecifiedDocumentContextParameterId.Extended => flags.HasFlag(FacturXProfileFlags.Extended),
            _ => false
        };

    /// <summary>
    ///     Return a new <see cref="FacturXProfileFlags" /> that has all the flags that are lower or equal to the current flags.
    /// </summary>
    public static FacturXProfileFlags AndLower(this FacturXProfileFlags flags)
    {
        if (flags.HasFlag(FacturXProfileFlags.Extended))
        {
            return FacturXProfileFlags.All;
        }

        if (flags.HasFlag(FacturXProfileFlags.En16931))
        {
            return FacturXProfileFlags.Minimum | FacturXProfileFlags.BasicWl | FacturXProfileFlags.Basic | FacturXProfileFlags.En16931;
        }

        if (flags.HasFlag(FacturXProfileFlags.Basic))
        {
            return FacturXProfileFlags.Minimum | FacturXProfileFlags.BasicWl | FacturXProfileFlags.Basic;
        }

        if (flags.HasFlag(FacturXProfileFlags.BasicWl))
        {
            return FacturXProfileFlags.Minimum | FacturXProfileFlags.BasicWl;
        }

        if (flags.HasFlag(FacturXProfileFlags.Minimum))
        {
            return FacturXProfileFlags.Minimum;
        }

        return FacturXProfileFlags.None;
    }

    /// <summary>
    ///     Return a new <see cref="FacturXProfileFlags" /> that has all the flags that are higher or equal to the current flags.
    /// </summary>
    public static FacturXProfileFlags AndHigher(this FacturXProfileFlags flags)
    {
        if (flags.HasFlag(FacturXProfileFlags.Minimum))
        {
            return FacturXProfileFlags.All;
        }

        if (flags.HasFlag(FacturXProfileFlags.BasicWl))
        {
            return FacturXProfileFlags.BasicWl | FacturXProfileFlags.Basic | FacturXProfileFlags.En16931 | FacturXProfileFlags.Extended;
        }

        if (flags.HasFlag(FacturXProfileFlags.Basic))
        {
            return FacturXProfileFlags.Basic | FacturXProfileFlags.En16931 | FacturXProfileFlags.Extended;
        }

        if (flags.HasFlag(FacturXProfileFlags.En16931))
        {
            return FacturXProfileFlags.En16931 | FacturXProfileFlags.Extended;
        }

        if (flags.HasFlag(FacturXProfileFlags.Extended))
        {
            return FacturXProfileFlags.Extended;
        }

        return FacturXProfileFlags.None;
    }

    /// <summary>
    ///     Return the smallest flag in the given flags.
    /// </summary>
    public static FacturXProfileFlags GetMinProfile(this FacturXProfileFlags flags)
    {
        if (flags.HasFlag(FacturXProfileFlags.Minimum))
        {
            return FacturXProfileFlags.Minimum;
        }

        if (flags.HasFlag(FacturXProfileFlags.BasicWl))
        {
            return FacturXProfileFlags.BasicWl;
        }

        if (flags.HasFlag(FacturXProfileFlags.Basic))
        {
            return FacturXProfileFlags.Basic;
        }

        if (flags.HasFlag(FacturXProfileFlags.En16931))
        {
            return FacturXProfileFlags.En16931;
        }

        if (flags.HasFlag(FacturXProfileFlags.Extended))
        {
            return FacturXProfileFlags.Extended;
        }

        return FacturXProfileFlags.None;
    }

    /// <summary>
    ///     Return the largest flag in the given flags.
    /// </summary>
    /// <param name="flags"></param>
    /// <returns></returns>
    public static FacturXProfileFlags GetMaxProfile(this FacturXProfileFlags flags)
    {
        if (flags.HasFlag(FacturXProfileFlags.Extended))
        {
            return FacturXProfileFlags.Extended;
        }

        if (flags.HasFlag(FacturXProfileFlags.En16931))
        {
            return FacturXProfileFlags.En16931;
        }

        if (flags.HasFlag(FacturXProfileFlags.Basic))
        {
            return FacturXProfileFlags.Basic;
        }

        if (flags.HasFlag(FacturXProfileFlags.BasicWl))
        {
            return FacturXProfileFlags.BasicWl;
        }

        if (flags.HasFlag(FacturXProfileFlags.Minimum))
        {
            return FacturXProfileFlags.Minimum;
        }

        return FacturXProfileFlags.None;
    }
}
