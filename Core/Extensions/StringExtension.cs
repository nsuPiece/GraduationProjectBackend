namespace Core.Extensions;

/// <summary>
/// 字符串扩展
/// </summary>
public static class StringExtension
{
    /// <summary>
    /// 是否空或空格
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

    /// <summary>
    /// 是否空或""
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

    /// <summary>
    /// 字符串三元表达式
    /// </summary>
    /// <typeparam name="T">返回的数据类型</typeparam>
    /// <param name="str">要判断的字符串</param>
    /// <param name="t1">如果为空返回<paramref name="t1"/></param>
    /// <param name="t2">如果不为空返回<paramref name="t2"/></param>
    /// <returns></returns>
    public static T EmptyReplace<T>(this string str, T t1, T t2) => str.IsNullOrEmpty() ? t1 : t2;

    /// <summary>
    /// 是否不为空
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static bool IsNotEmpty(this string str) => !str.IsNullOrEmpty();

    /// <summary>
    /// 将 8 位数的数字时间转换为 日期格式
    /// </summary>
    /// <param name="dateNum"></param>
    /// <returns></returns>
    public static DateTime NumToDate(this string dateNum)
    {
        if (dateNum.Length != 8)
            return DateTime.MinValue;

        return DateTime.ParseExact(dateNum, "yyyyMMdd", null);
    }

    /// <summary>
    /// 时间字符串格式转时间
    /// </summary>
    /// <param name="dateString">时间字符串</param>
    /// <returns></returns>
    public static DateTime ToDate(this string dateString)
    {
        if (DateTime.TryParse(dateString, out DateTime dateTime))
            return dateTime;

        throw new Exception("日期格式不合法");
    }

    /// <summary>
    /// 字符串小驼峰命名
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static string SmallHump(this string str)
    {
        if (str.IsNullOrEmpty())
            return str;

        if (str.Length == 1)
            return str.ToLower();

        try
        {
            return str[0] + str[1..];
        }
        catch (Exception)
        {
            return str;
        }
    }

    /// <summary>
    /// 字符串非空判断
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="replaceVal">如果为空，需要替换的值</param>
    /// <returns></returns>
    public static string EmptyReplace(this string str, string replaceVal) => str.IsNullOrWhiteSpace() ? replaceVal : str;

    /// <summary>
    /// 字符串转 Guid
    /// </summary>
    /// <param name="str">Guid字符串</param>
    /// <returns></returns>
    public static Guid ToGuid(this string str)
    {
        if (Guid.TryParse(str, out Guid guid))
            return guid;

        throw new FormatException("Guid格式不合法");
    }

    /// <summary>
    /// 格式化文件大小
    /// </summary>
    /// <param name="fileSize">文件的大小</param>
    /// <returns>格式化后的值</returns>
    public static string FormatFileSize(this long fileSize)
    {
        if (fileSize < 0)
            return "0";

        else if (fileSize >= 1024 * 1024 * 1024) //文件大小大于或等于1024MB
            return string.Format("{0:0.00} GB", fileSize / (1024 * 1024 * 1024m));

        else if (fileSize >= 1024 * 1024) //文件大小大于或等于1024KB
            return string.Format("{0:0.00} MB", fileSize / (1024 * 1024m));

        else if (fileSize >= 1024) //文件大小大于等于1024bytes
            return string.Format("{0:0.00} KB", fileSize / 1024m);

        else
            return string.Format("{0:0.00} Byte", fileSize);
    }
}
