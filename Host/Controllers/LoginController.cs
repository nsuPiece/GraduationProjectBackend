using Furion.DataEncryption;
using Host.Base.Result;
using Host.Dto.Login;
using Host.Dto.Setting;
using Host.Dto.User;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaosData.Extensions;
using TaosData.Tables;

namespace Host.Controllers;

[ApiController]
[Route("login")]
public class LoginController : ControllerBase
{
    /// <summary>
    /// 用户登录
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IResult<LoginOutputDto>> Login(LoginInputDto input)
    {
        var result = new ResultDto<LoginOutputDto>();

        var user = await TaosExtensions.Conn.Query<SettingDto>("Setting").Where(x => x.Email == input.Email).ToOneAsync();

        if (user == null)
        {
            return result.Error("邮箱或密码错误");
        }

        if (user.Password != input.Password)
        {
            return result.Error("邮箱或密码错误");
        }

        // token
        var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>()
         {
            { "UserId" , user.UserId},//用户ID
            { "Email" , user.Email },  // 邮箱
        });

        // 获取刷新 token
        var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken, 120); // 第二个参数是刷新 token 的有效期（分钟），默认三十天

        var output = new LoginOutputDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = DateTimeOffset.Now.AddHours(2).ToUnixTimeMilliseconds()
        };

        var data = new Token
        {
            Tags = user.Adapt<TokenTags>(),
            Fields = output.Adapt<TokenFields>(),
            _ts = DateTimeOffset.Now.ToUnixTimeMilliseconds()
        };

        await TaosExtensions.Conn.InsertAsync(data);

        return result.Success(output);
    }

}
