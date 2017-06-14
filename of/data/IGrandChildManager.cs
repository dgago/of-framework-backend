using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace of.data
{
	public interface IGrandChildManager<TItem, TKey>
	{
		Task<Results<TItem>> FindAll(IPrincipal user, TKey grandParentId, TKey parentId, int pageIndex, int pageSize);

		Task<Results<TItem>> Find(IPrincipal user, TKey grandParentId, TKey parentId, IEnumerable<KeyValuePair<string, string>> query, int pageIndex, int pageSize);

		Task<TItem> FindOne(IPrincipal user, TKey grandParentId, TKey parentId, TKey childId);

		Task<TKey> Create(IPrincipal user, TKey grandParentId, TKey parentId, TItem item);

		Task Replace(IPrincipal user, TKey grandParentId, TKey parentId, TKey id, TItem item);

		Task Delete(IPrincipal user, TKey grandParentId, TKey parentId, TKey id);
	}
}