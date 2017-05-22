using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;

using of.data;
using of.support;
using of.support.configuration;
using of.support.diagnostics;
using of.web.http.filter;

namespace of.web.http
{
	[OfExceptionFilter]
	public class BaseApiController : ApiController
	{
		protected const int MAX_RECORDS = 300;
		protected bool UseViewModel = false;

		public BaseApiController()
		{
			SettingsProvider = Support.Settings;
			LogProvider = Support.LogProvider;
		}

		protected ISettingManager SettingsProvider { get; }

		protected ILogProvider LogProvider { get; }

		protected override InvalidModelStateResult BadRequest(ModelStateDictionary modelState)
		{
			string message = GetErrorDescriptions(ModelState.Values.SelectMany(v => v.Errors));
			LogProvider?.Debug(message);
			return base.BadRequest(modelState);
		}

		protected IHttpActionResult Created(string id, object item)
		{
			return Created(new Uri(Request.RequestUri.PadUri() + id), item);
		}

		protected StatusCodeResult NoContent()
		{
			return StatusCode(HttpStatusCode.NoContent);
		}

		protected IHttpActionResult OkCount<T>(Results<T> item)
		{
			Request.Properties["Count"] = item.Count;

			return Ok(item.Items);
		}

		#region helpers

		private string GetErrorDescriptions(IEnumerable<ModelError> modelErrors)
		{
			IEnumerable<string> errors = modelErrors.Select(error => error.ErrorMessage);
			return string.Join("\r\n", errors);
		}

		#endregion
	}
}
