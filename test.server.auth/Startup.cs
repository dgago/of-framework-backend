using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

using IdentityServer3.Core.Configuration;

using Microsoft.Owin;

using of.identity;

using Owin;

using Serilog;

using test.server.auth;

[assembly: OwinStartup(typeof(Startup))]

namespace test.server.auth
{
	internal class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			Log.Logger = new LoggerConfiguration()
				.WriteTo.RollingFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log", "log-{Date}.log"), shared: true)
				//				.WriteTo.LiterateConsole(outputTemplate: "{Timestamp:HH:mm} [{Level}] ({Name:l}){NewLine} {Message}{NewLine}{Exception}")
				.CreateLogger();

			IdentityServerServiceFactory factory = new IdentityServerServiceFactory();

			ServiceOptions options = new ServiceOptions { ConnectionStringName = "of.mongodb" };
			factory.RegisterMongoDbServices(options);

			app.UseIdentityServer(new IdentityServerOptions
			{
				Factory = factory,
				RequireSsl = false,
				SigningCertificate = LoadCertificate()
			});
		}

		X509Certificate2 LoadCertificate()
		{
			string fileName = $@"{AppDomain.CurrentDomain.BaseDirectory}\bin\idsrv3test.pfx";
			X509Certificate2 cert = new X509Certificate2(fileName, "idsrv3test");
			return cert;
		}
	}
}