using System.Threading.Tasks;

using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;

using of.data;
using of.identity.models;

namespace of.identity.data
{
	public class MongoDbClientStore : MongoDbStore<MongoDbClient, string>, IClientStore
	{
		public MongoDbClientStore(MongoDbContext context) : base(context.GetCollection<MongoDbClient>("of.auth.client"))
		{
		}

		public async Task<Client> FindClientByIdAsync(string clientId)
		{
			MongoDbClient client = await FindOneAsync(clientId);
			return client;
		}
	}
}