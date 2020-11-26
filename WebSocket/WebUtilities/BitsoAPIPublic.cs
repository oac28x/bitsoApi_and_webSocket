using System;
using Newtonsoft.Json;
using WebSocket.DataModels;
using WebSocket.Interfaces;
using WebSocket.Utilities;

namespace WebSocket.WebUtilities
{
    public class BitsoAPIPublic : BitsoAPI, IAPIPublic
    {
        public BitsoAPIPublic(IBitsoService _bistoClient) : base(_bistoClient)
        {

        }

        //https://bitso.com/api_info#available-books
        public BookInfo[] GetAvailableBooks()
        {
            var rawResponse = BitsoClient.SendRequest("available_books", "GET", false);
            return JsonConvert.DeserializeObject<BookInfo[]>(rawResponse);
        }

        //https://bitso.com/api_info#ticker
        public Ticker GetTicker(string book = "btc_mxn")
        {
            var rawResponse = BitsoClient.SendRequest($"ticker?book={book}", "GET", false);
            return JsonConvert.DeserializeObject<Ticker>(rawResponse);
        }

        //https://bitso.com/api_info#order_book
        public OrderBook GetOrderBook(string book = "btc_mxn", bool aggregate = true)
        {
            var rawResponse = BitsoClient.SendRequest($"order_book?book={book}&aggregate={(aggregate ? "true" : "false")}", "GET", false);
            return JsonConvert.DeserializeObject<OrderBook>(rawResponse);
        }

        //https://bitso.com/api_info#trades
        public Trade[] GetTrades(string book = "btc_mxn", string marker = "", string sort = "desc", int limit = 25)
        {
            var rawResponse = BitsoClient.SendRequest("trades" + ExtraUtils.BuildQueryString("book", book, "marker", marker, "sort", sort, "limit", limit.ToString()), "GET", false);
            return JsonConvert.DeserializeObject<Trade[]>(rawResponse);
        }
    }
}
