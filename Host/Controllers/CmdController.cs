using Host.Base.Result;
using Host.Dto.Cmd;
using Host.Dto.Setting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaosData.Extensions;
using TaosData.Tables;

namespace Host.Controllers;

[ApiController]
[Route("cmd")]
public class CmdController
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IResult<CmdOutputDto>> GetCmdResult(CmdInputDto input)
    {
        var result = new ResultDto<CmdOutputDto>();

        var user = await TaosExtensions.Conn.Query<SettingDto>("Setting").ToOneAsync();

        CmdCommand data = new()
        {
            Tags = new CmdCommandTags
            {
                Email = user.Email,
            },
            Fields = new CmdCommandFields
            {
                Command = input.Command,
            },
            _ts = DateTimeOffset.Now.ToUnixTimeMilliseconds()
        };
        await TaosExtensions.Conn.InsertAsync(data);

        await Task.Delay(TimeSpan.FromSeconds(2));

        var cmdResult = await TaosExtensions.Conn.Query<CmdOutputDto>("CmdResult").Where(x => x.Cid == data._ts).ToOneAsync();

        if (cmdResult == null)
            return result.Error("超时，请重试");
        else return result.Success(cmdResult);

    }
}
