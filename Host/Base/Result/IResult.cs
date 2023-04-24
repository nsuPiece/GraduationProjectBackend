namespace Host.Base.Result;

/// <summary>
/// 状态返回服务
/// </summary>
public interface IResult
{
    /// <summary>
    /// 响应码
    /// </summary>
    int Code { get; set; }

    /// <summary>
    /// 响应信息
    /// </summary>
    string Message { get; set; }

    /// <summary>
    /// 成功
    /// </summary>
    bool IsSuccess { get; set; }

    /// <summary>
    /// 时间戳(毫秒)
    /// </summary>
    long Timestamp { get; }
}

/// <summary>
/// 带数据返回
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IResult<T> : IResult
{
    /// <summary>
    /// 返回的数据
    /// </summary>
    T Data { get; }
}
