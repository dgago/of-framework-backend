using System.Collections.Generic;
using System.Threading.Tasks;

using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;

using of.data;
using of.identity.models;

namespace of.identity.data
{
	public class MongoDbTokenHandleStore : MongoDbStore<MongoDbToken, string>, ITokenHandleStore
	{
		public MongoDbTokenHandleStore(MongoDbContext context) : base(context.GetCollection<MongoDbToken>("of.auth.token"))
		{
		}

		public Task StoreAsync(string key, Token value)
		{
			MongoDbToken item = new MongoDbToken(value) { Id = key };
			return CreateAsync(item);
		}

		public async Task<Token> GetAsync(string key)
		{
			MongoDbToken item = await FindOneAsync(key);
			return item.FromModel();
		}

		public async Task<IEnumerable<ITokenMetadata>> GetAllAsync(string subject)
		{
			List<MongoDbToken> items = await FindAsync(x => x.SubjectId == subject);
			return items.FromModel();
		}

		public Task RevokeAsync(string subject, string client)
		{
			return RemoveAsync(x => x.SubjectId == subject && x.ClientId == client);
		}
	}
}