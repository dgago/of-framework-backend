using of.data;

namespace of.support.configuration.data
{
    public class MongoDbSettingStore  : MongoDbStore<Setting, string>, ISettingStore
    {
	    public MongoDbSettingStore(MongoDbContext context) : base(context.GetCollection<Setting>("settings"))
	    {
	    }
    }
}
