namespace Host.Dto.Login;

/// <summary>
/// 登录返回信息
/// </summary>
public class LoginOutputDto
{
    /// <summary>
    /// 授权票据
    /// </summary>
    public string AccessToken { get; set; }
    /// <summary>
    /// 刷新票据
    /// </summary>
    public string RefreshToken { get; set; }
    /// <summary>
    /// 过期时间
    /// </summary>
    public long ExpiresIn { get; set; }
}
