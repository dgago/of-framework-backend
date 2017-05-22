using System.Collections.Generic;

using of.data;
using of.support.configuration;
using of.web.http;

namespace of.support.web.controllers
{
	public class SettingsController : DefaultApiController<Setting, string>
	{
		public SettingsController(ISettingManager manager) : base(manager)
		{
			UseViewModel = true;
		}

		protected override Results<object> GetViewModel(Results<Setting> results)
		{
			List<dynamic> list = results.Items.ToDynamicList();
			return new Results<object>(list, results.Count, results.PageIndex, results.PageSize);
		}

		protected override object GetViewModel(Setting item)
		{
			return Extensions.ToDynamic(item);
		}
	}
}
