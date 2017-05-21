using System.Collections.Generic;
using System.Dynamic;

using MongoDB.Bson;

namespace of.support.configuration
{
	public static class Extensions
	{
		public static List<dynamic> ToDynamicList(this List<Setting> settings)
		{
			List<dynamic> res = new List<dynamic>();
			foreach (Setting setting in settings)
			{
				IDictionary<string, object> dyn = new ExpandoObject();

				dyn.Add("id", setting.Id);
				foreach (BsonElement item in setting.Value)
				{
					dyn.Add(item.Name, item.Value);
				}

				res.Add(dyn);
			}

			return res;
		}
	}
}