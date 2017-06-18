using System;
using System.Collections.Generic;

using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;

using of.data;
using of.identity.data;
using of.identity.models;

namespace of.identity
{
	public static class MongoDbIdentityServerServiceFactoryExtensions
	{
		public static void RegisterMongoDbServices(this IdentityServerServiceFactory factory, ServiceOptions options)
		{
			if (factory == null) throw new ArgumentNullException(nameof(factory));

			factory.Register(new Registration<MongoDbContext>(resolver => new MongoDbContext(options.ConnectionStringName, new[] { "of", "IdentityServer3" })));

			factory.ClientStore = new Registration<IClientStore, MongoDbClientStore>();
			factory.CorsPolicyService = new Registration<ICorsPolicyService, ClientConfigurationCorsPolicyService>();

			factory.ScopeStore = new Registration<IScopeStore, MongoDbScopeStore>();

			factory.AuthorizationCodeStore = new Registration<IAuthorizationCodeStore, MongoDbAuthorizationCodeStore>();
			factory.TokenHandleStore = new Registration<ITokenHandleStore, MongoDbTokenHandleStore>();
			factory.ConsentStore = new Registration<IConsentStore, MongoDbConsentStore>();
			factory.RefreshTokenStore = new Registration<IRefreshTokenStore, MongoDbRefreshTokenStore>();

			factory.Register(new Registration<MongoDbUserStore>());
			factory.UserService = new Registration<IUserService, MyUserService>();
		}

		public static void RegisterInMongoDb(this List<Scope> scopes, ServiceOptions options)
		{
			MongoDbContext context = new MongoDbContext(options.ConnectionStringName, new[] { "of", "IdentityServer3" });
			MongoDbScopeStore store = new MongoDbScopeStore(context);
			foreach (Scope scope in scopes)
			{
				bool exists = store.Exists(x => x.Id == scope.Name);
				if (exists)
				{
					continue;
				}
				store.Create(new MongoDbScope(scope));
			}
		}

		public static void RegisterInMongoDb(this List<Client> clients, ServiceOptions options)
		{
			MongoDbContext context = new MongoDbContext(options.ConnectionStringName, new[] { "of", "IdentityServer3" });
			MongoDbClientStore store = new MongoDbClientStore(context);
			foreach (Client client in clients)
			{
				bool exists = store.Exists(x => x.Id == client.ClientId);
				if (exists)
				{
					continue;
				}
				store.Create(new MongoDbClient(client));
			}
		}
	}
}