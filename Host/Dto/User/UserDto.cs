namespace Host.Dto.User;

public class UserDto
{
    /// <summary>
    /// 用户Id
    /// </summary>
    [Obsolete]
    public long UserId => _ts;
    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 时间主键
    /// </summary>
    public long _ts { get; set; }
}
