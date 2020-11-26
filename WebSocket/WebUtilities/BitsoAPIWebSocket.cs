using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebSocket.DataModels;
using WebSocket.Interfaces;

namespace WebSocket.WebUtilities
{
    public class BitsoAPIWebSocket : IAPIWebSocket
    {
        public event EventHandler<string> OnTradeMessage;

        private ClientWebSocket client;
        private CancellationTokenSource cts;
        private string[] suscriptions;

        private volatile bool readMessages;

        //Services
        protected readonly ITelegramReporter tr;

        public BitsoAPIWebSocket(ITelegramReporter _tr)
        {
            tr = _tr;
        }

        public void Init(params string[] _coinsSuscription)
        {
            if (_coinsSuscription?.Length >= 1)
            {
                Console.WriteLine("Inicializando WebSocket...");
                suscriptions = _coinsSuscription;
                Restart();
            }
            else
            {
                Console.WriteLine("Es necesario seleccionar monedas.");
            }
        }

        public void Stop()
        {
            cts.Cancel();
        }

        public void Restart()
        {
            readMessages = false;
            client?.Dispose();
            cts?.Dispose();
            client = null;
            cts = null;

            client = new ClientWebSocket(); ;
            cts = new CancellationTokenSource();
            readMessages = true;

            ConnectToServerAsync(suscriptions);
        }

        private async void ConnectToServerAsync(string[] coins)
        {
            await client.ConnectAsync(new Uri("wss://ws.bitso.com"), cts.Token);

            foreach (string coin in coins)
            {
                SendSubscribeAsync(new { action = "subscribe", book = coin, type = "trades" });
            }

            await Task.Factory.StartNew( async () =>
            {
                while (readMessages)
                {
                    await ReadMessage();
                }
            }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private async Task ReadMessage()
        {
            WebSocketReceiveResult result;
            var message = new ArraySegment<byte>(new byte[4096]);
            do
            {
                result = await client.ReceiveAsync(message, cts.Token);
                if (result.MessageType != WebSocketMessageType.Text) break;

                var messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
                string receivedMessage = Encoding.UTF8.GetString(messageBytes);

                BitsoTradeDataModel bookData = JsonConvert.DeserializeObject<BitsoTradeDataModel>(receivedMessage);

                if(!string.IsNullOrEmpty(bookData.Book))
                {
                    OnTradeMessage?.Invoke(bookData, bookData.Book);
                }
            }
            while (!result.EndOfMessage);
        }

        /// <summary>
        /// Method serealize object and make call SendAsync
        /// </summary>
        /// <param name="data">Objec data Bitso needs to suscribe to channel</param>
        private async void SendSubscribeAsync(object data)
        {
            string message = JsonConvert.SerializeObject(data);
            byte[] byteMessage = Encoding.UTF8.GetBytes(message);
            var segmnet = new ArraySegment<byte>(byteMessage);

            await client.SendAsync(segmnet, WebSocketMessageType.Text, true, cts.Token);
        }
    }
}
