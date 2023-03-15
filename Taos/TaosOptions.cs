namespace Taos;

/// <summary>
/// 涛思数据库配置
/// </summary>
public class TaosOptions
{
    /// <summary>
    /// 地址
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// 端口
    /// </summary>
    public short Port { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 数据库
    /// </summary>
    public string Database { get; set; }
}
