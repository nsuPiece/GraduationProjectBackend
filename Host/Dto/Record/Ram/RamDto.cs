namespace Host.Dto.Record.Ram;

public class RamDto
{

    public long _ts { get; set; }
    /// <summary>
    /// 主机名
    /// </summary>
    public string HostName { get; set; }    


    /// <summary>
    /// 总内存
    /// </summary>
    public double TotalMemory { get; set; }
    /// <summary>
    /// 已用内存
    /// </summary>
    public double UsedMemory { get; set; }
    /// <summary>
    /// 可用内存
    /// </summary>
    public double FreeMemory { get; set; }

    /// <summary>
    /// 使用率
    /// </summary>
    public double RamUsage { get; set; }
}
