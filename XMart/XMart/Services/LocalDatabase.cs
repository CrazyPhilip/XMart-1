using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMart.Models;

namespace XMart.Services
{
    public class LocalDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public LocalDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(SearchedItem).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(SearchedItem)).ConfigureAwait(false);
                    initialized = true;
                }
            }
        }

		#region 可见操作
		public Task<CreateTableResult> CreateUserInfo()
		{
			var result = Database.CreateTableAsync<UserInfo>();
			Database.CreateIndexAsync("UserInfo", "id", true);
			return result;
		}

		public Task<UserInfo> GetUserInfo()
		{
			return Database.Table<UserInfo>().FirstOrDefaultAsync();
		}

		public Task<int> SaveSearchedItem(SearchedItem searchedItem)
		{
			//Database.DeleteAllAsync<SearchedItem>();   //先清空
			return Database.InsertOrReplaceAsync(searchedItem);
		}

		public Task<int> SaveAllSearchedItem(List<Category> list)
		{
			//database.DeleteAllAsync<UserInfo>();   //先清空
			return Database.InsertAllAsync(list);
		}

		public Task<List<SearchedItem>> GetAllSearchedItem()
		{
			return Database.Table<SearchedItem>().ToListAsync();
		}
		#endregion

		/*
		#region 基本操作
		/// <summary>
		/// 创建表
		/// </summary>
		/// <param name="objectName">创建对象的类名</param>
		/// <returns>Created=0;Migrated=1</returns>
		private Task<CreateTableResult> CreatTable(string objectName)
		{
			Type type = Type.GetType(objectName);
			var result = Database.CreateTableAsync(type);
			return result;
		}

		/// <summary>
		/// 创建表
		/// </summary>
		/// <returns>Created=0;Migrated=1</returns>
		private async Task<CreateTableResult> CreatTable()
		{
			var result = await Database.CreateTableAsync<T>();
			return result;
		}

		/// <summary>
		/// 删除表
		/// </summary>
		/// <param name="objectName">创建对象的类名</param>
		/// <returns></returns>
		private Task<int> DropTable(string objectName)
		{
			Type type = Type.GetType(objectName);
			TableMapping map = new TableMapping(type);
			var result = Database.DropTableAsync(map);
			return result;
		}

		/// <summary>
		/// 删除表
		/// </summary>
		/// <returns></returns>
		private Task<int> DropTable()
		{
			return Database.DropTableAsync<T>();
		}

		/// <summary>
		/// 获取表中所有数据
		/// </summary>
		/// <returns></returns>
		private Task<List<T>> GetAllItems()
		{
			return database.Table().ToListAsync();
		}

		private Task<List<T>> Query<T>(string sql)
		{
			return Database.QueryAsync<T>(sql);
		}
		#endregion*/

	}
}
