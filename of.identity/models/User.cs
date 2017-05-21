using System.Collections.Generic;
using System.Security.Claims;

namespace of.identity.models
{
	public class User
	{
		public string Id { get; set; }

		public bool Active { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }

		public string Provider { get; set; }

		public string ProviderId { get; set; }

		public List<UserClaim> Claims { get; set; }

		public bool Locked { get; set; }

/*
		public bool IsLockedOut { get; set; }

		public string PasswordHash { get; set; }

		public string SecurityStamp { get; set; }

		public int AccessFailedCount { get; set; }

		public DateTime? LockoutEndDateUtc { get; set; }

		public string Email { get; set; }

		public string PhoneNumber { get; set; }

		public bool EmailConfirmed { get; set; }

		public bool PhoneNumberConfirmed { get; set; }
*/
	}

	public class UserClaim
	{
		public UserClaim(Claim x)
		{
			Issuer = x.Issuer;
			OriginalIssuer = x.OriginalIssuer;
			Properties = x.Properties;
			Type = x.Type;
			Value = x.Value;
			ValueType = x.ValueType;
		}

		public string Issuer { get; set; }

		public string OriginalIssuer { get; set; }

		public string Type { get; set; }

		public string Value { get; set; }

		public string ValueType { get; set; }

		public IDictionary<string, string> Properties { get; set; }
	}

}