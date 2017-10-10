using System.Web.Http;

namespace IdentityManagerRestApis
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "LoginApi",
                routeTemplate: "api/{controller}/{domain}/{userName}/{password}/{ldapPath}",
                defaults: new { domain = RouteParameter.Optional, userName = RouteParameter.Optional, password = RouteParameter.Optional, ldapPath =RouteParameter.Optional}
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}"
            );
        }
    }
}
