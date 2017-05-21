using IdentityServer3.Core.Models;

namespace of.identity.models
{
	public class MongoDbAuthorizationCode : AuthorizationCode
	{
		public MongoDbAuthorizationCode(AuthorizationCode item)
		{
			Client = item.Client;
			CodeChallenge = item.CodeChallenge;
			CodeChallengeMethod = item.CodeChallengeMethod;
			CreationTime = item.CreationTime;
			IsOpenId = item.IsOpenId;
			Nonce = item.Nonce;
			RedirectUri = item.RedirectUri;
			RequestedScopes = item.RequestedScopes;
			SessionId = item.SessionId;
			Subject = item.Subject;
			WasConsentShown = item.WasConsentShown;
		}

		public string Id { get; set; }
	}
}