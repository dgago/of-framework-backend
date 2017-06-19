using System.Collections.Generic;

using IdentityServer3.Core;
using IdentityServer3.Core.Resources;
using IdentityServer3.Core.Services;

namespace of.identity
{
	public class SpLocalizationService : ILocalizationService
	{
		private static readonly Dictionary<string, Dictionary<string, string>> Dic = new Dictionary<string, Dictionary<string, string>>
		{
			{
				Constants.LocalizationCategories.Events, new Dictionary<string, string>
				{
					{EventIds.ClientPermissionsRevoked, "Permiso de cliente revocado"},
					{EventIds.CspReport, "Reporte de política de seguridad de contenido (CSP)"},
					{EventIds.ExternalLoginError, "Error en inicio de sesión externo"},
					{EventIds.ExternalLoginFailure, "Fallo en inicio de sesión externo"},
					{EventIds.ExternalLoginSuccess, "Inicio de sesión externo exitoso"},
					{EventIds.LocalLoginFailure, "Fallo en inicio de sesión local"},
					{EventIds.LocalLoginSuccess, "Inicio de sesión local exitoso"},
					{EventIds.LogoutEvent, "Evento de cierre de sesión"},
					{EventIds.PartialLogin, "Inicio de sesión parcial"},
					{EventIds.PartialLoginComplete, "Inicio de sesión parcial completo"},
					{EventIds.PreLoginFailure, "Fallo en pre-inicio de sesión"},
					{EventIds.PreLoginSuccess, "Pre-inicio de sesión exitoso."},
					{EventIds.ResourceOwnerFlowLoginFailure, "Fallo en inicio de sesión del flujo Resource Owner Password"},
					{EventIds.ResourceOwnerFlowLoginSuccess, "Inicio de sesión del flujo Resource Owner Password exitoso"},
					{EventIds.TokenRevoked, "Token revocado"},
				}
			},
			{
				Constants.LocalizationCategories.Scopes, new Dictionary<string, string>
				{
					{ScopeIds.Roles_DisplayName, "Roles de usuario" },
					{ScopeIds.Address_DisplayName, "Tu dirección postal" },
					{ScopeIds.All_claims_DisplayName, "Toda la información del usuario" },
					{ScopeIds.Email_DisplayName, "Tu correo electrónico" },
					{ScopeIds.Offline_access_DisplayName, "Acceso fuera de línea" },
					{ScopeIds.Openid_DisplayName, "Tu identificador de usuario" },
					{ScopeIds.Phone_DisplayName, "Tu número de teléfono" },
					{ScopeIds.Profile_Description, "La información de tu perfil (nombre, apellido, etc.)" },
					{ScopeIds.Profile_DisplayName, "Tu perfil de usuario" },
				}
			},
			{
				Constants.LocalizationCategories.Messages, new Dictionary<string, string>
				{
					{MessageIds.ClientIdRequired, "El identificador del cliente es requerido" },
					{MessageIds.ExternalProviderError, "Hubo un error iniciando sesión en el proveedor externo. El mensaje de error es: {0}" },
					{MessageIds.Invalid_request, "La aplicación cliente hizo un requerimiento inválido." },
					{MessageIds.Invalid_scope, "La aplicación cliente ha intentado acceder a un recurso al cual no tiene acceso." },
					{MessageIds.InvalidUsernameOrPassword, "Nombre de usuario o contraseña inválidos" },
					{MessageIds.MissingClientId, "No se encontró el identificador del cliente" },
					{MessageIds.MissingToken, "No se encontró el token" },
					{MessageIds.MustSelectAtLeastOnePermission, "Debe seleccionar al menos un permiso." },
					{MessageIds.NoExternalProvider, "El proveedor de inicio de sesión externo no fue provisto." },
					{MessageIds.NoMatchingExternalAccount, "Cuenta inválida" },
					{MessageIds.NoSignInCookie, "Hubo un error al determinar a cuál aplicación está ingresando. Retorna a la aplicación e intenta nuevamente." },
					{MessageIds.NoSubjectFromExternalProvider, "Error autenticando con el proveedor externo" },
					{MessageIds.PasswordRequired, "La contraseña es requerida" },
					{MessageIds.SslRequired, "SSL es requerido" },
					{MessageIds.Unauthorized_client, "La aplicación cliente no se conoce o no está autorizada." },
					{MessageIds.UnexpectedError, "Hubo un error inesperado" },
					{MessageIds.Unsupported_response_type, "El servidor de autorización no soporta el tipo de respuesta requerido." },
					{MessageIds.UnsupportedMediaType, "Media type no soportado" },
					{MessageIds.UsernameRequired, "El nombre de usuario es requerido" },
				}
			}
		};

		public string GetString(string category, string id)
		{
			if (Dic[category].ContainsKey(id))
			{
				return Dic[category][id];
			}

			return null;
		}
	}
}
