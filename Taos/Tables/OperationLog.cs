using Taos.Base;
namespace Taos.Tables;

public class OperationLog : TaosBaseClass
{
    public override string STName { get; set; }
    public override string TName { get; set; }//子表为$"O{UserId}"
    public override TaosBaseTags Tags { get; set; }
    public override TaosBaseFields Fields { get; set; }

    public override long _ts { get; set; }

    public OperationLog()
    {
        STName = nameof(OperationLog);
        Tags = new OperationLogTags();
        Fields = new OperationLogFields();
    }
}

public class OperationLogTags : TaosBaseTags
{
    /// <summary>
    /// 用户编号
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }
}
public class OperationLogFields :  TaosBaseFields
{
    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 运行时间
    /// </summary>
    public double ElapsedTime { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    public string InterfaceAddress { get; set; }

    /// <summary>
    /// 请求参数
    /// </summary>
    public string Parameters { get; set; }

    /// <summary>
    /// 返回的结果
    /// </summary>
    public string ReturnResult { get; set; }

    /// <summary>
    /// IP地址
    /// </summary>
    public string IpAddress { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    public string OperatingSystem { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    public string Browser { get; set; }

    /// <summary>
    /// 浏览器信息
    /// </summary>
    public string UserAgent { get; set; }

    /// <summary>
    /// 是否请求成功
    /// </summary>
    public bool IsRequestSucceed { get; set; }

    /// <summary>
    /// Http请求方式
    /// </summary>
    public string HttpMethod { get; set; }
}
