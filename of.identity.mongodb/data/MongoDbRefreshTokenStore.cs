using System.Collections.Generic;
using System.Threading.Tasks;

using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;

using of.data;
using of.identity.models;

namespace of.identity.data
{
	public class MongoDbRefreshTokenStore : MongoDbStore<MongoDbRefreshToken, string>, IRefreshTokenStore
	{
		public MongoDbRefreshTokenStore(MongoDbContext context) : base(context.GetCollection<MongoDbRefreshToken>("of.auth.refresh-token"))
		{
		}

		public Task StoreAsync(string key, RefreshToken value)
		{
			MongoDbRefreshToken item = new MongoDbRefreshToken(value) { Id = key };
			return CreateAsync(item);
		}

		public async Task<RefreshToken> GetAsync(string key)
		{
			return await FindOneAsync(key);
		}

		public async Task<IEnumerable<ITokenMetadata>> GetAllAsync(string subject)
		{
			return await FindAsync(x => x.SubjectId == subject);
		}

		public Task RevokeAsync(string subject, string client)
		{
			return RemoveAsync(x => x.SubjectId == subject && x.ClientId == client);
		}
	}
}