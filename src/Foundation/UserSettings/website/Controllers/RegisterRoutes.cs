using Sitecore.Pipelines;
using System.Web.Http;

namespace SF.Foundation.Facets.Controllers
{
    public class RegisterRoutes
    {
        public void Process(PipelineArgs args)
        {
            GlobalConfiguration.Configure(this.Configure);
        }

        protected void Configure(HttpConfiguration configuration)
        {
            MapRouteWithSession(configuration, "SF.UserSettings.GetArea", "api/sf/1.0/userSettings/{area}", "UserSettings", "GetAreaSettings");
            MapRouteWithSession(configuration, "SF.UserSetting.sGet", "api/sf/1.0/userSettings/{area}/{key}", "UserSettings", "GetUserSetting");
            MapRouteWithSession(configuration, "SF.UserSettings.Index", "api/sf/1.0/userSettings", "UserSettings", "Index");
        }

        protected static void MapRouteWithSession(HttpConfiguration configuration, string routeName, string routePath, string controller, string action)
        {
            var routes = configuration.Routes;
            routes.MapHttpRoute(routeName, routePath, new
            {
                controller = controller,
                action = action
            });

            var route = System.Web.Routing.RouteTable.Routes[routeName] as System.Web.Routing.Route;
            route.RouteHandler = new SessionRouteHandler();
        }
    }
}