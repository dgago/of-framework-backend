using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using of.data;
using of.web.http.filter;

namespace of.web.http
{
	public abstract class GrandChildApiController<TItem, TKey> : BaseApiController
	{
		protected readonly IGrandChildManager<TItem, TKey> Manager;

		public GrandChildApiController(IGrandChildManager<TItem, TKey> manager)
		{
			Manager = manager;
		}

		[PaginationFilter]
		public virtual async Task<IHttpActionResult> Get([FromUri] TKey id, [FromUri] TKey childId, [FromUri] int? pageIndex = null, [FromUri] int? pageSize = null, [FromUri] string sortBy = null)
		{
			IEnumerable<KeyValuePair<string, string>> qs = Request.GetQueryNameValuePairs();
			Results<TItem> results = await Manager.Find(User, id, childId, qs, pageIndex ?? 1, pageSize ?? MAX_RECORDS, sortBy);

			return UseViewModel ? OkCount(GetViewModel(results)) : OkCount(results);
		}

		public virtual async Task<IHttpActionResult> GetOne([FromUri] TKey id, [FromUri] TKey childId, [FromUri] TKey grandChildId)
		{
			TItem res = await Manager.FindOne(User, id, childId, grandChildId);
			if (res == null)
			{
				return NotFound();
			}
			return Ok(res);
		}

		public virtual async Task<IHttpActionResult> Post([FromUri] TKey id, [FromUri] TKey childId, TItem item)
		{
			TKey res = await Manager.Create(User, id, childId, item);
			return Created(res.ToString(), item);
		}

		public virtual async Task<IHttpActionResult> Put([FromUri] TKey id, [FromUri] TKey childId, [FromUri] TKey grandChildId, [FromBody] TItem item)
		{
			await Manager.Replace(User, id, childId, grandChildId, item);
			return NoContent();
		}

		public virtual async Task<IHttpActionResult> Delete([FromUri] TKey id, [FromUri] TKey childId, [FromUri] TKey grandChildId)
		{
			await Manager.Delete(User, id, childId, grandChildId);
			return NoContent();
		}

		protected virtual Results<object> GetViewModel(Results<TItem> results)
		{
			return results.AsObjects<object>();
		}
	}
}