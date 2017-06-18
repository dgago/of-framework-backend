using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Principal;
using System.Threading.Tasks;

using MongoDB.Driver;
using MongoDB.Driver.GridFS;

using of.data;

namespace of.support.io
{
	public class MongoDbFileManager : IFileManager
	{
		private readonly GridFSBucket _bucket;

		public MongoDbFileManager(string connectionStringName)
		{
			MongoUrl url = new MongoUrl(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
			IMongoDatabase database = new MongoClient(url).GetDatabase(url.DatabaseName);
			GridFSBucketOptions options = new GridFSBucketOptions
			{
				WriteConcern = WriteConcern.W1,
				ReadPreference = ReadPreference.Secondary
			};
			_bucket = new GridFSBucket(database, options);
		}

		public Task<Results<FileModel>> FindAllAsync(IPrincipal user, int pageIndex, int pageSize, string sortBy)
		{
			throw new NotImplementedException();
		}

		public Task<Results<FileModel>> FindAsync(IPrincipal user, IEnumerable<KeyValuePair<string, string>> query, int pageIndex, int pageSize, string sortBy)
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

		public Task<string> GetDownloadUrlAsync(IPrincipal user, string id)
		{
			throw new NotImplementedException();
		}

		public bool AllowDownloads => false;
	}
}
