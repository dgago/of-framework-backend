using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

using MongoDB.Bson;

namespace of.support.configuration
{
	public static class Extensions
	{
		public static List<dynamic> ToDynamicList(this List<Setting> settings)
		{
			return settings.Select(ToDynamic).Cast<dynamic>().ToList();
		}

		public static IDictionary<string, object> ToDynamic(this Setting setting)
		{
			IDictionary<string, object> dyn = new ExpandoObject();

			dyn.Add("id", setting.Id);
			foreach (BsonElement item in setting.Value)
			{
				dyn.Add(item.Name, item.Value);
			}

			return dyn;
		}
	}
}