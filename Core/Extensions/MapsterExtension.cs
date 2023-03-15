using Core.Extension;
using Mapster;


namespace Core.Extensions;

/// <summary>
/// Mapster扩展
/// </summary>
public static class MapsterExtension
{
    /// <summary>
    /// 添加默认Mapster配置
    /// </summary>
    public static void AddDefaultMapper()
    {
        // 枚举转字符串
        TypeAdapterConfig<Enum, string>.NewConfig().MapWith(s => s.GetDescription());

        // 日期格式转字符串
        TypeAdapterConfig<DateTime, string>.NewConfig().MapWith(s => s.ToString("yyyy-MM-dd HH:mm:ss"));

        // 可空日期格式转字符串
        TypeAdapterConfig<DateTime?, string>.NewConfig().MapWith(s => s != null ? s.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");
    }
}
