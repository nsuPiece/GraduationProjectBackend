using System.ComponentModel;
using System.Reflection;
using Core.Extensions;

namespace Core.Extension;

/// <summary>
/// KeyValue模型
/// </summary>
[Serializable]
public class EnumKeyValue
{
    public EnumKeyValue(string key, int value, string description)
    {
        Key = key;
        Value = value;
        Description = description;
    }

    /// <summary>
    /// 键
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// 值
    /// </summary>
    public int Value { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }
}

/// <summary>
/// 枚举扩展
/// </summary>
public static class EnumExtension
{
    /// <summary>
    /// 枚举描述缓存
    /// </summary>
    private static readonly Dictionary<string, List<EnumKeyValue>> EnumDescriptions = new();

    /// <summary>
    /// 初始化缓存枚举
    /// </summary>
    /// <param name="types"></param>
    public static void InitCaches(IEnumerable<Type> types)
    {
        if (types.IsExists())
            foreach (var type in types)
                GetDescriptions(type);
    }

    /// <summary>
    /// 获取所有的枚举
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, List<EnumKeyValue>> GetAllEnums() => EnumDescriptions;

    /// <summary>
    /// 枚举转字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="EnumVal"></param>
    /// <returns></returns>
    public static string EnumToString<T>(this object EnumVal) where T : Enum
    {
        return Enum.GetName(typeof(T), EnumVal);
    }

    /// <summary>
    /// 字符串转枚举
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="EnumVal"></param>
    /// <returns></returns>
    public static T ToEnum<T>(this string EnumVal) where T : Enum
    {
        try
        {
            return (T)Enum.Parse(typeof(T), EnumVal);
        }
        catch (Exception)
        {
            return default;
        }
    }

    /// <summary>
    /// 数字转枚举
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Num"></param>
    /// <returns></returns>
    public static T ToEnum<T>(this int Num) where T : Enum
    {
        if (Enum.IsDefined(typeof(T), Num))
            return (T)Enum.ToObject(typeof(T), Num);

        return default;
    }

    /// <summary>
    /// 获取所有枚举值描述
    /// </summary>
    /// <typeparam name="T">枚举</typeparam>
    /// <param name="enumType">枚举类型</param>
    /// <returns></returns>
    public static IEnumerable<EnumKeyValue> GetDescriptions(this Type enumType)
    {
        if (!enumType.IsEnum)
            throw new Exception($"{enumType.Name} 不是枚举");

        var name = enumType.FullName;

        if (EnumDescriptions.TryGetValue(name, out var value))
            return value;

        var descriptions = new List<EnumKeyValue>();
        var fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);

        foreach (var field in fields)
        {
            var description = field.GetCustomAttribute<DescriptionAttribute>();
            descriptions.Add(new EnumKeyValue(field.Name, (int)field.GetValue(null), description?.Description));
        }

        // 加入缓存
        EnumDescriptions[name] = descriptions;

        return descriptions;
    }

    /// <summary>
    /// 获取某个枚举值的描述
    /// </summary>
    /// <param name="enumVal"></param>
    /// <returns></returns>
    public static string GetDescription(this Enum enumVal)
    {
        return enumVal.GetType().GetDescriptions().FirstOrDefault(x => x.Key == enumVal.ToString())?.Description;
    }
}
