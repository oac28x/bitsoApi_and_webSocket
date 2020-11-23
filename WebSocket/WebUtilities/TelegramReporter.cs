using System;
using System.Net;
using System.Threading.Tasks;
using WebSocket.DataModels;
using WebSocket.Interfaces;

namespace WebSocket.WebUtilities
{
    public class TelegramReporter : ITelegramReporter
    {
        const string urlBase = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}";
        const string apiToken = "BotId";
        const string chatId = "ChatId";

        public TelegramReporter()
        {
            Console.WriteLine("Telegram Reporter Incializado.");
        }

        public void SendMessage(string message)
        {
            TelegramRequestMessage(message);
        }

        public void SendCoinMessage(CoinDataModel coinData, bool type)
        {
            TelegramRequestMessage(CreateTelegramMessage(type, ref coinData));
        }

        /// <summary>
        /// Telegram WebRequest Message
        /// </summary>
        /// <param name="message">Message to send TelegramBot</param>
        private void TelegramRequestMessage(string message)
        {
            try
            {
                Task.Run(() =>
                {
                    string urlString = string.Format(urlBase, apiToken, chatId, message);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlString);
                    request.KeepAlive = false;
                    request.Timeout = 1500;


                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.ContentLength == 0 && response.StatusCode == HttpStatusCode.OK)
                        {
                            //Message sent
                        }
                        else
                        {
                            //Message error
                        }
                    }
                });
            }
            catch
            {
                Console.WriteLine(">> Telegram send coin message error...");
            }
        }

        /// <summary>
        /// Creates telegram message 
        /// </summary>
        /// <param name="type">Trade type, true = buying, false = selling</param>
        /// <param name="coinData">CoinData</param>
        /// <returns></returns>
        private string CreateTelegramMessage(bool type, ref CoinDataModel coinData)
        {
            string message = string.Empty;
            decimal percentage = decimal.Round((coinData.Price * 100 / coinData.LastPrice) - 100, 2, MidpointRounding.AwayFromZero);
            if (type)
            {
                message += $"{coinData.CoinName} ↑↑ALZA {percentage}%\n";
            }
            else
            {
                message += $"{coinData.CoinName} ↓↓BAJA {percentage}%\n";
            }

            message += $"[POld: ${coinData.LastPrice.ToString("D2")}]\n";
            message += $"[PNew: ${coinData.Price.ToString("D2")}]\n";
            message += $"[PAvg: ${coinData.Promedio.ToString("D2")}]\n";
            message += $"[Pmax: ${coinData.MaxPrice.ToString("D2")}]\n";
            message += $"[Pmin: ${coinData.MinPrice.ToString("D2")}]\n";

            message += $"[Tm: {coinData.MinuteCountTotal} - ${coinData.MinuteAmountTrades.ToString("D2")}]\n";
            message += $"[Tm↑: {coinData.MinuteCountUp} - ${coinData.MinuteAmountTradesUp.ToString("D2")}]\n";
            message += $"[Tm↓: {coinData.MinuteCountDown} - ${coinData.MinuteAmountTradesDown.ToString("D2")}]";

            return message;
        }

        public void Clean()
        {
            
        }
    }
}
