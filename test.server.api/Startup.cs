using System.Web.Http;

using IdentityServer3.AccessTokenValidation;

using Microsoft.Owin;

using of.web;

using Owin;

using test.server.api;

[assembly: OwinStartup(typeof(Startup))]

namespace test.server.api
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			// accept access tokens from identityserver and require a scope of 'api1'
			app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
			{
				Authority = "http://localhost:30080",
				ValidationMode = ValidationMode.Both,

				RequiredScopes = new[] { "api1" }
			});

			// configure web api
			HttpConfiguration config = new HttpConfiguration();
			app.UseWebApi(config.UseWebApi(true, true, true));
		}
	}
}