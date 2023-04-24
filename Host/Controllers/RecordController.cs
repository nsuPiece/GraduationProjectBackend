using Host.Base.Result;
using Host.Dto.Record.Disk;
using Host.Dto.Record.Ram;
using Host.Dto.Record.Swap;
using Host.Dto.Record.Usage;
using Host.Dto.Setting;
using Host.Dto.UpTime;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaosData.Extensions;

namespace Host.Controllers;

[ApiController]
[Route("record")]
public class RecordController
{
    [HttpGet("uptime")]
    [AllowAnonymous]
    public async Task<IResult<UpTimeOutputDto>> GetUpTime()
    {
        var result = new ResultDto<UpTimeOutputDto>();
        var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        var output = new UpTimeOutputDto();

        var data = await TaosExtensions.Conn.Query<UpTimeDto>("UpTime").ToOneAsync();

        if (now - data._ts > 60000)
        {
            output.s = "未连接" + data.HostName;
            return result.Success(output);
        }
        else
        {
            if (data.UpTime > 3600 * 24)
            {
                output.s = data.HostName + "已经运行" + $"{data.UpTime / (3600 * 24):F1}天";
            }
            else if (data.UpTime > 3600)
            {
                output.s = data.HostName + "已经运行" + $"{data.UpTime / 3600:F1}小时";
            }
            else if (data.UpTime > 60)
            {
                output.s = data.HostName + "已经运行" + $"{data.UpTime / 60:F1}分钟";
            }
            else
            {
                output.s = data.HostName + "已经运行" + $"{data.UpTime:F0}秒";
            }
            return result.Success(output);
        }
    }

    [HttpGet("usages")]
    [AllowAnonymous]
    public async Task<IResult<UsageDto>> GetCpuUsage()
    {
        var result = new ResultDto<UsageDto>();

        var dataNum = await TaosExtensions.Conn.Query<SettingDto>("Setting").ToOneAsync();

        var dataCpu = await TaosExtensions.Conn.Query<CpuUsageOutputDto>("CpuUsage").
            OrderByDescending(x => x._ts).LIMIT(0, dataNum.Num)
            .ToListAsync();

        var dataRam = await TaosExtensions.Conn.Query<RamUsageOutputDto>("RamUsage").
            OrderByDescending(x => x._ts).LIMIT(0, dataNum.Num)
            .ToListAsync();

        var dataSwap = await TaosExtensions.Conn.Query<SwapUsageOutputDto>("SwapUsage").
            OrderByDescending(x => x._ts).LIMIT(0, dataNum.Num)
            .ToListAsync();

        var dataDisk = await TaosExtensions.Conn.Query<DiskUsageOutputDto>("DiskUsage").
            OrderByDescending(x => x._ts).LIMIT(0, dataNum.Num)
            .ToListAsync();

        UsageDto data = new()
        {
            CpuUsage = dataCpu.Adapt<List<CpuUsageOutputDto>>(),
            RamUsage = dataRam.Adapt<List<RamUsageOutputDto>>(),
            SwapUsage = dataSwap.Adapt<List<SwapUsageOutputDto>>(),
            DiskUsage = dataDisk.Adapt<List<DiskUsageOutputDto>>()
        };

        return result.Success(data);
    }

    [HttpGet("rams")]
    [AllowAnonymous]
    public async Task<IResult<List<RamDto>>> GetRam()
    {
        var result = new ResultDto<List<RamDto>>();

        var dataNum = await TaosExtensions.Conn.Query<SettingDto>("Setting").ToOneAsync();

        var data = await TaosExtensions.Conn.Query<RamDto>("RamUsage").
            OrderByDescending(x => x._ts).LIMIT(0, dataNum.Num)
            .ToListAsync();

        return result.Success(data);
    }

    [HttpGet("swaps")]
    [AllowAnonymous]
    public async Task<IResult<List<SwapDto>>> GetSwap()
    {
        var result = new ResultDto<List<SwapDto>>();

        var dataNum = await TaosExtensions.Conn.Query<SettingDto>("Setting").ToOneAsync();

        var data = await TaosExtensions.Conn.Query<SwapDto>("SwapUsage").
            OrderByDescending(x => x._ts).LIMIT(0, dataNum.Num)
            .ToListAsync();

        return result.Success(data);
    }

    [HttpGet("disks")]
    [AllowAnonymous]
    public async Task<IResult<List<DiskDto>>> GetDisk()
    {
        var result = new ResultDto<List<DiskDto>>();

        var dataNum = await TaosExtensions.Conn.Query<SettingDto>("Setting").ToOneAsync();

        var data = await TaosExtensions.Conn.Query<DiskDto>("DiskUsage").
            OrderByDescending(x => x._ts).LIMIT(0, dataNum.Num)
            .ToListAsync();

        return result.Success(data);
    }
}
