namespace Host.Base.Result;

/// <summary>
/// 分页请求信息
/// </summary>
public interface IPageRequest
{
    /// <summary>
    /// 页码
    /// </summary>
    int PageNum { get; set; }
    /// <summary>
    /// 数量
    /// </summary>
    int PageSize { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    string Sorting { get; set; }
}
