using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MLY
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                "Default1", // 路由名称
                "{controller}/{action}/{gameid}/{id}", // 带有参数的 URL
                new { controller = "Mytz", action = "Shty28", gameid = UrlParameter.Optional, id = UrlParameter.Optional } // 参数默认值
            );
            routes.MapRoute(
              "Default2", // 路由名称
              "{controller}/{action}/{id}/", // 带有参数的 URL
              new { controller = "RankRightList", action = "Shared", Parma1 = UrlParameter.Optional } // 参数默认值
          );
        }

    }
}
