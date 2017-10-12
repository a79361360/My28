using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MLY
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
    //        routes.RouteExistingFiles = false;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Js28", action = "Index", id = UrlParameter.Optional } // 参数默认值
            );

            routes.MapRoute(
                "Default1", // 路由名称
                "{controller}/{action}/{Parma1}/{Parma2}", // 带有参数的 URL
                new { controller = "Home", action = "Index", Parma1 = UrlParameter.Optional, Parma2 = UrlParameter.Optional } // 参数默认值
            );


        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
