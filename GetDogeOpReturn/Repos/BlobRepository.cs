using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Threading;
using System.Threading.Tasks;
using GetDogeOpReturn.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using GetDogeOpReturn.Utils;

namespace GetDogeOpReturn.Repos
{

	/// <summary>
	/// An implementation of Blob storage
	/// </summary>
	/// <typeparam name="T">The Type of items to be stored</typeparam>
	public class BlobRepository<T> : IRepository<T> where T : IStringId
	{
		private static CloudStorageAccount account = null;
		private static CloudBlobClient client = null;
		private static CloudBlobContainer cont = null;
		private static object lockObject = new object();

		public BlobRepository(string connString)
		{
			if (account == null)
			{
				account = AzureUtils.GetStorageAccount<T>(connString);
			}
			if (client == null)
			{
				lock (lockObject)
				{
					if (client == null)
					{
						client = account.CreateCloudBlobClient();
						cont = client.GetContainerReference(typeof(T).Name.ToLower());
						cont.CreateIfNotExistsAsync().GetAwaiter().GetResult();
					}
				}
			}
		}


		public async Task<T> Load(String id)
		{
			T obj = default(T);
			try
			{
				CloudBlockBlob blob = cont.GetBlockBlobReference(id);


				int maxTimes = 1;
				for (int times = 0; times < maxTimes; times++)
				{
					try
					{

						string txt = await blob.DownloadTextAsync();
						if (!string.IsNullOrEmpty(txt))
						{
							obj = JsonConvert.DeserializeObject<T>(txt);
						}
					}
					catch (Exception e)
					{
						ErrorUtils.LogException(e);
						if (times < maxTimes - 1)
						{
							Thread.Sleep(10);
						}
					}

					if (obj != null)
					{
						break;
					}
				}

			}
			catch (Exception eee)
			{
				ErrorUtils.LogException(eee);
			}

			return obj;
		}


		public async Task<bool> Save(T t, string id = "")
		{
			if (t == null)
			{
				return false;
			}

			if (string.IsNullOrEmpty(id))
            {
				id = t.Id;
            }

			string data = JsonConvert.SerializeObject(t);
			return await StringSave(id, data);
		}

		protected async Task<bool> StringSave(string id, string data)
		{

			if (String.IsNullOrEmpty(id) || string.IsNullOrEmpty(data))
			{
				return false;
			}

			CloudBlockBlob blob = cont.GetBlockBlobReference(id);

			bool success = false;
			int maxTimes = 2;
			for (int times = 0; times < maxTimes; times++)
			{
				if (string.IsNullOrEmpty(data))
				{
					return false;
				}

				try
				{
					await blob.UploadTextAsync(data);
					success = true;
					break;
				}
				catch (Exception e)
				{
					if (times < maxTimes - 1)
					{
						await Task.Delay(20);
					}
					if (times == 2)
					{

						ErrorUtils.LogException(e);
					}
				}
			}


			return success;
		}

		public async Task<bool> Delete(T t)
		{
			if (t == null)
			{
				return false;
			}

			if (String.IsNullOrEmpty(t.Id))
			{
				return false;
			}

			CloudBlockBlob blob = cont.GetBlockBlobReference(t.Id);

			bool success = false;
			try
			{
				await blob.DeleteAsync();
				success = true;
			}
			catch (Exception e)
			{
				ErrorUtils.LogException(e);
				try
				{
					await Task.Delay(10);
					await blob.DeleteAsync();
					success = true;
				}
				catch (Exception ee)
				{
					ErrorUtils.LogException(ee);

				}
			}
			return success;
		}
	}

}
