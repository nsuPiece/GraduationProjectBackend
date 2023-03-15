namespace Core.Extensions;

/// <summary>
/// 集合扩展
/// </summary>
public static class EnumerableExtension
{
    /// <summary>
    /// 字符串集合拼接成字符串
    /// </summary>
    /// <param name="source">字符串集合</param>
    /// <param name="separator">拼接符合</param>
    /// <returns>拼接后的字符串</returns>
    public static string JoinString(this IEnumerable<string> source, string separator) => string.Join(separator, source ?? List.Empty<string>());

    /// <summary>
    /// 给定条件是否过滤
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="source">数据集合</param>
    /// <param name="condition">是否执行</param>
    /// <param name="predicate">过滤条件</param>
    /// <returns过滤或不过滤后的数据</returns>
    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate) => condition ? source.Where(predicate) : source;

    /// <summary>
    /// 给定条件是否过滤
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="source">数据集合</param>
    /// <param name="condition">是否执行</param>
    /// <param name="predicate">过滤条件</param>
    /// <returns></returns>
    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, int, bool> predicate)=> condition ? source.Where(predicate) : source;

    /// <summary>
    /// 某个集合是否存在数据
    /// </summary>
    /// <typeparam name="T">数据对象</typeparam>
    /// <param name="source">数据集合</param>
    /// <param name="predicate">筛选条件</param>
    /// <returns></returns>
    public static bool IsExists<T>(this IEnumerable<T> source, Func<T, bool> predicate = null)
    {
        if (predicate != null)
            return source?.Any(predicate) ?? false;

        return source?.Any() ?? false;
    }
}

/// <summary>
/// List 扩展
/// </summary>
public static partial class List
{
    /// <summary>
    /// 空的集合
    /// </summary>
    /// <returns></returns>
    public static List<T> Empty<T>() => new();
}
