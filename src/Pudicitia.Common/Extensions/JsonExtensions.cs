using Newtonsoft.Json;

namespace Pudicitia.Common.Extensions;

public static class JsonExtensions
{
    public static string ToJson(this object value)
    {
        return JsonConvert.SerializeObject(value);
    }

    public static T? ToObject<T>(this string value)
    {
        return JsonConvert.DeserializeObject<T>(value);
    }

    public static object? ToObject(this string value, Type type)
    {
        return JsonConvert.DeserializeObject(value, type);
    }
}
