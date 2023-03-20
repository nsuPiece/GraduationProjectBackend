﻿using Core.Consts;
using Core.Extensions;
using Mapster;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using StackExchange.Profiling.Internal;
using System.Diagnostics;
using System.Security.Claims;
using TaosData.Extensions;
using TaosData.Tables;
using UAParser;

namespace Host.Filter;

/// <summary>
/// 请求审计过滤器
/// </summary>
public class RequestAuditFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //============== 这里是执行方法之前获取数据 ====================

        // 获取控制器、路由信息
        var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

        // 获取请求的方法
        var method = actionDescriptor.MethodInfo;

        // 获取 HttpContext 和 HttpRequest 对象
        var httpContext = context.HttpContext;
        var httpRequest = httpContext.Request;

        // 获取客户端 Ipv4 地址
        var remoteIPv4 = httpContext.GetRemoteIpAddressToIPv4();

        // 获取请求的 Url 地址
        var requestUrl = httpRequest.GetRequestUrlAddress();

        // 获取来源 Url 地址
        var refererUrl = httpRequest.GetRefererUrlAddress();

        // 获取请求参数（写入日志，需序列化成字符串后存储）
        var parameters = context.ActionArguments;

        // 获取操作人（必须授权访问才有值）"userId" 为你存储的 claims type，jwt 授权对应的是 payload 中存储的键名
        var userId = httpContext.User?.FindFirstValue(ClaimTypeConsts.Id);

        var userName = httpContext.User?.Identities.First().Name ?? "未登录";

        // 请求时间
        var requestedTime = DateTimeOffset.Now;


        //============== 这里是执行方法之后获取数据 ====================
        var actionContext = await next();

        // 返回时间
        var returnTime = DateTimeOffset.Now;

        // 获取返回的结果
        var returnResult = actionContext.Result;

        // 判断是否请求成功，没有异常就是请求成功
        var isRequestSucceed = actionContext.Exception == null;

        // 获取调用堆栈信息，提供更加简单明了的调用和异常堆栈
        var stackTrace = EnhancedStackTrace.Current();

        // 这里写入日志，或存储到数据库中！！！~~~~~~~~~~~~~~~~~~~~

        var referer = httpRequest.Headers["Referer"];
        var userAgent = httpRequest.Headers["User-Agent"];

        var uaParser = Parser.GetDefault();
        var c = uaParser.Parse(userAgent);

        var data = new OperationRecordsLog()
        {
            StartTime = requestedTime.DateTime,
            EndTime = returnTime.DateTime,
            ElapsedTime = (returnTime.Ticks - requestedTime.Ticks) / 10000.0,
            InterfaceAddress = requestUrl,
            Parameters = parameters?.Serialize(),
            ReturnResult = isRequestSucceed ? returnResult?.ToJson() : "{}",
            UserId = long.Parse(userId ?? "0"),
            UserName = userName,
            IpAddress = remoteIPv4,
            OperatingSystem = c.OS.Family,
            Browser = c.UA.Family,
            UserAgent = userAgent,
            IsRequestSucceed = isRequestSucceed,
            HttpMethod = httpRequest.Method
        };

        ////SqlServer
        //OperationRepository.Insert(data);

        //TDengine
        OperationLog operationLog = new OperationLog
        {
            TName = $"O{data.UserId}",
            Tags = data.Adapt<OperationLogTags>(),
            Fields = data.Adapt<OperationLogFields>(),
            _ts = data.StartTime.ToTimeStamp()
        };

        await TaosExtensions.Conn.InsertAsync(operationLog);
    }
}
