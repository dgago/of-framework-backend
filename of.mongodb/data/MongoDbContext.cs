using System.Configuration;
using System.Linq;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace of.data
{
	public class MongoDbContext
	{
		public MongoDbContext(string connectionStringName, string[] assemblyPrefixes)
		{
			ConventionPack pack = new ConventionPack
			{
				new CamelCaseElementNameConvention(),
				new EnumRepresentationConvention(BsonType.String)
			};
			ConventionRegistry.Register("CamelCaseConventions", pack,
												t =>
												t.AssemblyQualifiedName != null
												&& (assemblyPrefixes.Any(x => t.AssemblyQualifiedName.StartsWith(x))
													|| t.AssemblyQualifiedName.StartsWith("Microsoft.AspNet.Identity")));

			MongoUrlBuilder mongoUrlBuilder =
				new MongoUrlBuilder(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);

			MongoClient client = new MongoClient(mongoUrlBuilder.ToMongoUrl());

			Database = client.GetDatabase(mongoUrlBuilder.DatabaseName);
		}

		public IMongoDatabase Database { get; set; }

		public IMongoCollection<T> GetCollection<T>(string name)
		{
			return Database.GetCollection<T>(name);
		}
	}
}