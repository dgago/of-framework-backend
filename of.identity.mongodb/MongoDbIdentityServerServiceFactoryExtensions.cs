using System;

using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;

using of.data;
using of.identity.data;

namespace of.identity
{
	public static class MongoDbIdentityServerServiceFactoryExtensions
	{
		public static void RegisterMongoDbServices(this IdentityServerServiceFactory factory, ServiceOptions options)
		{
			if (factory == null) throw new ArgumentNullException(nameof(factory));

			factory.Register(new Registration<MongoDbContext>(resolver => new MongoDbContext(options.ConnectionStringName, new[] { "of", "IdentityServer3" })));

			//			factory.Register(new Registration<IEnumerable<Client>>());
			factory.ClientStore = new Registration<IClientStore, MongoDbClientStore>();
			factory.CorsPolicyService = new Registration<ICorsPolicyService, ClientConfigurationCorsPolicyService>();

			//			factory.Register(new Registration<IEnumerable<Scope>>());
			factory.ScopeStore = new Registration<IScopeStore, MongoDbScopeStore>();

			factory.AuthorizationCodeStore = new Registration<IAuthorizationCodeStore, MongoDbAuthorizationCodeStore>();
			factory.TokenHandleStore = new Registration<ITokenHandleStore, MongoDbTokenHandleStore>();
			factory.ConsentStore = new Registration<IConsentStore, MongoDbConsentStore>();
			factory.RefreshTokenStore = new Registration<IRefreshTokenStore, MongoDbRefreshTokenStore>();

			factory.Register(new Registration<MongoDbUserStore>());
			factory.UserService = new Registration<IUserService, MyUserService>();
		}
	}
}