using System.Web.Http.Filters;

namespace of.web.http.filter
{
	public class PaginationFilterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
		{
			if (!actionExecutedContext.Request.Properties.ContainsKey("Count"))
			{
				return;
			}

			long count = actionExecutedContext.Request.Properties["Count"].ToInt64();
			actionExecutedContext.Response.Content.Headers.Add("X-Count", count.ToString());
		}
	}
}