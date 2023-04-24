using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;

namespace Nutri.Host.Extensions;

/// <summary>
/// 路由前缀应用
/// </summary>
public static class MvcOptionsExtension
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    /// <param name="opts"></param>
    /// <param name="routeAttribute"></param>
    public static void UseCentralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
    {
        // 添加我们自定义 实现IApplicationModelConvention的RouteConvention
        opts.Conventions.Insert(0, new RoutePrefixConvention(routeAttribute));
    }
}