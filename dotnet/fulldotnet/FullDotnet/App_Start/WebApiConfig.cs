using Swashbuckle.Application;
using System.Web.Http;

namespace FullDotnet
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "swagger_root",
                routeTemplate: "",
                defaults: null,
                constraints: null,
                handler: new RedirectHandler((message => message.RequestUri.ToString().TrimEnd('/')), "swagger/ui/index"));

            config.MapHttpAttributeRoutes();

            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "AuthTest .Net Framework 4.7.2");
            })
            .EnableSwaggerUi(c =>
            {
                c.DocumentTitle("AuthTest .Net Framework 4.7.2");
            });
        }
    }
}
