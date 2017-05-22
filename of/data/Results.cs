using System;
using System.Collections.Generic;
using System.Linq;

namespace of.data
{
	public class Results<TEntity>
	{
		public Results(List<TEntity> items, long count, int pageIndex, int pageSize)
		{
			PageIndex = pageIndex;
			if (pageSize == 0)
			{
				pageSize = 10;
			}
			PageSize = pageSize;
			Items = items;
			Count = count;
			PageCount = CalculatePageCount(count, pageSize);
		}

		public List<TEntity> Items { get; set; }

		public long Count { get; set; }

		public int PageIndex { get; set; }

		public int PageSize { get; set; }

		public int PageCount { get; set; }

		public Results<T> AsObjects<T>() where T : class
		{
			return new Results<T>(Items.Cast<T>().ToList(), Count, PageIndex, PageSize);
		}

		#region helpers

		protected int CalculatePageCount(long count, int pageSize)
		{
			return (int) Math.Ceiling((double) count / pageSize);
		}

		#endregion
	}
}