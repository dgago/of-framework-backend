using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IdentityServer3.Core.Services;

using of.identity.data;
using of.identity.models;

namespace of.identity
{
	public class ClientConfigurationCorsPolicyService : ICorsPolicyService
	{
		private readonly MongoDbClientStore _store;

		public ClientConfigurationCorsPolicyService(IClientStore store)
		{
			_store = (MongoDbClientStore)store;
		}

		public async Task<bool> IsOriginAllowedAsync(string origin)
		{
			List<MongoDbClient> all = await _store.FindAllAsync();

			List<string> urls = new List<string>();
			foreach (MongoDbClient client in all)
			{
				urls.AddRange(client.AllowedCorsOrigins);
			}

			IEnumerable<string> origins = urls
				.Select(x => x.GetOrigin())
				.Where(x => x != null)
				.Distinct();

			bool result = origins.Contains(origin, StringComparer.OrdinalIgnoreCase);

			return result;
		}
	}
}