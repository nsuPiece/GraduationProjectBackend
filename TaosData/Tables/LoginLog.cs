using TaosData.Base;

namespace TaosData.Tables;

/// <summary>
/// 登录日志
/// </summary>
public class LoginLog : TaosBaseClass
{
    public override string STName { get; set; }
    public override string TName { get; set; }//子表为$"L{UserId}"
    public override TaosBaseTags Tags { get; set; }
    public override TaosBaseFields Fields { get; set; }

    public override long _ts { get; set; }

    public LoginLog()
    {
        STName = nameof(LoginLog);
        Tags = new LoginLogTags();
        Fields = new LoginLogFields();
    }
}

public class LoginLogTags : TaosBaseTags
{
    /// <summary>
    /// 用户编号
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }
}
public class LoginLogFields : TaosBaseFields
{
    /// <summary>
    /// 状态
    /// </summary>
    public string State { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// IP地址
    /// </summary>
    public string IpAddress { get; set; }

    /// <summary>
    /// 来源地址
    /// </summary>
    public string Referer { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    public string Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    public string OperatingSystem { get; set; }

    /// <summary>
    /// 浏览器信息
    /// </summary>
    public string UserAgent { get; set; }

}
