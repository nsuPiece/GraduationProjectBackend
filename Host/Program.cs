using Tao.Extensions;

namespace Host;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args).Inject();

        builder.Services.AddControllers().AddInject();

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseInject();

        app.MapControllers();

        //添加涛思数据库
        app.AddTaos();

        app.Run();
    }
}