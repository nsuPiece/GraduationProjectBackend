using TaosData.Base;

namespace TaosData.Tables;

public class Setting : TaosBaseClass
{
    public override string STName { get; set; }
    public override string TName { get; set; }
    public override TaosBaseTags Tags { get; set; }
    public override TaosBaseFields Fields { get; set; }
    public override long _ts { get; set; }

    public Setting()
    {
        STName = nameof(Setting);
        Tags = new SettingTags();
        Fields = new SettingFields();
    }
}

public class SettingTags : TaosBaseTags
{
    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }
}
public class SettingFields : TaosBaseFields
{
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }

    public int Num { get; set; }

    public int Interval { get; set; }
}