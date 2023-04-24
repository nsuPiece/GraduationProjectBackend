using TaosData.Base;

namespace TaosData.Tables;

public class CmdCommand : TaosBaseClass
{
    public override string STName { get; set; }
    public override string TName { get; set; }
    public override TaosBaseTags Tags { get; set; }
    public override TaosBaseFields Fields { get; set; }
    public override long _ts { get; set; }

    public CmdCommand()
    {
        STName = nameof(CmdCommand);
        Tags = new CmdCommandTags();
        Fields = new CmdCommandFields();
    }
}

public class CmdCommandTags : TaosBaseTags
{
    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }
}
public class CmdCommandFields : TaosBaseFields
{
    public string Command { get; set; }
}