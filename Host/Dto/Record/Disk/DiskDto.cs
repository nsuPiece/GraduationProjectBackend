namespace Host.Dto.Record.Disk;

public class DiskDto
{
    public long _ts { get; set; }
    /// <summary>
    /// 主机名
    /// </summary>
    public string HostName { get; set; }


    /// <summary>
    /// 总空间
    /// </summary>
    public double TotalSpace { get; set; }
    /// <summary>
    /// 可用空间
    /// </summary>
    public double AvailableSpace { get; set; }
    /// <summary>
    /// 已用空间
    /// </summary>
    public double UsedSpace { get; set; }
    /// <summary>
    /// 磁盘使用率
    /// </summary>
    public double DiskUsage { get; set; }
}
