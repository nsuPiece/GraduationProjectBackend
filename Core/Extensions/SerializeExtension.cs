using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Extensions;

namespace Core.Extensions;

/// <summary>
/// 序列化操作
/// </summary>
public static class SerializeExtension
{
    /// <summary>
    /// JSON 序列化选项
    /// </summary>
    /// <returns></returns>
    private static JsonSerializerOptions SerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            //解决中文序列化被编码的问题
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            // 字段序列化
            IncludeFields = true,

            PropertyNameCaseInsensitive = true,
        };
        //解决时间格式序列化的问题
        options.Converters.Add(new DateTimeConverter());
        options.Converters.Add(new DateTimeNullableConverter());

        return options;
    }

    /// <summary>
    /// 实体对象转JSON字符串
    /// </summary>
    /// <param name="obj">实体对象</param>
    /// <returns></returns>
    public static string Serialize<T>(this T obj) => JsonSerializer.Serialize(obj, SerializerOptions());

    /// <summary>
    /// Stream 序列化
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    /// <param name="utf8Json">JSON字符串流</param>
    /// <param name="value">返回值</param>
    /// <returns></returns>
    public static async Task SerializeAsync<T>(this Stream utf8Json, T value)
    {
        await JsonSerializer.SerializeAsync(utf8Json, value, SerializerOptions());
    }

    /// <summary>
    ///JSON字符串转对象
    /// </summary>
    /// <param name="json">JSON字符串</param>
    /// <returns></returns>
    public static T Deserialize<T>(this string json) => JsonSerializer.Deserialize<T>(json, SerializerOptions());

    /// <summary>
    /// Span JSON 字符串转对象
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    /// <param name="utf8Json">json值</param>
    /// <returns></returns>
    public static T Deserialize<T>(this ReadOnlySpan<byte> utf8Json) => JsonSerializer.Deserialize<T>(utf8Json, SerializerOptions());

    /// <summary>
    /// Span JSON 字符串转对象
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    /// <param name="stream">文件流</param>
    /// <returns></returns>
    public static T Deserialize<T>(this Stream stream)
    {
        stream.Position = 0;
        return JsonSerializer.Deserialize<T>(stream, SerializerOptions());
    }

    /// <summary>
    /// Span JSON 字符串转对象
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    /// <param name="stream">文件流</param>
    /// <returns></returns>
    public static ValueTask<T> DeserializeAsync<T>(this Stream stream) => JsonSerializer.DeserializeAsync<T>(stream, SerializerOptions());
}

/// <summary>
/// 日期格式化
/// </summary>
public class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.Parse(reader.GetString() ?? "");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToCommonString());
    }
}

/// <summary>
/// 空日期转换
/// </summary>
public class DateTimeNullableConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return string.IsNullOrEmpty(reader.GetString()) ? default(DateTime?) : DateTime.Parse(reader.GetString() ?? "");
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToCommonString());
    }
}
