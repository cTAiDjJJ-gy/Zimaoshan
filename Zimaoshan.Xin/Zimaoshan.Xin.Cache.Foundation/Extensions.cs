using StackExchange.Redis;
using System.Reflection;
using System.Text.Json;

namespace Zimaoshan.Xin.Cache.Foundation;

internal static class Extensions
{
    /// <summary>
    /// 序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static byte[] Serialize<T>(this T value)
    {
        using var ms = new MemoryStream();
        var utf8JsonWriter = new Utf8JsonWriter(ms);
        JsonSerializer.Serialize(utf8JsonWriter, value);
        return ms.ToArray();
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T? Deserialize<T>(this RedisValue value)
    {
        if (value.IsNullOrEmpty) return default(T?);

        byte[] bytes = value!;
        return bytes.Deserilize<T>();
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static T? Deserilize<T>(this byte[] bytes)
    {
        var span = new ReadOnlySpan<byte>(bytes, 0, bytes.Length);
        return JsonSerializer.Deserialize<T>(span);
    }

    /// <summary>
    /// 获取程序集名称
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static string GetFriendlyAssemblyName(this Assembly assembly)
    {
        var fullName = assembly.FullName;
        var name = fullName![..fullName!.IndexOf(',')];
        return name;
    }
}
