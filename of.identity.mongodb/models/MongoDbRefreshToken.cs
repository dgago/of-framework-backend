using IdentityServer3.Core.Models;

namespace of.identity.models
{
	public class MongoDbRefreshToken : RefreshToken
	{
		public MongoDbRefreshToken(RefreshToken item)
		{
			AccessToken = item.AccessToken;
			CreationTime = item.CreationTime;
			LifeTime = item.LifeTime;
			Subject = item.Subject;
			Version = item.Version;
		}

		public string Id { get; set; }
	}
}