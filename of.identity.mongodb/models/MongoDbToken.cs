using System;
using System.Collections.Generic;
using System.Linq;

using IdentityServer3.Core;
using IdentityServer3.Core.Models;

namespace of.identity.models
{
	public class MongoDbToken
	{
		public MongoDbToken(Token item)
		{
			Audience = item.Audience;
			Claims = item.Claims.ToModel();
			Client = item.Client;
			CreationTime = item.CreationTime;
			Issuer = item.Issuer;
			Lifetime = item.Lifetime;
			Type = item.Type;
			Version = item.Version;
			SubjectId = item.Claims.Where(x => x.Type == Constants.ClaimTypes.Subject).Select(x => x.Value).SingleOrDefault();
			ClientId = item.Client.ClientId;
		}

		public string ClientId { get; set; }

		public string SubjectId { get; set; }

		public string Audience { get; set; }

		public string Issuer { get; set; }

		public DateTimeOffset CreationTime { get; set; }

		public int Lifetime { get; set; }

		public string Type { get; set; }

		public Client Client { get; set; }

		public List<UserClaim> Claims { get; set; }

		public int Version { get; set; }

		public string Id { get; set; }
	}
}