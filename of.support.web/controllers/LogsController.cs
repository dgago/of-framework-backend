using of.support.diagnostics;
using of.web.http;

namespace of.support.web.controllers
{
	public class LogsController : DefaultApiController<Log, string>
	{
		public LogsController(ILogManager manager) : base(manager)
		{
		}
	}
}