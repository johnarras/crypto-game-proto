using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetDogeOpReturn.Repos
{/// <summary>
 /// This contains some common code used in Azure repositories
 /// </summary>
	public class AzureUtils
	{

		private static Dictionary<string, CloudStorageAccount> _accounts = new Dictionary<string, CloudStorageAccount>();
		/// <summary>
		/// Creates a storage account object from the RepositoryData provided.
		/// This object is shared among all repositories, so a singleton is
		/// used to only set this up once.
		/// </summary>
		/// <param name="rd">RepositoryData containing setup information</param>
		/// <returns>A new CloudStorageAccount</returns>
		public static CloudStorageAccount GetStorageAccount<T>(string connString)
		{
			if (_accounts.ContainsKey(connString))
			{
				return _accounts[connString];
			}


			CloudStorageAccount acct = CloudStorageAccount.Parse(connString);

			if (acct != null)
			{
				_accounts[connString] = acct;
			}

			return acct;
		}
	}
}
