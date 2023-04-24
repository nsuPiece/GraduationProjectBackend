using Furion.Web.Core;
using Host.Extensions;
using Microsoft.AspNetCore.Mvc;
using Nutri.Host.Extensions;
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

        //添加涛思数据库
        services.AddTaos(configuration);

        //注册JWT
        services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);

        // 跨域
        services.AddCorsAccessor();
        services.AddControllers(options =>
        {
            // 设置全局路由前缀(api/????)
            options.UseCentralRoutePrefix(new RouteAttribute("api"));
        }).AddInjectWithUnifyResult<StandardResultProvider>();

        var app = builder.Build();

        // 跨域
        app.UseCorsAccessor();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseInject();

        app.MapControllers();

        app.Run();
    }
}