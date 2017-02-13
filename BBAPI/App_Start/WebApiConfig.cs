using System.Web.Http;

namespace BBAPI
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				// api/user
				//api/user/dl@me.com --> returns only one user entry
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{email}",
				defaults: new { email = RouteParameter.Optional}
			);

		}
	}
}
