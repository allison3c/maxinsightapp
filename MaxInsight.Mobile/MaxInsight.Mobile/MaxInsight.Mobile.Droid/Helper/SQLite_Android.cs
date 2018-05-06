using SQLite.Net.Platform.XamarinAndroid;
using System.IO;
using MaxInsight.Mobile.Data;
using SQLite.Net;
using SQLite.Net.Async;

namespace MaxInsight.Mobile.Droid.Helper
{
    public class SQLite_Android : ISQLite
    {
        public static SQLiteConnection _sQLiteConnection;
        public static SQLiteAsyncConnection _sQLiteAsyncConnection;
        public SQLite_Android() { }
        public SQLiteConnection GetConnection()
        {
            if (_sQLiteConnection == null)
            {
                var sqliteFilename = "MaxInsight.db3";
                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
                var path = Path.Combine(documentsPath, sqliteFilename);
                _sQLiteConnection = new SQLiteConnection(new SQLitePlatformAndroid(), path, false);
                _sQLiteConnection.Execute(@"PRAGMA synchronous = off;");
            }
            return _sQLiteConnection;
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            if (_sQLiteAsyncConnection == null)
            {
                var sqliteFilename = "MaxInsight.db3";
                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
                var path =Path.Combine(documentsPath, sqliteFilename);
                var connectionString = new SQLiteConnectionString(path, false);
                var connectionWithLock = new SQLiteConnectionWithLock(new SQLitePlatformAndroid(), connectionString);
                _sQLiteAsyncConnection = new SQLiteAsyncConnection(() => connectionWithLock);
            }
            return _sQLiteAsyncConnection;
        }
    }
}