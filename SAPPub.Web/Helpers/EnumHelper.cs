using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SAPPub.Web.Helpers;

public static class EnumHelper
{
    public static string? GetDisplayName(this Enum enumValue)
    {
        var displayAttribute = enumValue.GetType()
            .GetMember(enumValue.ToString())
            .Single()
            .GetCustomAttribute<DisplayAttribute>();

        return displayAttribute is null ? enumValue.ToString() : displayAttribute.GetName();
    }
}
