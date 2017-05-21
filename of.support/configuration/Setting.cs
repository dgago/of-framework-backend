using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace of.support.configuration
{
	public class Setting
	{
		public string Id { get; set; }

		[BsonExtraElements]
		public BsonDocument Value { get; set; }
	}
}