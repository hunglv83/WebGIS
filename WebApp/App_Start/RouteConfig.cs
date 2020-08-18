using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Empty",
                url: "",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "WebApp.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "cs/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "WebApp.Controllers" }
            );


            routes.MapRoute(
             name: "News",
             url: "{pagekey}",
             defaults: new { controller = "Home", action = "Index" },
             namespaces: new[] { "WebApp.Controllers" }
             );

            routes.MapRoute(
           name: "News Category",
           url: "{pagekey}/{category}",
           defaults: new { controller = "Home", action = "Index", category = UrlParameter.Optional },
           namespaces: new[] { "WebApp.Controllers" }
           );

            routes.MapRoute(
            name: "News Detail1",
            url: "{pagekey}/{pagename}/{pagetype}-{id}",
            defaults: new { controller = "Home", action = "Index", pagename = UrlParameter.Optional, pagetype = UrlParameter.Optional, id = UrlParameter.Optional },
            namespaces: new[] { "WebApp.Controllers" }
            );



            routes.MapRoute(
            name: "News Detail2",
            url: "{pagekey}/{category}/{pagename}/{pagetype}-{id}",
            defaults: new { controller = "Home", action = "Index", category = UrlParameter.Optional, pagename = UrlParameter.Optional, pagetype = UrlParameter.Optional, id = UrlParameter.Optional },
            namespaces: new[] { "WebApp.Controllers" }
            );


        }
    }
}