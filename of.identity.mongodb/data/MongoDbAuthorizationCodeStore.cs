using System.Collections.Generic;
using System.Threading.Tasks;

using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;

using of.data;
using of.identity.models;

namespace of.identity.data
{
	public class MongoDbAuthorizationCodeStore : MongoDbStore<MongoDbAuthorizationCode, string>, IAuthorizationCodeStore
	{
		public MongoDbAuthorizationCodeStore(MongoDbContext context) : base(context.GetCollection<MongoDbAuthorizationCode>("of.auth.code"))
		{
		}

		public Task StoreAsync(string key, AuthorizationCode value)
		{
			MongoDbAuthorizationCode code = new MongoDbAuthorizationCode(value) { Id = key };
			return CreateAsync(code);
		}

		public async Task<AuthorizationCode> GetAsync(string key)
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