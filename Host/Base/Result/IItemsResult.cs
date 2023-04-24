namespace Host.Base.Result;

/// <summary>
/// 数据返回
/// </summary>
/// <typeparam name="T">对象类型</typeparam>
public interface IItemsResult<T> : IResult
{
    /// <summary>
    /// 数据
    /// </summary>
    IEnumerable<T> Items { get; set; }
}
