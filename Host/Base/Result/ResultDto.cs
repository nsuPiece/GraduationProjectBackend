namespace Host.Base.Result;

/// <summary>
/// 状态码
/// </summary>
internal class ResponseCode
{
    /// <summary>
    /// 成功
    /// </summary>
    public const int Success = 2000;

    /// <summary>
    /// 错误
    /// </summary>
    public const int Error = 5000;
}
/// <summary>
/// 状态返回
/// </summary>
[Serializable]
public class ResultDto : IResult
{
    /// <summary>
    /// 响应码
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 响应信息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 成功
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// 时间戳(毫秒)
    /// </summary>
    public long Timestamp => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    #region 成功
    /// <summary>
    /// 成功
    /// </summary>
    /// <returns></returns>
    public virtual ResultDto Success()
    {
        IsSuccess = true;
        Code = ResponseCode.Success;
        return this;
    }


    /// <summary>
    /// 成功
    /// </summary>
    /// <returns></returns>
    public static ResultDto NewSuccess() => new()
    {
        Code = ResponseCode.Success,
        Message = "OK",
        IsSuccess = true
    };
    #endregion

    #region 失败
    /// <summary>
    /// 失败（需要实例化）
    /// </summary>
    /// <param name="message">错误信息</param>
    /// <returns></returns>
    public virtual IResult Error(string message = "服务器处理错误，请稍后重试")
    {
        Message = message;
        IsSuccess = false;
        Code = ResponseCode.Error;

        return this;
    }

    /// <summary>
    /// 失败
    /// </summary>
    /// <returns></returns>
    public static ResultDto NewError() => NewError(default);

    /// <summary>
    /// 失败
    /// </summary>
    /// <param name="message">错误信息</param>
    /// <param name="code">状态码</param>
    /// <returns></returns>
    public static ResultDto NewError(string message, int code = ResponseCode.Error) => new()
    {
        Code = code,
        Message = message,
        IsSuccess = false
    };
    #endregion

    /// <summary>
    /// 根据状态返回
    /// </summary>
    /// <param name="isSuccess">是否成功</param>
    /// <returns></returns>
    public static ResultDto Result(bool isSuccess)
    {
        return isSuccess ? NewSuccess() : NewError();
    }
}

/// <summary>
/// 带数据返回
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public class ResultDto<T> : ResultDto, IResult<T>
{
    /// <summary>
    /// 返回的数据
    /// </summary>
    public T Data { get; set; }

    #region 成功

    /// <summary>
    /// 成功
    /// </summary>
    /// <returns></returns>
    public override ResultDto<T> Success()
    {
        base.Success();
        return this;
    }

    /// <summary>
    /// 成功
    /// </summary>
    /// <returns></returns>
    public ResultDto<T> Success(T data)
    {
        Data = data;
        IsSuccess = true;
        Code = ResponseCode.Success;

        return this;
    }

    /// <summary>
    /// 成功
    /// </summary>
    /// <returns></returns>
    public static ResultDto<T> NewSuccess(T data) => new()
    {
        Code = ResponseCode.Success,
        Message = "OK",
        IsSuccess = true,
        Data = data
    };
    #endregion

    #region 失败

    /// <summary>
    /// 失败（需要实例化）
    /// </summary>
    /// <param name="message">错误信息</param>
    /// <returns></returns>
    public override ResultDto<T> Error(string message) => Error(message, default);

    /// <summary>
    /// 失败（需要实例化）
    /// </summary>
    /// <param name="message">错误信息</param>
    /// <param name="data">返回的数据</param>
    /// <returns></returns>
    public virtual ResultDto<T> Error(string message, T data)
    {
        base.Error(message);
        Data = data;

        return this;
    }

    /// <summary>
    /// 失败
    /// </summary>
    /// <param name="message">错误信息</param>
    /// <param name="data">返回的数据</param>
    /// <returns></returns>
    public static ResultDto<T> NewError(string message, T data = default) => NewError(message, data);

    /// <summary>
    /// 失败
    /// </summary>
    /// <param name="message">错误信息</param>
    /// <param name="code">状态码</param>
    /// <param name="data">返回的数据</param>
    /// <returns></returns>
    public static ResultDto<T> NewError(string message, int code = ResponseCode.Error, T data = default) => new()
    {
        Code = code,
        Message = message,
        Data = data,
        IsSuccess = false
    };
    #endregion
}
