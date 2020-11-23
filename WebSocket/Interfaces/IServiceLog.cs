using System;
namespace WebSocket.Interfaces
{
    public interface IServiceLog
    {
        //Service Log Methods
        void LogDatabase(string id, DateTime date, string message);
        void LogTxt(string id, string date, string message);
        void LogTelegram(string message);
    }
}
