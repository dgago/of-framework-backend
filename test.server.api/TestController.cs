using System;
using System.Security.Claims;
using System.Web.Http;

using of.web.http;

namespace test.server.api
{
	public class TestController : BaseApiController
	{
		public IHttpActionResult Get()
		{
			ClaimsPrincipal caller = User as ClaimsPrincipal;

			if (caller == null)
			{
				throw new ApplicationException("Token incorrecto.");
			}

			Claim subjectClaim = caller.FindFirst("sub");
			if (subjectClaim != null)
			{
				return Json(new
				{
					message = "OK user",
					client = caller.FindFirst("client_id").Value,
					subject = subjectClaim.Value
				});
			}
			else
			{
				return Json(new
				{
					message = "OK computer",
					client = caller.FindFirst("client_id").Value
				});
			}
		}
	}
}