using MaxInsight.Mobile.Domain;
using SQLite.Net;
using SQLite.Net.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Data
{
    public interface IRepository<T> where T : BaseEntity
    {
        TableQuery<T> Table { get; }
        AsyncTableQuery<T> AsyncTable { get; }
        Task<List<T>> TableWithChildrenAsync { get; }
        SQLiteAsyncConnection GetAsyncConnection();
        SQLiteConnection GetConnection();
        IEnumerable<T> TableWithChildren { get; }
        //T GetById(object id);
        int Insert(T entity);
        //void Insert(IEnumerable<T> entities);
        void Update(T entity);
        //void Update(IEnumerable<T> entities);
        void Delete(T entity);
        void InsertWithChildren(T entity);
        void UpdateWithChildren(T entity);
        IList<T> QueryListForTask(string sql, object[] param);

        T QueryDtoForTask(string sql, object[] param);

        T Get(string sql, object[] para);
        T GetWithPK(object id);
        int Update(string query, object[] param);
        int InsertAll(List<T> entity);

        Task<int> InsertAllAsync(List<T> entity);
        Task<List<T>> QueryListForTaskAsync(string sql, object[] param);
        int UpdateAll(List<T> entity);

        bool GetTableInfo(string tableName);
        void CreateTable();



    }
}
