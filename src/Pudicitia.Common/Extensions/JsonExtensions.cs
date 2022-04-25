using System.Text.Json;

namespace Pudicitia.Common.Extensions;

public static class JsonExtensions
{
    public static string ToJson(this object value)
    {
        return JsonSerializer.Serialize(value);
    }

    public static byte[] ToUtf8Bytes(this object value)
    {
        return JsonSerializer.SerializeToUtf8Bytes(value);
    }

    public static T? ToObject<T>(this string value)
    {
        return JsonSerializer.Deserialize<T>(value);
    }

    public static object? ToObject(this string value, Type type)
    {
        return JsonSerializer.Deserialize(value, type);
    }

    public static object? ToObject(this ReadOnlySpan<byte> value, Type type)
    {
        return JsonSerializer.Deserialize(value, type);
    }
}
