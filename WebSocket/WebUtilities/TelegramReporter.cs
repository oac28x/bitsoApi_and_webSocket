using System;
using System.Net;
using System.Threading.Tasks;
using WebSocket.DataBase.ODMs;
using WebSocket.DataModels;
using WebSocket.Interfaces;

namespace WebSocket.WebUtilities
{
    public class TelegramReporter : ITelegramReporter, IPrivateTelegramReporter
    {
        //To check Telegram Bot updates, chatID's, GroupID's, etc.
        //https://api.telegram.org/bot + apiToken + /getUpdates

        const string urlBase = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}";

        //Change next to your Telegram data.
        const string apiToken = ConfigData.TelegramApiToken;
        const string publicChatId = ConfigData.TelegramPublicChatId;
        const string myChatId = ConfigData.TelegramMyChatId;

        public TelegramReporter()
        {
            Console.WriteLine("Telegram Reporter Incializado.");
        }

        public void SendMessage(string message)
        {
            SendPublicMessage(message);
        }

        public void SendCoinMessage(CoinDataModel coinData, bool type)
        {
            SendPublicMessage(CreatePublicMessage(type, ref coinData));
        }


        #region PrivateMessaging
        public void SendSellingMessage(BitsoSell coinData)
        {
            throw new NotImplementedException();
        }

        public void SendBuyingMessage(BitsoBuy buyData)
        {
            throw new NotImplementedException();
        }

        public void SendMessageTest(string message)
        {
            SendPrivateMessage(message);
        }
        #endregion


        /// <summary>
        /// SendPrivate message adapter method
        /// </summary>
        /// <param name="message">Message string</param>
        private void SendPrivateMessage(string message)
        {
            TelegramRequestMessage(message, myChatId);
        }

        /// <summary>
        /// SendPublic message adapter method
        /// </summary>
        /// <param name="message">Message string</param>
        private void SendPublicMessage(string message)
        {
            TelegramRequestMessage(message, publicChatId);
        }

        /// <summary>
        /// Telegram WebRequest Message
        /// </summary>
        /// <param name="message">Message to send TelegramBot</param>
        private void TelegramRequestMessage(string message, string _chatId)
        {
            try
            {
                Task.Run(() =>
                {
                    string urlString = string.Format(urlBase, apiToken, _chatId, message);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlString);
                    request.KeepAlive = false;
                    request.Timeout = 1500;


                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.ContentLength == 0 && response.StatusCode == HttpStatusCode.OK)
                        {
                            //Message sent action
                        }
                        else
                        {
                            //Message error action
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
        private string CreatePublicMessage(bool type, ref CoinDataModel coinData)
        {
            string message = string.Empty;
            decimal percentage = decimal.Round((coinData.Price.Price * 100 / coinData.LastPrice) - 100, 2, MidpointRounding.AwayFromZero);
            if (type)
            {
                message += $"{coinData.CoinName} ↑↑ALZA {percentage}%\n";
            }
            else
            {
                message += $"{coinData.CoinName} ↓↓BAJA {percentage}%\n";
            }

            message += $"[POld: ${coinData.LastPrice.ToString("F")}]\n";
            message += $"[PNew: ${coinData.Price.Price.ToString("F")}]\n";
            message += $"[PAvg: ${coinData.Promedio.ToString("F")}]\n";
            message += $"[Pmax: ${coinData.MaxPrice.ToString("F")}]\n";
            message += $"[Pmin: ${coinData.MinPrice.ToString("F")}]\n";

            message += $"[Tm: {coinData.MinuteCountTotal} - ${coinData.MinuteAmountTrades.ToString("F")}]\n";
            message += $"[Tm↑: {coinData.MinuteCountUp} - ${coinData.MinuteAmountTradesUp.ToString("F")}]\n";
            message += $"[Tm↓: {coinData.MinuteCountDown} - ${coinData.MinuteAmountTradesDown.ToString("F")}]";

            return message;
        }
    }
}
