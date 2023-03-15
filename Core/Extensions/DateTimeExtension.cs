using System.Globalization;

namespace Core.Extensions;

/// <summary>
/// DateTime 扩展
/// </summary>
public static class DateTimeExtension
{
    /// <summary>
    /// DateTime 转 多久前
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string FromNow(this DateTime date)
    {
        TimeSpan span = DateTime.Now - date;
        var days = span.TotalDays;

        if (days > 365)
        {
            return string.Format("{0}年前", (int)Math.Floor(days / 365));
        }
        else if (days > 30)
        {
            return string.Format("{0}月前", (int)Math.Floor(days / 30));
        }
        else if (days > 7)
        {
            return string.Format("{0}周前", (int)Math.Floor(days / 7));
        }
        else if (days > 1)
        {
            return string.Format("{0}天前", (int)Math.Floor(days));
        }
        else if (span.TotalHours > 1)
        {
            return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
        }
        else if (span.TotalMinutes > 1)
        {
            return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
        }
        else if (span.TotalSeconds >= 1)
        {
            return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
        }
        else
        {
            return "1秒前";
        }
    }

    /// <summary>
    /// 时间戳转换为时间
    /// </summary>
    /// <param name="timeStamp">时间戳</param>
    /// <returns></returns>
    public static DateTime ToDateTime(this long timeStamp) => new DateTime(1970, 1, 1, 8, 0, 0).AddMilliseconds(timeStamp);

    /// <summary>
    /// DateTime时间格式转换为Unix时间戳格式
    /// </summary>
    /// <param name="date"> DateTime时间格式</param>
    /// <returns>Unix时间戳格式</returns>
    public static long ToTimeStamp(this DateTime date)
    {
        var timeSpan = date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);

        return (long)timeSpan.TotalMilliseconds;
    }

    /// <summary>
    /// 转换为通用时间格式
    /// </summary>
    /// <param name="date">要转换的时间</param>
    /// <param name="Format">日期格式化格式</param>
    /// <returns>yyyy-MM-dd hh:mm:ss</returns>
    public static string ToCommonString(this DateTime date, string format = "yyyy-MM-dd HH:mm:ss")=> date.ToString(format);

    /// <summary>
    /// 转换为通用日期格式
    /// </summary>
    /// <param name="date">要转换的时间</param>
    /// <param name="Format">日期格式化格式</param>
    /// <returns>yyyy-MM-dd hh:mm:ss</returns>
    public static string ToCommonDateString(this DateTime date, string format = "yyyy-MM-dd")=> date.ToString(format);

    /// <summary>
    /// 转换为通用时间格式
    /// </summary>
    /// <param name="date">要转换的时间</param>
    /// <returns>yyyy-MM-dd hh:mm:ss</returns>
    public static DateTime ToDateTime(this string date)=> DateTime.Parse(date);

    /// <summary>  
    /// 获取当前时间戳  
    /// </summary>
    /// <returns></returns>  
    public static long GetNowTimeStamp(this DateTime _)=> ToTimeStamp(DateTime.Now);

    /// <summary>  
    /// 获取当前时间
    /// </summary>  
    /// <returns></returns>  
    public static string GetNowTime() => ToCommonString(DateTime.Now);

    /// <summary>
    /// 获取星期几
    /// </summary>
    /// <param name="date">日期</param>
    /// <returns></returns>
    public static string ToWeekName(this DateTime date) => CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(date.DayOfWeek);

    /// <summary>
    /// 获取某个月的开始时间和结束时间
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static (DateTime, DateTime) MonthRange(this DateTime date) 
    { 
        // 月初
        var beginDate = date.Date.AddDays(1 - date.Day);
        // 月末
        var endDate = beginDate.AddMonths(1).AddSeconds(-1);

        return (beginDate, endDate);
    }

    /// <summary>
    /// 获取某周的开始时间和结束时间
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static (DateTime, DateTime) WeekRange(this DateTime date)
    {
        int dayOfWeek = -1 * (int)date.Date.DayOfWeek;
        //Sunday = 0,Monday = 1,Tuesday = 2,Wednesday = 3,Thursday = 4,Friday = 5,Saturday = 6,

        var weekStartTime = date.AddDays(dayOfWeek + 1);//取本周一
        if (dayOfWeek == 0) //如果今天是周日，则开始时间是上周一
            weekStartTime = weekStartTime.AddDays(-7);

        var beginDate = weekStartTime.Date;

        return (beginDate, beginDate.AddDays(7));
    }
}
