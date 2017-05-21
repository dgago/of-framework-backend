using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using IdentityServer3.Core.Models;

using of.identity.models;

namespace of.identity
{
	internal static class IdentityExtensions
	{
		public static string GetOrigin(this string url)
		{
			if (url == null || (!url.StartsWith("http://") && !url.StartsWith("https://")))
			{
				return null;
			}
			int idx = url.IndexOf("//", StringComparison.Ordinal);
			if (idx <= 0)
			{
				return null;
			}
			idx = url.IndexOf("/", idx + 2, StringComparison.Ordinal);
			if (idx >= 0)
			{
				url = url.Substring(0, idx);
			}
			return url;
		}

		public static List<UserClaim> ToModel(this IEnumerable<Claim> claims)
		{
			List<UserClaim> res = claims.Select(x => new UserClaim(x)).ToList();
			return res;
		}

		public static List<Claim> FromModel(this IEnumerable<UserClaim> claims)
		{
			List<Claim> res = claims.Select(arg => new Claim(arg.Type, arg.Value, arg.ValueType, arg.Issuer)).ToList();
			return res;
		}

		public static List<Token> FromModel(this IEnumerable<MongoDbToken> items)
		{
			return items.Select(x => x.FromModel()).ToList();
		}

		public static Token FromModel(this MongoDbToken item)
		{
			Token res = new Token
			{
				Audience = item.Audience,
				Claims = item.Claims.FromModel(),
				Client = item.Client,
				CreationTime = item.CreationTime,
				Issuer = item.Issuer,
				Lifetime = item.Lifetime,
				Type = item.Type,
				Version = item.Version
			};

			return res;
		}
	}
}