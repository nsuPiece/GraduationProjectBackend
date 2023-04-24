namespace Host.Dto.Setting;

public class SettingDto
{

    /// <summary>
    /// 用户Id
    /// </summary>
    [Obsolete]
    public long UserId => _ts;

    public long _ts { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public int Num { get; set; }

    public int Interval { get; set; }
}
