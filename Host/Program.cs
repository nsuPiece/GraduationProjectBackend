using Taos.Extensions;

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

        //app.AddTaos();

        app.Run();
    }
}