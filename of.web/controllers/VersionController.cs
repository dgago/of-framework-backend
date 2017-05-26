using System.Web.Http;

using of.web.http;

namespace of.web.controllers
{
	public class VersionController : BaseApiController
	{
		public IHttpActionResult Get()
		{
			return Ok(WebExtensions.GetApplicationAssembly().GetName().Version.ToString());
		}
	}
}