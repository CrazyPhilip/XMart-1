using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using XMart.Models;
using System.Linq;

namespace XMart.Services
{
	public static class LocalDatabaseHelper<T> where T : new()
	{
		static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
		{
			return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
		});

		static SQLiteAsyncConnection Database => lazyInitializer.Value;
		static bool initialized = false;

		static LocalDatabaseHelper()
		{
			InitializeAsync().SafeFireAndForget(false);
		}

		static async Task InitializeAsync()
		{
			if (!initialized)
			{
				if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(SearchedItem).Name))
				{
					await Database.CreateTablesAsync(CreateFlags.None, typeof(SearchedItem)).ConfigureAwait(false);
					initialized = true;
				}

				if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Category).Name))
				{
					await Database.CreateTablesAsync(CreateFlags.None, typeof(Category)).ConfigureAwait(false);
					initialized = true;
				}
			}
		}

		#region 基本操作
		/// <summary>
		/// 创建表
		/// </summary>
		/// <param name="objectName">创建对象的类名</param>
		/// <returns>Created=0;Migrated=1</returns>
		public static async Task<CreateTableResult> CreateTable(string objectName)
		{
			Type type = Type.GetType(objectName);
			var result = await Database.CreateTableAsync(type);
			return result;
		}

		/// <summary>
		/// 创建表
		/// </summary>
		/// <returns>Created=0;Migrated=1</returns>
		public static async Task<CreateTableResult> CreateTable()
		{
			var result = await Database.CreateTableAsync<T>();
			return result;
		}

		/// <summary>
		/// 删除表
		/// </summary>
		/// <param name="objectName">创建对象的类名</param>
		/// <returns></returns>
		public static async Task<int> DropTable(string objectName)
		{
			Type type = Type.GetType(objectName);
			TableMapping map = new TableMapping(type);
			var result = await Database.DropTableAsync(map);
			return result;
		}

		/// <summary>
		/// 删除表
		/// </summary>
		/// <returns></returns>
		public static async Task<int> DropTable()
		{
			return await Database.DropTableAsync<T>();
		}

		/// <summary>
		/// 获取表中所有数据
		/// </summary>
		/// <returns></returns>
		public static async Task<List<T>> GetAllItems()
		{
			return await Database.Table<T>().ToListAsync();
		}

		public static async Task<List<T>> Query(string sql)
		{
			return await Database.QueryAsync<T>(sql);
		}

		/// <summary>
		/// 插入或替换
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static async Task<int> InsertOrReplaceAsync(T t)
		{
			return await Database.InsertOrReplaceAsync(t);
		}

		/// <summary>
		/// 全部插入
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static async Task<int> InsertAll(List<T> ts)
		{
			return await Database.InsertAllAsync(ts);
		}

		/// <summary>
		/// 删除表中所有数据
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static async Task<int> DeleteAllItems()
		{
			return await Database.DeleteAllAsync<T>();
		}
		#endregion

	}
}
