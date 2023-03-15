using Microsoft.Extensions.Configuration;

namespace Core.Extensions;

/// <summary>
/// 选项扩展
/// </summary>
public static class OptionsExtension
{
    /// <summary>
    /// 获取配置信息
    /// </summary>
    /// <typeparam name="T">配置选项</typeparam>
    /// <returns></returns>
    public static T GetOptions<T>(this IConfiguration configuration) where T : class, new()
    {
        var name = typeof(T).Name;
        var key = name[..name.IndexOf("options", StringComparison.OrdinalIgnoreCase)];

        return GetSection<T>(configuration, key);
    }

    /// <summary>
    /// 获取配置信息
    /// </summary>
    /// <typeparam name="T">配置选项</typeparam>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static T GetSection<T>(this IConfiguration configuration, string key)
    {
        return configuration.GetSection(key).Get<T>();
    }
}
