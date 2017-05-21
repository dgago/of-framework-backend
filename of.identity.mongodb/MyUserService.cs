using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using IdentityServer3.Core;
using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;

using of.identity.data;
using of.identity.models;

namespace of.identity
{
/*
 * TODO:
 * 
 * Security stamp.
 * Lock user.
 * Verify locked user.
 * Logins en colección.
 * Claims para email y phone.
 * Logging.
 */

	/// <summary>
	///    In-memory user service
	/// </summary>
	public class MyUserService : UserServiceBase
	{
		private readonly MongoDbUserStore _users;

		/// <summary>
		///    Initializes a new instance of the <see cref="MyUserService" /> class.
		/// </summary>
		/// <param name="users">The users.</param>
		public MyUserService(MongoDbUserStore users)
		{
			_users = users;
		}

		/// <summary>
		///    This methods gets called for local authentication (whenever the user uses the username and password dialog).
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		public override async Task AuthenticateLocalAsync(LocalAuthenticationContext context)
		{
			User user = await _users.FindOneAsync(u => u.Username == context.UserName);
			if (user != null)
			{
				if (user.Locked)
				{
					context.AuthenticateResult = new AuthenticateResult("El usuario está bloqueado.");
					return;
				}

				bool ok = Crypto.VerifyHashedPassword(user.Password, context.Password);
				if (ok)
				{
					// agregar claim para security stamp?
					context.AuthenticateResult = new AuthenticateResult(user.Id, GetDisplayName(user));
				}
				else
				{
					// access failed

					context.AuthenticateResult = new AuthenticateResult("El usuario o la contraseña ingresados son incorrectos.");
					return;
				}
			}
		}

		/// <summary>
		///    This method gets called when the user uses an external identity provider to authenticate.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		public override async Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
		{
			ExternalIdentity externalIdentity = context.ExternalIdentity;
			if (externalIdentity == null)
			{
				throw new ArgumentNullException(nameof(externalIdentity));
			}

			User user = await _users.FindOneAsync(u => u.Provider == externalIdentity.Provider && u.ProviderId == externalIdentity.ProviderId);
			if (user == null)
			{
				Claim name = externalIdentity.Claims.FirstOrDefault(x => x.Type == Constants.ClaimTypes.Name);
				string displayName = name == null ? externalIdentity.ProviderId : name.Value;

				user = new User
				{
					Id = CryptoRandom.CreateUniqueId(),
					Provider = externalIdentity.Provider,
					ProviderId = externalIdentity.ProviderId,
					Username = displayName,
					Claims = externalIdentity.Claims.ToModel()
				};
				await _users.CreateAsync(user);
			}

			context.AuthenticateResult = new AuthenticateResult(user.Id, GetDisplayName(user),
																				identityProvider: externalIdentity.Provider);
		}

		/// <summary>
		///    This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo
		///    endpoint)
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
		{
			ClaimsPrincipal subject = context.Subject;
			if (subject == null)
			{
				throw new ArgumentNullException(nameof(subject));
			}

			User user = await _users.FindOneAsync(u => u.Id == context.Subject.GetSubjectId());

			List<Claim> claims = new List<Claim>
			{
				new Claim(Constants.ClaimTypes.Subject, user.Id)
			};

			claims.AddRange(user.Claims.FromModel());
			if (!context.AllClaimsRequested)
			{
				claims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
			}

			context.IssuedClaims = claims;
		}

		/// <summary>
		///    This method gets called whenever identity server needs to determine if the user is valid or active (e.g. during
		///    token issuance or validation)
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">subject</exception>
		public override async Task IsActiveAsync(IsActiveContext context)
		{
			ClaimsPrincipal subject = context.Subject;
			if (subject == null)
			{
				throw new ArgumentNullException(nameof(subject));
			}

			User user = await _users.FindOneAsync(u => u.Id == subject.GetSubjectId());

			context.IsActive = (user != null) && user.Active;

			// validar security stamp?
		}

		/// <summary>
		///    Retrieves the display name.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <returns></returns>
		protected virtual string GetDisplayName(User user)
		{
			if (user.Claims == null)
			{
				return user.Username;
			}

			UserClaim nameClaim = user.Claims.FirstOrDefault(x => x.Type == Constants.ClaimTypes.Name);
			return nameClaim != null ? nameClaim.Value : user.Username;
		}
	}
}