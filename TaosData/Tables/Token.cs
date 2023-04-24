

using TaosData.Base;

namespace TaosData.Tables;

public class Token : TaosBaseClass
{
    public override string STName { get ; set ; }
    public override string TName { get ; set ; }
    public override TaosBaseTags Tags { get; set; }
    public override TaosBaseFields Fields { get; set; }
    public override long _ts { get; set; }

    public Token ()
    {
        STName = nameof(Token);
        Tags = new TokenTags();
        Fields = new TokenFields();
    }
}

public class TokenTags :TaosBaseTags
{
    /// <summary>
    /// Id
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }
}
public class TokenFields : TaosBaseFields
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