using System;
using WebSocket.DataBase.ODMs;
using WebSocket.DataModels;

namespace WebSocket.Interfaces
{
    public interface ITelegramReporter
    {
        void SendCoinMessage(CoinDataModel coinData, bool type);
        void SendMessage(string message);
    }

    public interface IPrivateTelegramReporter
    {
        void SendSellingMessage(BitsoSell coinData);
        void SendBuyingMessage(BitsoBuy buyData);
        void SendMessageTest(string message);
    }
}
