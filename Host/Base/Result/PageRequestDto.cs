namespace Host.Base.Result;

/// <summary>
/// 分页请求信息
/// </summary>
public class PageRequestDto : IPageRequest
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public PageRequestDto()
    {
        // 默认第一页
        PageNum = 1;
        // 默认 20 条数据
        PageSize = 20;
    }

    /// <summary>
    /// 页码
    /// </summary>
    public int PageNum { get; set; }
    /// <summary>
    /// 数量
    /// </summary>
    public int PageSize { get; set; }
    /// <summary>
    /// 排序方式
    /// </summary>
    public string Sorting { get; set; }
}
