using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Threading.Tasks;

using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using of.data;
using of.support.io;

namespace of.support.azure
{
	public class AzureBlobFileManager : IFileManager
	{
		private static CloudStorageAccount _storageAccount;
		private static CloudBlobClient _blobClient;
		private static string _containerName;
		private static string _accountName;

		public AzureBlobFileManager(string connectionStringKeyName, string containerKeyName)
		{
			// Retrieve storage account from connection string.
			if (_storageAccount == null)
			{
				string setting = CloudConfigurationManager.GetSetting(connectionStringKeyName);
				string[] strings = setting.Split(';');
				foreach (string str in strings)
				{
					string[] parts = str.Split('=');
					if (parts[0].ToLower() == "accountname" && parts.Length == 2)
					{
						_accountName = parts[1];
						break;
					}
				}

				_storageAccount = CloudStorageAccount.Parse(setting);
			}

			// Create the blob client.
			if (_blobClient == null)
			{
				_blobClient = _storageAccount.CreateCloudBlobClient();
			}

			_containerName = CloudConfigurationManager.GetSetting(containerKeyName);
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
			id.NotNull(nameof(id));

			CloudBlobContainer container = _blobClient.GetContainerReference(_containerName);

			id = id.Replace("\\", "/");
			CloudBlockBlob blockBlob = container.GetBlockBlobReference(id);

			byte[] bytes;
			using (MemoryStream ms = new MemoryStream())
			{
				await blockBlob.DownloadToStreamAsync(ms);

				bytes = ms.ToArray();
			}

			return new FileModel { Name = id, Contents = bytes, Size = bytes.Length };
		}

		public async Task<string> CreateAsync(IPrincipal user, FileModel item)
		{
			item.NotNull(nameof(item));
			item.Name.NotNull(nameof(item.Name));
			item.Contents.NotNull(nameof(item.Contents));

			item.Name = item.Name.Replace("/", "-");
			item.Name = item.Name.Replace("\\", "-");

			CloudBlobContainer container = _blobClient.GetContainerReference(_containerName);
			container.CreateIfNotExists();
			container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

			string blobName = $"{DateTime.UtcNow.Ticks}/{item.Name}";
			CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
			await blockBlob.UploadFromByteArrayAsync(item.Contents, 0, item.Contents.Length);

			return blobName.Replace("/", "\\");
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
			id = id.Replace("\\", "/");
			string url = $"https://{_accountName}.blob.core.windows.net/{_containerName}/{id}";
			return Task.FromResult(url);
		}

		public bool AllowDownloads => true;
	}
}
