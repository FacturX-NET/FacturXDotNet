using FacturXDotNet.Models;

namespace FacturXDotNet.Validation.Common;

/// <summary>
///     Represents the different Factur-X profiles as a set of flags.
/// </summary>
[Flags]
public enum FacturXProfileFlags
{
    None = 0,

    Minimum = 1<<0,
    BasicWl = 1<<1,
    Basic = 1<<2,
    En16931 = 1<<3,
    Extended = 1<<4,

    All = Minimum | BasicWl | Basic | En16931 | Extended
}

public static class ProfileFlagsExtensions
{
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

    public static FacturXProfileFlags AndLower(this FacturXProfileFlags flags) =>
        flags switch
        {
            FacturXProfileFlags.None => FacturXProfileFlags.None,
            FacturXProfileFlags.Minimum => FacturXProfileFlags.Minimum,
            FacturXProfileFlags.BasicWl => FacturXProfileFlags.Minimum | FacturXProfileFlags.BasicWl,
            FacturXProfileFlags.Basic => FacturXProfileFlags.Minimum | FacturXProfileFlags.BasicWl | FacturXProfileFlags.Basic,
            FacturXProfileFlags.En16931 => FacturXProfileFlags.Minimum | FacturXProfileFlags.BasicWl | FacturXProfileFlags.Basic | FacturXProfileFlags.En16931,
            FacturXProfileFlags.Extended => FacturXProfileFlags.All,
            FacturXProfileFlags.All => FacturXProfileFlags.All,
            _ => throw new ArgumentOutOfRangeException(nameof(flags), flags, null)
        };

    public static FacturXProfileFlags AndHigher(this FacturXProfileFlags flags) =>
        flags switch
        {
            FacturXProfileFlags.None => FacturXProfileFlags.All,
            FacturXProfileFlags.Minimum => FacturXProfileFlags.All,
            FacturXProfileFlags.BasicWl => FacturXProfileFlags.BasicWl | FacturXProfileFlags.Basic | FacturXProfileFlags.En16931 | FacturXProfileFlags.Extended,
            FacturXProfileFlags.Basic => FacturXProfileFlags.Basic | FacturXProfileFlags.En16931 | FacturXProfileFlags.Extended,
            FacturXProfileFlags.En16931 => FacturXProfileFlags.En16931 | FacturXProfileFlags.Extended,
            FacturXProfileFlags.Extended => FacturXProfileFlags.Extended,
            FacturXProfileFlags.All => FacturXProfileFlags.All,
            _ => throw new ArgumentOutOfRangeException(nameof(flags), flags, null)
        };

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
