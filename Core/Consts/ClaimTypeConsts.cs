using System.Security.Claims;

namespace Core.Consts;

/// <summary>
/// 身份信息
/// </summary>
public static class ClaimTypeConsts
{
    /// <summary>
    /// 主键
    /// </summary>
    public const string Id = ClaimTypes.NameIdentifier;
    /// <summary>
    /// 编号
    /// </summary>
    public const string Code = "code";
    /// <summary>
    /// 类型
    /// </summary>
    public const string Type = "type";
    /// <summary>
    /// 名称
    /// </summary>
    public const string Name = ClaimTypes.Name;
    /// <summary>
    /// 邮箱
    /// </summary>
    public const string Email = ClaimTypes.Email;
    /// <summary>
    /// 角色
    /// </summary>
    public const string Role = ClaimTypes.Role;
    /// <summary>
    /// 会话
    /// </summary>
    public const string Sid = ClaimTypes.Sid;
}
