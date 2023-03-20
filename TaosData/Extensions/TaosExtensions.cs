using Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaosData.Base;
using TDengineDriver;

namespace TaosData.Extensions;

public static class TaosExtensions
{
    public static nint Conn { get; set; }

    /// <summary>
    /// 添加 TDengine
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置信息</param>
    /// <returns></returns>
    public static IServiceCollection AddTaos(this IServiceCollection services, IConfiguration configuration)
    {
        var option = configuration.GetOptions<TaosOptions>();

        Conn = TDengine.Connect(option.Host, option.UserName, option.Password, option.Database, option.Port);

        if (Conn == IntPtr.Zero)
            throw new Exception("Connect to TDengine failed");
        else
            Console.WriteLine("Connect to TDengine success");

        return services;
    }

    public static Task InsertAsync(string sql)
    {
        var res = TDengine.Query(Conn, sql);

        if (TDengine.ErrorNo(res) != 0)
        {
            throw new Exception($"failed to insert data since: {TDengine.Error(res)}");
        }

        var affectedRows = TDengine.AffectRows(res);
        Console.WriteLine("affectedRows " + affectedRows);
        TDengine.FreeResult(res);
        return Task.CompletedTask;
    }

    public static Task<int> InsertAsync(this IntPtr conn, TaosBaseClass o)
    {
        string measurement = $"{o.STName}";
        string tag_set = "";
        string field_set = "";
        string timestamp = o._ts.ToString();


        if (o.TName != null)
            measurement += $",tName={o.TName}";

        // 获取标签类型
        Type tagsType = o.Tags.GetType();

        // 遍历标签的属性
        foreach (var prop in tagsType.GetProperties())
        {
            // 获取属性名和属性值
            string propName = prop.Name;
            object propValue = prop.GetValue(o.Tags)!;

            tag_set += $",{propName}={propValue}";
        }

        // 获取普通列类型
        Type fieldsType = o.Fields.GetType();

        // 遍历普通列的属性
        foreach (var prop in fieldsType.GetProperties())
        {
            // 获取属性名和属性值
            string propName = prop.Name;
            object propValue = prop.GetValue(o.Fields)!;

            switch (propValue)
            {
                case string s:
                    s = s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("=", "\\=").Replace(",", "\\,").Replace(" ", "\\ ");
                    if (s.Length > 4000)
                        s = s[..4000] + "...";
                    field_set += $",{propName}=L\"{s} \"";
                    break;
                case double d:
                    field_set += $",{propName}={d}f64";
                    break;
                case float f:
                    field_set += $",{propName}={f}f32";
                    break;
                case byte b:
                    field_set += $",{propName}={b}i8";
                    break;
                case short s:
                    field_set += $",{propName}={s}i16";
                    break;
                case ushort us:
                    field_set += $",{propName}={us}u16";
                    break;
                case int i:
                    field_set += $",{propName}={i}i32";
                    break;
                case uint ui:
                    field_set += $",{propName}={ui}u32";
                    break;
                case long l:
                    field_set += $",{propName}={l}i64";
                    break;
                case ulong ul:
                    field_set += $",{propName}={ul}u64";
                    break;
                case bool b:
                    field_set += $",{propName}={b}";
                    break;
                case Enum e:
                    int enumValue = Convert.ToInt32(e);
                    field_set += $",{propName}={enumValue}i32";
                    break;
                case DateTime dt:
                    //field_set += $",{propName}={dt.ToTimeStamp()}i64";
                    field_set += $",{propName}=\"{dt:yyyy-MM-dd HH:mm:ss.fff}\"";
                    break;
                default:
                    throw new Exception("存在不支持类型");
            }
        }

        string[] lines = { measurement + tag_set + " " + field_set.Remove(0, 1) + " " + timestamp };

        var rows = TDengine.SchemalessInsertRaw(conn, lines,
            TDengineSchemalessProtocol.TSDB_SML_LINE_PROTOCOL,
            TDengineSchemalessPrecision.TSDB_SML_TIMESTAMP_MILLI_SECONDS);

        return Task.FromResult(rows);
    }

    /// <summary>
    /// 涛思查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="conn">连接的数据库</param>
    /// <param name="TName">查询的表名</param>
    /// <returns></returns>
    public static SqlQueryExtensions<T> Query<T>(this IntPtr conn, string tName) where T : class
    {
        return new SqlQueryExtensions<T>(tName);
    }
}

