namespace Host.Base.Result;

/// <summary>
/// 分页返回
/// </summary>
public interface IPagedResult<T> : IResult, IItemsResult<T>
{
    /// <summary>
    /// 总的数量
    /// </summary>
    long Total { get; set; }
}
