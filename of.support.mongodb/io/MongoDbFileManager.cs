using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

using MongoDB.Driver.GridFS;

using of.data;

namespace of.support.io
{
	public class MongoDbFileManager : IFileManager
	{
		private readonly GridFSBucket _bucket;

		public MongoDbFileManager(GridFSBucket bucket)
		{
			_bucket = bucket;
		}


		public Task<Results<FileModel>> FindAllAsync(IPrincipal user, int pageIndex, int pageSize)
		{
			throw new NotImplementedException();
		}

		public Task<Results<FileModel>> FindAsync(IPrincipal user, IEnumerable<KeyValuePair<string, string>> query, int pageIndex, int pageSize)
		{
			throw new NotImplementedException();
		}

		public async Task<FileModel> FindOneAsync(IPrincipal user, string id)
		{
			byte[] bytes = await _bucket.DownloadAsBytesByNameAsync(id);
			return new FileModel { Name = id, Contents = bytes, Size = bytes.Length };
		}

		public async Task<string> CreateAsync(IPrincipal user, FileModel item)
		{
			return (await _bucket.UploadFromBytesAsync(item.Name, item.Contents)).ToString();
		}

		public Task ReplaceAsync(IPrincipal user, string id, FileModel item)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(IPrincipal user, string id)
		{
			throw new NotImplementedException();
		}
	}
}
