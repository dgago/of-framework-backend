using IdentityServer3.Core.Models;

using MongoDB.Bson.Serialization.Attributes;

namespace of.identity.models
{
	public class MongoDbScope : Scope
	{
		public MongoDbScope(Scope item)
		{
			AllowUnrestrictedIntrospection = item.AllowUnrestrictedIntrospection;
			Claims = item.Claims;
			ClaimsRule = item.ClaimsRule;
			Description = item.Description;
			DisplayName = item.DisplayName;
			Emphasize = item.Emphasize;
			Enabled = item.Enabled;
			IncludeAllClaimsForUser = item.IncludeAllClaimsForUser;
			Name = item.Name;
			Id = item.Name;
			Required = item.Required;
			ScopeSecrets = item.ScopeSecrets;
			ShowInDiscoveryDocument = item.ShowInDiscoveryDocument;
			Type = item.Type;
		}

		[BsonId]
		public string Id { get; set; }
	}
}