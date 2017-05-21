using System.Collections.Generic;
using System.Data;
using System.Dynamic;

namespace of.data
{
	public static class DataExtensions
	{
		public static List<dynamic> ToDynamic(this DataTable dt)
		{
			List<dynamic> res = new List<dynamic>();
			foreach (DataRow row in dt.Rows)
			{
				dynamic dyn = new ExpandoObject();
				res.Add(dyn);
				foreach (DataColumn column in dt.Columns)
				{
					IDictionary<string, object> dic = (IDictionary<string, object>)dyn;
					dic[column.ColumnName.ToLower()] = row[column];
				}
			}
			return res;
		}
	}
}