using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace of.data
{
	public interface IChildManager<TItem, TKey>
	{
		Task<Results<TItem>> FindAll(IPrincipal user, TKey parentId, int pageIndex, int pageSize, string sortBy);

		Task<Results<TItem>> Find(IPrincipal user, TKey parentId, IEnumerable<KeyValuePair<string, string>> query, int pageIndex, int pageSize, string sortBy);

		Task<TItem> FindOne(IPrincipal user, TKey parentId, TKey childId);

		Task<TKey> Create(IPrincipal user, TKey parentId, TItem item);

		Task Replace(IPrincipal user, TKey parentId, TKey id, TItem item);

		Task Delete(IPrincipal user, TKey parentId, TKey id);
	}
}