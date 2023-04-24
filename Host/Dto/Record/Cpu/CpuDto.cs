namespace Host.Dto.Record.Cpu;

public class CpuDto
{
    public long _ts { get; set; }

    public double CpuUsage { get; set; }

    public string HostName { get; set; }
}
