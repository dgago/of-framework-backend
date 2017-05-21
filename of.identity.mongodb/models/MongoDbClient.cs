using IdentityServer3.Core.Models;

using MongoDB.Bson.Serialization.Attributes;

namespace of.identity.models
{
	public class MongoDbClient : Client
	{
		public MongoDbClient(Client item)
		{
			AbsoluteRefreshTokenLifetime = item.AbsoluteRefreshTokenLifetime;
			AccessTokenLifetime = item.AccessTokenLifetime;
			AccessTokenType = item.AccessTokenType;
			AllowAccessToAllCustomGrantTypes = item.AllowAccessToAllCustomGrantTypes;
			AllowAccessToAllScopes = item.AllowAccessToAllScopes;
			AllowAccessTokensViaBrowser = item.AllowAccessTokensViaBrowser;
			AllowClientCredentialsOnly = item.AllowClientCredentialsOnly;
			AllowRememberConsent = item.AllowRememberConsent;
			AllowedCorsOrigins = item.AllowedCorsOrigins;
			AllowedCustomGrantTypes = item.AllowedCustomGrantTypes;
			AllowedScopes = item.AllowedScopes;
			AlwaysSendClientClaims = item.AlwaysSendClientClaims;
			AuthorizationCodeLifetime = item.AuthorizationCodeLifetime;
			Claims = item.Claims;
			Id = item.ClientId;
			ClientId = item.ClientId;
			ClientName = item.ClientName;
			ClientSecrets = item.ClientSecrets;
			ClientUri = item.ClientUri;
			EnableLocalLogin = item.EnableLocalLogin;
			Enabled = item.Enabled;
			Flow = item.Flow;
			IdentityProviderRestrictions = item.IdentityProviderRestrictions;
			IdentityTokenLifetime = item.IdentityTokenLifetime;
			IncludeJwtId = item.IncludeJwtId;
			LogoUri = item.LogoUri;
			LogoutSessionRequired = item.LogoutSessionRequired;
			LogoutUri = item.LogoutUri;
			PostLogoutRedirectUris = item.PostLogoutRedirectUris;
			PrefixClientClaims = item.PrefixClientClaims;
			RedirectUris = item.RedirectUris;
			RefreshTokenExpiration = item.RefreshTokenExpiration;
			RefreshTokenUsage = item.RefreshTokenUsage;
			RequireConsent = item.RequireConsent;
			RequireSignOutPrompt = item.RequireSignOutPrompt;
			SlidingRefreshTokenLifetime = item.SlidingRefreshTokenLifetime;
			UpdateAccessTokenClaimsOnRefresh = item.UpdateAccessTokenClaimsOnRefresh;
		}

		[BsonId]
		public string Id { get; set; }
	}
}