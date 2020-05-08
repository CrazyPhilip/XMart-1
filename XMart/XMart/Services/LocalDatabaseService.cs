using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XMart.Models;

namespace XMart.Services
{
    public class LocalDatabaseService
    {

		#region 搜索
		/// <summary>
		/// 插入搜索历史
		/// </summary>
		/// <param name="searchedItem"></param>
		/// <returns></returns>
		public Task<int> InsertSearchedItem(SearchedItem searchedItem)
		{
			//Database.DeleteAllAsync<SearchedItem>();   //先清空
			return LocalDatabaseHelper<SearchedItem>.InsertOrReplaceAsync(searchedItem);
		}

		/// <summary>
		/// 查找所有搜索历史
		/// </summary>
		/// <returns></returns>
		public Task<List<SearchedItem>> GetAllSearchedItem()
		{
			return LocalDatabaseHelper<SearchedItem>.GetAllItems();
		}

		/// <summary>
		/// 删除所有搜索历史
		/// </summary>
		/// <returns></returns>
		public Task<int> DeleteAllSearchedItem()
		{
			return LocalDatabaseHelper<SearchedItem>.DeleteAllItems();
		}
		#endregion

		/// <summary>
		/// 清除所有数据
		/// </summary>
		/// <returns></returns>
		public Task<int> ClearAllData()
		{
			return LocalDatabaseHelper<SearchedItem>.ClearDatabase();
		}
	}
}
