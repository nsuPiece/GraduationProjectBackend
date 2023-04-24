namespace Host.Base.Result;

/// <summary>
/// 分页数据返回
/// </summary>
public class PageResultDto<T> : ItemsResultDto<T>, IPagedResult<T>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public PageResultDto()
    {
        IsSuccess = true;
    }

    /// <summary>
    /// 总的数量
    /// </summary>
    public long Total { get; set; }

    /// <summary>
    /// 成功
    /// </summary>
    /// <returns></returns>
    public new IPagedResult<T> Success()
    {
        IsSuccess = true;
        Code = ResponseCode.Success;

        return this;
    }

    /// <summary>
    /// 成功
    /// </summary>
    /// <param name="items">数据</param>
    /// <returns></returns>
    public new IPagedResult<T> Success(IEnumerable<T> items)
    {
        Items = items;
        IsSuccess = true;
        Total = items.Count();
        Code = ResponseCode.Success;

        return this;
    }

    /// <summary>
    /// 成功
    /// </summary>
    /// <param name="items">数据</param>
    /// <param name="total">总的数量</param>
    /// <returns></returns>
    public IPagedResult<T> Success(IEnumerable<T> items, long total)
    {
        Items = items;
        Total = total;
        IsSuccess = true;
        Code = ResponseCode.Success;

        return this;
    }

    /// <summary>
    /// 成功
    /// </summary>
    /// <param name="items">数据</param>
    /// <returns></returns>
    public static new IPagedResult<T> NewSuccess(IEnumerable<T> items) => new PageResultDto<T>
    {
        Code = ResponseCode.Success,
        Message = "OK",
        IsSuccess = true,
        Items = items
    };

    /// <summary>
    /// 成功
    /// </summary>
    /// <param name="items">数据</param>
    /// <param name="total">总的数量</param>
    /// <returns></returns>
    public static IPagedResult<T> NewSuccess(IEnumerable<T> items, long total) => new PageResultDto<T>
    {
        Code = ResponseCode.Success,
        Message = "OK",
        IsSuccess = true,
        Items = items,
        Total = total
    };
}
