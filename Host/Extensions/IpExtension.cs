using Core.Extensions;
using Furion.DataValidation;

namespace Nutri.Host.Extensions;

/// <summary>
/// 客户端IP地址扩展
/// </summary>
public static class IpExtension
{
    /// <summary>
    /// 获得IP地址
    /// </summary>
    /// <param name="request">请求信息</param>
    /// <returns></returns>
    public static string GetIpAddress(this HttpRequest request)
    {
        if (request == null)
            return "";

        var ip = request.Headers["X-Real-IP"].FirstOrDefault();

        if (ip.IsNullOrEmpty())
            ip = request.Headers["X-Forwarded-For"].FirstOrDefault();

        if (ip.IsNullOrEmpty())
            ip = request.HttpContext?.Connection?.RemoteIpAddress?.ToString();

        if (string.IsNullOrEmpty(ip) || !ip.Split(":")[0].TryValidate(ValidationTypes.IPv4).IsValid)
            ip = "127.0.0.1";

        return ip;
    }
}
