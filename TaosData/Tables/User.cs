using TaosData.Base;

namespace TaosData.Tables;

public class User : TaosBaseClass
{
    public override string STName { get; set; }
    public override string TName { get; set; }
    public override TaosBaseTags Tags { get; set; }
    public override TaosBaseFields Fields { get; set; }
    public override long _ts { get; set; }

    public User()
    {
        STName = nameof(User);
        Tags = new TokenTags();
        Fields = new TokenFields();
    }
}

public class UserTags : TaosBaseTags
{
    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }
}
public class UserFields : TaosBaseFields
{
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
}