namespace Host.Base.Result;

/// <summary>
/// 数据返回
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
[Serializable]
public class ItemsResultDto<T> : ResultDto, IItemsResult<T>
{
    /// <summary>
    /// 无参构造函数
    /// </summary>
    public ItemsResultDto()
    {
        Items = new List<T>();
    }

    /// <summary>
    /// 带数据的构造函数
    /// </summary>
    /// <param name="items">List of items</param>
    public ItemsResultDto(IReadOnlyList<T> items)
    {
        Items = items;
    }

    /// <summary>
    /// 返回的数据
    /// </summary>
    public IEnumerable<T> Items
    {
        get { return _items ??= Enumerable.Empty<T>(); }
        set { _items = value; }
    }
    private IEnumerable<T> _items;

    /// <summary>
    /// 失败（需要实例化）
    /// </summary>
    /// <param name="message">错误信息</param>
    /// <returns></returns>
    public override ItemsResultDto<T> Error(string message) => Error(message, default);

    /// <summary>
    /// 失败（需要实例化）
    /// </summary>
    /// <param name="message">错误信息</param>
    /// <param name="data">返回的数据</param>
    /// <returns></returns>
    public ItemsResultDto<T> Error(string message, IEnumerable<T> data)
    {
        Items = data;
        IsSuccess = true;
        Code = ResponseCode.Error;

        return this;
    }

    /// <summary>
    /// 成功
    /// </summary>
    /// <param name="items">数据</param>
    /// <returns></returns>
    public IItemsResult<T> Success(IEnumerable<T> items)
    {
        Items = items;
        IsSuccess = true;
        Code = ResponseCode.Success;

        return this;
    }

    /// <summary>
    /// 成功
    /// </summary>
    /// <param name="items">数据</param>
    /// <returns></returns>
    public static IItemsResult<T> NewSuccess(IEnumerable<T> items) => new ItemsResultDto<T>
    {
        Code = ResponseCode.Success,
        Message = "OK",
        IsSuccess = true,
        Items = items
    };

    /// <summary>
    /// 失败
    /// </summary>
    /// <param name="message">错误信息</param>
    /// <param name="code">状态码</param>
    /// <param name="items">数据</param>
    /// <returns></returns>
    public static IItemsResult<T> NewError(string message, int code = ResponseCode.Error, IEnumerable<T> items = default) => new ItemsResultDto<T>
    {
        Code = code,
        Message = message,
        IsSuccess = true,
        Items = items ?? Enumerable.Empty<T>()
    };
}
