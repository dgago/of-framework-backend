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
		private readonly IGrandChildManager<TItem, TKey> _manager;

		public GrandChildApiController(IGrandChildManager<TItem, TKey> manager)
		{
			_manager = manager;
		}

		[PaginationFilter]
		public async Task<IHttpActionResult> Get([FromUri] TKey id, [FromUri] TKey childId, [FromUri] int? pageIndex = null, [FromUri] int? pageSize = null)
		{
			IEnumerable<KeyValuePair<string, string>> qs = Request.GetQueryNameValuePairs();
			Results<TItem> results = await _manager.Find(User, id, childId, qs, pageIndex ?? 1, pageSize ?? MAX_RECORDS);

			return UseViewModel ? OkCount(GetViewModel(results)) : OkCount(results);
		}

		public async Task<IHttpActionResult> GetOne([FromUri] TKey id, [FromUri] TKey childId, [FromUri] TKey grandChildId)
		{
			TItem res = await _manager.FindOne(User, id, childId, grandChildId);
			if (res == null)
			{
				return NotFound();
			}
			return Ok(res);
		}

		public async Task<IHttpActionResult> Post([FromUri] TKey id, [FromUri] TKey childId, TItem item)
		{
			TKey res = await _manager.Create(User, id, childId, item);
			return Created(res.ToString(), item);
		}

		public async Task<IHttpActionResult> Put([FromUri] TKey id, [FromUri] TKey childId, [FromUri] TKey grandChildId, [FromBody] TItem item)
		{
			await _manager.Replace(User, id, childId, grandChildId, item);
			return NoContent();
		}

		public async Task<IHttpActionResult> Delete([FromUri] TKey id, [FromUri] TKey childId, [FromUri] TKey grandChildId)
		{
			await _manager.Delete(User, id, childId, grandChildId);
			return NoContent();
		}

		protected virtual Results<object> GetViewModel(Results<TItem> results)
		{
			return results.AsObjects<object>();
		}
	}
}