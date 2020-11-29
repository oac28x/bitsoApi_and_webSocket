using System;
using WebSocket.DataModels;

namespace WebSocket.Interfaces
{
    public interface IAPIPublic
    {
        BookInfo[] GetAvailableBooks();
        Ticker GetTicker(string book = "btc_mxn");
        OrderBook GetOrderBook(string book = "btc_mxn", bool aggregate = true);
        Trade[] GetTrades(string book = "btc_mxn", string marker = "", string sort = "desc", int limit = 25);
    }
}
