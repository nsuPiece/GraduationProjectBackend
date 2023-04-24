namespace Host.Dto.Record.Usage;

public class UsageDto
{

    public List<CpuUsageOutputDto> CpuUsage { get; set; }

    public List<RamUsageOutputDto> RamUsage { get; set; }

    public List<SwapUsageOutputDto> SwapUsage { get; set; }

    public List<DiskUsageOutputDto> DiskUsage { get; set; }
}

public class CpuUsageOutputDto
{
    public long _ts { get; set; }

    public double CpuUsage { get; set; }
}

public class RamUsageOutputDto
{
    public long _ts { get; set; }

    public double RamUsage { get; set; }
}

public class SwapUsageOutputDto
{
    public long _ts { get; set; }
    /// <summary>
    /// 交换区峰值使用量
    /// </summary>
    public double SwapUsage { get; set; }
}

public class DiskUsageOutputDto
{
    public long _ts { get; set; }
    /// <summary>
    /// 磁盘使用率
    /// </summary>
    public double DiskUsage { get; set; }
}
