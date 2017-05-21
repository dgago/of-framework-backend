using System.Collections.Generic;
using System.Threading.Tasks;

using of.data;
using of.support.configuration;
using of.web.http;

namespace of.support.web.controllers
{
	public class SettingsController : DefaultApiController<Setting, string>
	{
		public SettingsController(ISettingManager manager) : base(manager)
		{
		}

		protected override async Task<object> FindAllAsync(IEnumerable<KeyValuePair<string, string>> qs, int? pageIndex, int? pageSize)
		{
			Results<Setting> res = (Results<Setting>) await base.FindAllAsync(qs, pageIndex, pageSize);
			List<dynamic> list = res.Items.ToDynamicList();
			return new Results<object>(list, res.Count, res.PageIndex, res.PageSize);
		}
	}
}
