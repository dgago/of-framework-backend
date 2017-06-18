using System.Collections.Generic;
using System.Dynamic;

namespace of
{
	public static class MongoDbDataExtensions
	{
		public static IDictionary<string, object> ToDynamic(this string name, object value)
		{
			IDictionary<string, object> dyn = new ExpandoObject();

			dyn.Add(name, value);

			return dyn;
		}
	}
}