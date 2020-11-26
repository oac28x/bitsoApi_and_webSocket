using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using WebSocket.DataBase.ODMs;

namespace WebSocket.Interfaces
{
    public interface ISQLiteService
    {
        void CreateTable<T>() where T : Transaction, new();
        void DeleteTable<T>() where T : Transaction, new();

        Task<List<T>> GetItemsAsync<T>() where T : Transaction, new();
        Task<int> SaveItemAsync<T>(T element) where T : Transaction, new();
        Task<T> GetItemByIdAsync<T>(int Id) where T : Transaction, new();
        Task<int> DeleteItemAsync<T>(T item) where T : Transaction, new();

        AsyncTableQuery<T> GetTable<T>() where T : Transaction, new();
    }
}
