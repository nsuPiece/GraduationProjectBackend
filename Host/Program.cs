using Host.Filter;
using TaosData.Extensions;

namespace Host;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args).Inject();

        // 服务配置
        var services = builder.Services;
        var configuration = builder.Configuration;

        services.AddControllers().AddInject();

        //添加涛思数据库
        services.AddTaos(configuration);
        //操作日志
        services.AddMvcFilter<RequestAuditFilter>();

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseInject();

        app.MapControllers();

        app.Run();
    }
}