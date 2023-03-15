using System.Security.Claims;

namespace Core.Extensions;

/// <summary>
/// 身份信息扩展
/// </summary>
public static class ClaimsExtension
{
    /// <summary>
    /// 从身份信息获取某个属性
    /// </summary>
    /// <param name="claims">身份信息</param>
    /// <param name="claim">属性</param>
    /// <returns></returns>
    public static string FindClaimValue(this IEnumerable<Claim> claims, string claim) => claims?.FirstOrDefault(t => t.Type == claim)?.Value;

    /// <summary>
    /// 从身份信息获取某个属性
    /// </summary>
    /// <param name="claims">身份信息</param>
    /// <param name="claim">属性</param>
    /// <returns></returns>
    public static IEnumerable<long> FindClaimsValue(this IEnumerable<Claim> claims, string claim) => claims?.Where(t => t.Type == claim).Select(t => long.Parse(t.Value));
    /// <summary>
    /// 从身份信息获取某个属性
    /// </summary>
    /// <param name="claims">身份信息</param>
    /// <param name="claim">属性</param>
    /// <returns></returns>
    public static long? FindLongValue(this IEnumerable<Claim> claims, string claim) => long.Parse(claims?.FirstOrDefault(t => t.Type == claim)?.Value ?? "0");
}
