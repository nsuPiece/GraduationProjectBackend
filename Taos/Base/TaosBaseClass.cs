namespace Tao.Base;

public abstract class TaosBaseClass
{
    /// <summary>
    /// 超级表名
    /// </summary>
    public abstract string STName { get; set; }
    /// <summary>
    /// 子表名
    /// </summary>
    public abstract string TName { get; set; }
    /// <summary>
    /// 标签数据
    /// </summary>
    public abstract TaosBaseTags Tags { get; set; }
    /// <summary>
    /// 普通列数据
    /// </summary>
    public abstract TaosBaseFields Fields { get; set; }
    /// <summary>
    /// 主键时间戳
    /// </summary>
    public abstract long _ts { get; set; }
}

/// <summary>
/// 标签
/// </summary>
public abstract class TaosBaseTags
{
}
/// <summary>
/// 普通列
/// </summary>
public abstract class TaosBaseFields
{
}
