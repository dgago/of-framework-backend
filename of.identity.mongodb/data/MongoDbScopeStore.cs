using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;

using of.data;
using of.identity.models;

namespace of.identity.data
{
	public class MongoDbScopeStore : MongoDbStore<MongoDbScope, string>, IScopeStore
	{
		public MongoDbScopeStore(MongoDbContext context) : base(context.GetCollection<MongoDbScope>("of.auth.scope"))
		{
		}

		public async Task<IEnumerable<Scope>> FindScopesAsync(IEnumerable<string> scopeNames)
		{
			List<MongoDbScope> scopes = await FindAllAsync();
			MongoDbScope[] scopes1 = (from x in scopes
												where scopeNames.Contains(x.Id)
												select x).ToArray();
			return scopes1;
		}

		public async Task<IEnumerable<Scope>> GetScopesAsync(bool publicOnly = true)
		{
			List<MongoDbScope> scopes = await FindAsync(x => x.ShowInDiscoveryDocument);
			return scopes;
		}
	}
}