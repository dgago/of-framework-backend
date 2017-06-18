using System.Collections.Generic;
using System.Threading.Tasks;

using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;

using MongoDB.Bson;

using of.data;
using of.identity.models;

namespace of.identity.data
{
	public class MongoDbConsentStore : MongoDbStore<MongoDbConsent, string>, IConsentStore
	{
		public MongoDbConsentStore(MongoDbContext context) : base(context.GetCollection<MongoDbConsent>("of.auth.consent"))
		{
		}

		public async Task<IEnumerable<Consent>> LoadAllAsync(string subject)
		{
			return await FindAsync(x => x.Subject == subject);
		}

		public Task RevokeAsync(string subject, string client)
		{
			return RemoveAsync(x => x.Subject == subject && x.ClientId == client);
		}

		public async Task<Consent> LoadAsync(string subject, string client)
		{
			return await FindOneAsync(x => x.Subject == subject && x.ClientId == client);
		}

		public async Task UpdateAsync(Consent consent)
		{
			MongoDbConsent item = await FindOneAsync(x => x.Subject == consent.Subject && x.ClientId == consent.ClientId);
			if (item != null)
			{
				item.Scopes = consent.Scopes;
				await UpdateAsync(item.Id, item);
			}
			else
			{
				MongoDbConsent c = new MongoDbConsent(consent) {Id = ObjectId.GenerateNewId().ToString()};
				await CreateAsync(c);
			}
		}
	}
}