using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using WebSocket.DataBase.ODMs;
using WebSocket.Interfaces;

namespace WebSocket.DataBase
{
    public class SQLiteControler : ISQLiteService, IDisposable
    {
        protected SQLiteAsyncConnection db;

        public SQLiteControler()
        {

            string DBDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BitsoCrypto");
            if (!Directory.Exists(DBDir)) Directory.CreateDirectory(DBDir);

            db = new SQLiteAsyncConnection(Path.Combine(DBDir, "BitsoSQlite.db3"));

            Console.WriteLine($"SQLite Database Inicializada, path: {DBDir}");
        }


        public void CreateTable<T>() where T: Transaction, new()
        {
            db.CreateTableAsync<T>().Wait();  //Wait till table is created sync
        }

        public void DeleteTable<T>() where T : Transaction, new()
        {
            db.DropTableAsync<T>().Wait();  //Wait till table is deleted sync
        }

        public Task<List<T>> GetItemsAsync<T>() where T : Transaction, new()
        {
            return db.Table<T>().ToListAsync();
        }

        public Task<int> SaveItemAsync<T>(T element) where T : Transaction, new()
        {
            if (element.ID != 0)
            {
                return db.UpdateAsync(element);
            }
            else
            {
                return db.InsertAsync(element);
            }
        }

        public Task<T> GetItemByIdAsync<T>(int Id) where T : Transaction, new()
        {
            return db.Table<T>().Where(i => i.ID == Id).FirstOrDefaultAsync();
        }

        public AsyncTableQuery<T> GetTable<T>() where T : Transaction, new()
        {
            return db.Table<T>();
        }

        public Task<int> DeleteItemAsync<T>(T item) where T : Transaction, new()
        {
            return db.DeleteAsync(item);
        }

        public void Dispose()
        {
            db?.CloseAsync();
            GC.SuppressFinalize(this);
        }
    }
}
