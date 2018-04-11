using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MySportsStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //以下的情况都是模拟实际应用场景的控制器选择
            //运行默认页的时候
            routes.MapRoute(
                null,
                "", //匹配空的URL，如"/" 
                new{controller = "Product", action = "List", category = (string)null, page = 1}
            );

            routes.MapRoute(
                null,
                "Home", //匹配空的URL，如"/" 
                new{controller = "Product", action = "List", category = (string)null, page = 1}
            );
            //点击分页的时候
            routes.MapRoute(
                null,
                "Page{page}", //匹配"/Page1"，当点击分页的时候
                new {controller = "Product", action = "List", category = (string)null},
                new {page = @"\d+"} //约束page为数字
            );

            //点击导航分类的时候
            routes.MapRoute(
                null,
                "{category}", //匹配 "/Soccer"，当点击导航分类的时候
                new {controller = "Product", action = "List", page = 1}
            );

            //点击导航分类后，再点击分页的时候
            routes.MapRoute(
                null,
                "{category}/Page{page}", //匹配"/Soccer/Page1"，当点击导航分类，在点击分页的时候
                new {controller = "Product", action = "List"},
                new {page = @"\d+"}
            );

            //默认情况
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Product", action = "List", id = UrlParameter.Optional }
            );
        }
    }
}