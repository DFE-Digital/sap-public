using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace SAPPub.Web.Helpers;

public static class TempDataHelper
{
    public static void Set<T>(this ITempDataDictionary tempData, string key, T value)
    {
        if (tempData != null)
        {
            tempData[key] = JsonSerializer.Serialize(value);
        }
    }

    public static T? Get<T>(this ITempDataDictionary tempData, string key)
    {
        if (tempData == null)
            return default;

        tempData.TryGetValue(key, out var returnValue);
        return returnValue == null ? default : JsonSerializer.Deserialize<T>(returnValue.ToString()!);
    }

    public static T? Peek<T>(this ITempDataDictionary tempData, string key)
    {
        if (tempData == null)
            return default;

        var returnValue = tempData.Peek(key);

        return returnValue == null ? default : JsonSerializer.Deserialize<T>(returnValue.ToString()!);
    }
}
