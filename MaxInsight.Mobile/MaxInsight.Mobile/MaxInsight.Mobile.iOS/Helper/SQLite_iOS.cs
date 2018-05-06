using System;
using MaxInsight.Mobile.Data;
using SQLite.Net;
using SQLite.Net.Async;
using System.IO;
using SQLite.Net.Platform.XamarinIOS;

namespace MaxInsight.Mobile.iOS.Helper
{
    public class SQLite_iOS : ISQLite
    {
        private SQLiteConnection _connection;
        private SQLiteAsyncConnection _sQLiteAsyncConnection;
        public SQLite_iOS() { }

        public SQLiteConnection GetConnection()
        {
            if (_connection == null)
            {
                var dbName = "MaxInsight";
                var document = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var filePath = Path.Combine(document, dbName);
                _connection = new SQLiteConnection(new SQLitePlatformIOS(), filePath, false);
                _connection.Execute(@"PRAGMA synchronous = off;");
            }
            return _connection;
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            if (_sQLiteAsyncConnection == null)
            {
                var dbName = "MaxInsight";
                var document = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var filePath = Path.Combine(document, dbName);
                var connectionString = new SQLiteConnectionString(filePath, false);
                var connectionWithLock = new SQLiteConnectionWithLock(new SQLitePlatformIOS(), connectionString);
                _sQLiteAsyncConnection = new SQLiteAsyncConnection(() => connectionWithLock);
            }
            return _sQLiteAsyncConnection;
        }
    }
}