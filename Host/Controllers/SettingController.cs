using Host.Base.Result;
using Host.Dto.Setting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaosData.Extensions;
using TaosData.Tables;
using IResult = Host.Base.Result.IResult;

namespace Host.Controllers;

[ApiController]
[Route("settings")]
public class SettingController
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IResult> UpdataSetting(SettingInput input )
    {
        var result = new ResultDto();

        Setting data = new()
        {
            Tags = new SettingTags {Email = input.Email },
            Fields = new SettingFields
            {
                Password = input.Password,
                Num = input.Num,
                Interval= input.Interval,
            },
            _ts = DateTimeOffset.Now.ToUnixTimeMilliseconds()
        };

        await TaosExtensions.Conn.InsertAsync(data);

        return result.Success();
    }

}
