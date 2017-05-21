using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using Microsoft.Owin;

using of.support;
using of.support.diagnostics;

namespace of.web.http.filter
{
	public class OfExceptionFilterAttribute : ExceptionFilterAttribute
	{
		// logger
		readonly ILogProvider _logger = Support.LogProvider;

		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			WriteLogAndSetResponse(actionExecutedContext);
		}

		public override Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext,
											CancellationToken cancellationToken)
		{
			WriteLogAndSetResponse(actionExecutedContext);

			return Task.CompletedTask;
		}

		#region helpers

		private void WriteLogAndSetResponse(HttpActionExecutedContext actionExecutedContext)
		{
			Exception ex = actionExecutedContext?.Exception;
			if (ex == null)
			{
				return;
			}

			HttpActionContext httpContext = actionExecutedContext.ActionContext;
			if (httpContext == null)
			{
				return;
			}

			string logId = WriteLog(actionExecutedContext, ex);
			HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
			if (ex is UnauthorizedAccessException)
			{
				httpStatusCode = HttpStatusCode.Forbidden;
			}
			else if (ex is NotFoundApplicationException)
			{
				httpStatusCode = HttpStatusCode.NotFound;
			}
			else if (ex is ApplicationException)
			{
				httpStatusCode = HttpStatusCode.BadRequest;
			}

			SetResponseForException(httpContext, ex, httpStatusCode, logId);
		}

		private string WriteLog(HttpActionExecutedContext context, Exception ex)
		{
			HttpActionContext actionContext = context.ActionContext;

			// username
			string userName = GetUserName(actionContext);

			// userhostaddress
			HttpRequestMessage request = actionContext.Request;
			IPAddress userHostAddress = GetUserHostAddress(request);

			// useragent
			HttpHeaderValueCollection<ProductInfoHeaderValue> userAgent = null;
			if (request?.Headers?.UserAgent != null)
			{
				userAgent = request.Headers.UserAgent;
			}

			// path
			string reference = request?.RequestUri?.AbsolutePath;

			Guid logId = Guid.NewGuid();
			const string MESSAGE = "User: {UserName}\r\nServer: {Server}\r\nResource: {Reference}\r\nHost: {Host} Agent: {Agent}";
			if (ex is UnauthorizedAccessException)
			{
				_logger.Information(ex, "[403] " + MESSAGE, userName,
													reference, Environment.MachineName, userHostAddress, userAgent);
			}
			else if (ex is NotFoundApplicationException)
			{
				_logger.Information(ex, "[404] " + MESSAGE, userName,
													reference, Environment.MachineName, userHostAddress, userAgent);
			}
			else if (ex is ApplicationException)
			{
				_logger.Information(ex, "[400] " + MESSAGE, userName,
													reference, Environment.MachineName, userHostAddress, userAgent);
			}
			else
			{
				_logger.Error(ex, "[500] [{Id}] " + MESSAGE, logId, userName,
											reference, Environment.MachineName, userHostAddress, userAgent);
			}

			return logId.ToString();
		}

		private static void SetResponseForException(HttpActionContext httpContext, Exception ex, HttpStatusCode httpStatusCode,
																	string logId = null)
		{
			HttpRequestContext requestContext = httpContext.RequestContext;

			var content = new
			{
				ex.Message,
				ExceptionType = ex.GetType().ToString(),
				StackTrace = requestContext.IncludeErrorDetail ? ex.StackTrace : null,
				LogId = logId,
				Type = ex is ApplicationException ? "warning" : "error"
			};

			JsonMediaTypeFormatter formatter = requestContext.Configuration.Formatters.OfType<JsonMediaTypeFormatter>().First();
			httpContext.Response = httpContext.Request.CreateResponse(httpStatusCode, content, formatter, "application/json");
		}

		private static string GetUserName(HttpActionContext actionContext)
		{
			string userName = null;
			IPrincipal principal = actionContext.RequestContext?.Principal;
			IIdentity identity = principal?.Identity;
			if (identity != null && identity.IsAuthenticated)
			{
				userName = identity.Name;
			}
			return userName;
		}

		private IPAddress GetUserHostAddress(HttpRequestMessage request)
		{
			const string MS_HTTP_CONTEXT = "MS_HttpContext";
			const string REMOTE_ENDPOINT_MESSAGE = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
			const string MS_OWIN_CONTEXT = "MS_OwinContext";

			if (request.Properties.ContainsKey(MS_HTTP_CONTEXT))
			{
				dynamic ctx = request.Properties[MS_HTTP_CONTEXT];
				if (ctx != null)
				{
					return IPAddress.Parse(ctx.Request.UserHostAddress);
				}
			}

			if (request.Properties.ContainsKey(REMOTE_ENDPOINT_MESSAGE))
			{
				dynamic remoteEndpoint = request.Properties[REMOTE_ENDPOINT_MESSAGE];
				if (remoteEndpoint != null)
				{
					return remoteEndpoint.Address;
				}
			}

			if (request.Properties.ContainsKey(MS_OWIN_CONTEXT))
			{
				return IPAddress.Parse(((OwinContext)request.Properties[MS_OWIN_CONTEXT]).Request.RemoteIpAddress);
			}

			return null;
		}

		#endregion
	}
}
