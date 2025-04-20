using System.Globalization;

namespace FacturXDotNet.API.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
class BuildDateAttribute : Attribute
{
    public BuildDateAttribute(string value)
    {
        BuildDate = DateTimeOffset.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.None);
    }

    public DateTimeOffset BuildDate { get; }
}
