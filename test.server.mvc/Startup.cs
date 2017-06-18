using System.Web.Mvc;
using System.Web.Routing;

using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;

using Owin;

using test.server.mvc;

[assembly: OwinStartup(typeof(Startup))]

namespace test.server.mvc
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = "Cookies"
			});

			app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
			{
				Authority = "http://localhost:30080",
				ClientId = "mvc",
				RedirectUri = "http://localhost:51338/",
				ResponseType = "id_token",

				SignInAsAuthenticationType = "Cookies"
			});

			RouteCollection routes = RouteTable.Routes;
			routes.MapRoute("Default", "{controller}/{action}/{id}",
				new { controller = "Default", action = "Index", id = UrlParameter.Optional }
			);

		}
	}
}