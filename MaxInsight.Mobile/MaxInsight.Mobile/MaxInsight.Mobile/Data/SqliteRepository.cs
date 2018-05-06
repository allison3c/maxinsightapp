using MaxInsight.Mobile.Domain;
using SQLite.Net;
using SQLite.Net.Async;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Data
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
        SQLiteAsyncConnection GetAsyncConnection();
    }
    public class SqliteRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ISQLite _sqlite;

        public SqliteRepository(ISQLite sqlite)
        {
            _sqlite = sqlite;
        }
        public SQLiteAsyncConnection GetAsyncConnection()
        {
            return _sqlite.GetAsyncConnection();
        }
        public SQLiteConnection GetConnection()
        {
            return _sqlite.GetConnection();
        }
        public TableQuery<T> Table
        {
            get
            {
                return _sqlite.GetConnection().Table<T>();
            }
        }
        public AsyncTableQuery<T> AsyncTable
        {

            get
            {
                return _sqlite.GetAsyncConnection().Table<T>();
            }
        }
        public IEnumerable<T> TableWithChildren
        {
            get
            {
                return _sqlite.GetConnection().GetAllWithChildren<T>();
                //return _sqlite.GetAsyncConnection().GetAllWithChildrenAsync<T>();
            }
        }
        public Task<List<T>> TableWithChildrenAsync
        {
            get
            {
                return _sqlite.GetAsyncConnection().GetAllWithChildrenAsync<T>();
            }
        }
        public void Delete(T entity)
        {
            _sqlite.GetConnection().Delete(entity);
        }
        public int Insert(T entity)
        {
            return _sqlite.GetConnection().Insert(entity);
        }
        public void Update(T entity)
        {
            _sqlite.GetConnection().Update(entity);
        }
        public void InsertWithChildren(T entity)
        {
            _sqlite.GetConnection().InsertOrReplaceWithChildren(entity);
        }
        public void UpdateWithChildren(T entity)
        {
            _sqlite.GetConnection().UpdateWithChildren(entity);
        }
        public IList<T> QueryListForTask(string sql, object[] param)
        {
            return _sqlite.GetConnection().Query<T>(sql, param);
        }
        public Task<List<T>> QueryListForTaskAsync(string sql, object[] param)
        {
            return _sqlite.GetAsyncConnection().QueryAsync<T>(sql, param);
        }
        public T QueryDtoForTask(string sql, object[] param)
        {
            return _sqlite.GetConnection().Query<T>(sql, param).FirstOrDefault();
        }
        T IRepository<T>.Get(string sql, object[] para)
        {
            return _sqlite.GetConnection().Query<T>(sql, para).ToList<T>().FirstOrDefault();
        }
        T IRepository<T>.GetWithPK(object id)
        {
            return _sqlite.GetConnection().GetWithChildren<T>(id);
        }
        public int Update(string query, object[] obj)
        {
            return _sqlite.GetConnection().Execute(query, obj);
        }
        public int InsertAll(List<T> entity)
        {
            return _sqlite.GetConnection().InsertOrReplace(entity, typeof(T));
        }
        public int UpdateAll(List<T> entity)
        {
            return _sqlite.GetConnection().UpdateAll(entity, true);
        }
        public Task<int> InsertAllAsync(List<T> entity)
        {
            return _sqlite.GetAsyncConnection().InsertOrReplaceAllAsync(entity);
        }
        public bool GetTableInfo(string tableName)
        {
            var info = _sqlite.GetConnection().GetTableInfo(tableName);
            if (info == null || info.Count == 0)
            {
                return false; //不存在
            }

            return true;
        }
        public void CreateTable()
        {
            _sqlite.GetConnection().CreateTable<T>();
        }
    }
}
