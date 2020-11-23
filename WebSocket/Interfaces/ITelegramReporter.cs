using System;
using WebSocket.DataModels;

namespace WebSocket.Interfaces
{
    public interface ITelegramReporter
    {
        void SendCoinMessage(CoinDataModel coinData, bool type);
        void SendMessage(string message);

        void Clean();
    }
}
