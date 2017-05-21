using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace of.data
{
	public interface IManager<TItem, TKey>
	{
		Task<Results<TItem>> FindAllAsync(IPrincipal user, int pageIndex, int pageSize);

		Task<Results<TItem>> FindAsync(IPrincipal user, IEnumerable<KeyValuePair<string, string>> query, int pageIndex, int pageSize);

		Task<TItem> FindOneAsync(IPrincipal user, TKey id);

		Task<TKey> CreateAsync(IPrincipal user, TItem item);

		Task ReplaceAsync(IPrincipal user, TKey id, TItem item);

		Task DeleteAsync(IPrincipal user, TKey id);
	}
}