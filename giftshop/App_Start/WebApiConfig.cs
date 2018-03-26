using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace giftshop
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de API web

            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                     name: "Common_default",
                     routeTemplate: "api/{controller}/{action}/{id}",
                     defaults: new { id = RouteParameter.Optional }
                 );

            config.Routes.MapHttpRoute(
                      name: "CommonApiAction",
                      routeTemplate: "api/{controller}/{action}",
                      defaults: new { id = RouteParameter.Optional }
                  );

            config.Routes.MapHttpRoute(
                   name: "CommonApi",
                   routeTemplate: "api/{controller}",
                   defaults: new { id = RouteParameter.Optional }
               );


            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}
