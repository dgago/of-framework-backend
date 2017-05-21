using IdentityServer3.Core.Models;

namespace of.identity.models
{
	public class MongoDbConsent : Consent
	{
		public MongoDbConsent(Consent item)
		{
			ClientId = item.ClientId;
			Scopes = item.Scopes;
			Subject = item.Subject;
		}

		public string Id { get; set; }
	}
}