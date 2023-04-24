using Furion;
using Furion.DataValidation;
using Furion.FriendlyException;
using Furion.UnifyResult;
using Host.Base.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Host.Extensions;

/// <summary>
/// 标准返回处理
/// </summary>
[UnifyModel(typeof(IResult<>))]
public class StandardResultProvider : IUnifyResultProvider
{
    /// <summary>
    /// 异常返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="metadata"></param>
    /// <returns></returns>
    public IActionResult OnException(ExceptionContext context, ExceptionMetadata metadata)
    {
        return new JsonResult(StandardResult(metadata.StatusCode, data: metadata.Data, message: context.Exception.Message ?? "服务器处理错误，请稍后重试"), UnifyContext.GetSerializerSettings(context)); // 当前行仅限 Furion 4.6.6+ 使用
    }

    /// <summary>
    /// 成功返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public IActionResult OnSucceeded(ActionExecutedContext context, object data)
    {
        if (data is Base.Result.IResult)
            return new JsonResult(data, UnifyContext.GetSerializerSettings(context));

        return new JsonResult(StandardResult(StatusCodes.Status200OK, true, string.Empty, data), UnifyContext.GetSerializerSettings(context)); // 当前行仅限 Furion 4.6.6+ 使用
    }

    /// <summary>
    /// 验证失败返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="metadata"></param>
    /// <returns></returns>
    public IActionResult OnValidateFailed(ActionExecutingContext context, ValidationMetadata metadata)
    {
        return new JsonResult(StandardResult(metadata.StatusCode ?? StatusCodes.Status400BadRequest, message: metadata.FirstErrorMessage)
            , UnifyContext.GetSerializerSettings(context)); // 当前行仅限 Furion 4.6.6+ 使用
    }

    /// <summary>
    /// 特定状态码返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="statusCode"></param>
    /// <param name="unifyResultSettings"></param>
    /// <returns></returns>
    public async Task OnResponseStatusCodes(HttpContext context, int statusCode, UnifyResultSettingsOptions unifyResultSettings)
    {
        // 设置响应状态码
        UnifyContext.SetResponseStatusCodes(context, statusCode, unifyResultSettings);

        switch (statusCode)
        {
            // 处理 401 状态码
            case StatusCodes.Status401Unauthorized:
                await context.Response.WriteAsJsonAsync(StandardResult(statusCode, message: "401 Unauthorized")
                    , App.GetOptions<JsonOptions>()?.JsonSerializerOptions);
                break;
            // 处理 403 状态码
            case StatusCodes.Status403Forbidden:
                await context.Response.WriteAsJsonAsync(StandardResult(statusCode, message: "403 Forbidden")
                    , App.GetOptions<JsonOptions>()?.JsonSerializerOptions);
                break;
            default: break;
        }
    }

    /// <summary>
    /// 返回 RESTful 风格结果集
    /// </summary>
    /// <param name="code"></param>
    /// <param name="isSuccess"></param>
    /// <param name="data"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    private static IResult<object> StandardResult(int code, bool isSuccess = false, string message = default, object data = default)
    {
        return new ResultDto<object>
        {
            Code = code,
            Data = data,
            Message = message,
            IsSuccess = isSuccess
        };
    }
}
