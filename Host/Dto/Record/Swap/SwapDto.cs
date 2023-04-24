namespace Host.Dto.Record.Swap;

public class SwapDto
{

    public long _ts { get; set; }
    /// <summary>
    /// 主机名
    /// </summary>
    public string HostName { get; set; }

    /// <summary>
    /// 交换区总量
    /// </summary>
    public double AllocatedBaseSize { get; set; }
    /// <summary>
    /// /交换区使用量
    /// </summary>
    public double CurrentUsage { get; set; }
    /// <summary>
    /// 交换区可用量
    /// </summary>
    public double FreeUsage { get; set; }
    /// <summary>
    /// 交换区使用率
    /// </summary>
    public double SwapUsage { get; set; }
}
