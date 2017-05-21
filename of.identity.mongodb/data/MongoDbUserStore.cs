using of.data;
using of.identity.models;

namespace of.identity.data
{
	public class MongoDbUserStore : MongoDbStore<User, string>
	{
		public MongoDbUserStore(MongoDbContext context) : base(context.GetCollection<User>("of.auth.user"))
		{
		}
	}
}