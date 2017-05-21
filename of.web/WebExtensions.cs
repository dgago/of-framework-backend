using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web;
using System.Web.Http;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

using Newtonsoft.Json.Serialization;

using of.web.http.dependencies;
using of.web.http.routing;

namespace of.web
{
	public static class WebExtensions
	{
		private static readonly UnityContainer Container = new UnityContainer();

		public static string PadUri(this Uri uri)
		{
			return uri.ToString().PadUri();
		}

		public static string PadUri(this string uri)
		{
			if (string.IsNullOrWhiteSpace(uri))
			{
				return "/";
			}

			uri = uri.Trim();
			return uri.EndsWith("/") ? uri : uri + "/";
		}

		private const string AspNetNamespace = "ASP";

		public static Assembly GetApplicationAssembly()
		{
			// Try the EntryAssembly, this doesn't work for ASP.NET classic pipeline (untested on integrated)
			Assembly ass = Assembly.GetEntryAssembly();

			// Look for web application assembly
			HttpContext ctx = HttpContext.Current;
			if (ctx != null)
				ass = GetWebApplicationAssembly(ctx);

			// Fallback to executing assembly
			return ass ?? (Assembly.GetExecutingAssembly());
		}

		private static Assembly GetWebApplicationAssembly(HttpContext context)
		{
			context.NotNull(nameof(context));

			object app = context.ApplicationInstance;
			if (app == null) return null;

			Type type = app.GetType();
			while (type != null && type != typeof(object) && type.Namespace == AspNetNamespace)
			{
				type = type.BaseType;
			}

			return type?.Assembly;
		}

		public static HttpConfiguration UseWebApi(this HttpConfiguration config, bool authorizeAll, bool enableCors, bool showErrorDetails)
		{
			// unity
			config.DependencyResolver = new ApiUnityDependencyResolver(Container);
			Container.LoadConfiguration();

			// require authentication for all controllers
			if (authorizeAll)
			{
				config.Filters.Add(new AuthorizeAttribute());
			}

			// javascript formatter
			JsonMediaTypeFormatter jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
			jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			//			jsonFormatter.SerializerSettings.Converters.Add(new BsonConverter());

			// cors
			if (enableCors)
			{
				//				config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
			}

			// error details
			if (showErrorDetails)
			{
				GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
			}

			// api
			config.MapHttpAttributeRoutes(new InheritanceDirectRouteProvider());

			HttpRouteCollection routes = config.Routes;
			routes.MapHttpRoute("DefaultApi", "{controller}/{id}", new { id = RouteParameter.Optional });

			return config;
		}
	}
}