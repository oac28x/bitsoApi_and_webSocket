using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebSocket.WebUtilities
{
    public class APIWebSoketChannels
    {
        ClientWebSocket client;
        CancellationTokenSource cts;

        public APIWebSoketChannels()
        {
            client = new ClientWebSocket();
            cts = new CancellationTokenSource();
        }

        public void Init()
        {
            ConnectToServerAsync();
        }

        public void Stop()
        {
            cts.Cancel();
        }

        async void ConnectToServerAsync()
        {
            await client.ConnectAsync(new Uri("wss://ws.bitso.com"), cts.Token);

            //Subscribe to Channels
            SendSubscribeAsync(new { action = "subscribe", book = "btc_mxn", type = "trades" });
            SendSubscribeAsync(new { action = "subscribe", book = "eth_mxn", type = "trades" });
            SendSubscribeAsync(new { action = "subscribe", book = "ltc_mxn", type = "trades" });
            SendSubscribeAsync(new { action = "subscribe", book = "xrp_mxn", type = "trades" });
            SendSubscribeAsync(new { action = "subscribe", book = "gnt_mxn", type = "trades" });
            SendSubscribeAsync(new { action = "subscribe", book = "bat_mxn", type = "trades" });
            SendSubscribeAsync(new { action = "subscribe", book = "bch_mxn", type = "trades" });
            SendSubscribeAsync(new { action = "subscribe", book = "dai_mxn", type = "trades" });
            SendSubscribeAsync(new { action = "subscribe", book = "mana_mxn", type = "trades" });

            await Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await ReadMessage();
                }
            }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        async Task ReadMessage()
        {
            WebSocketReceiveResult result;
            var message = new ArraySegment<byte>(new byte[4096]);
            do
            {
                result = await client.ReceiveAsync(message, cts.Token);
                if (result.MessageType != WebSocketMessageType.Text)
                    break;
                var messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
                string receivedMessage = Encoding.UTF8.GetString(messageBytes);
                Console.WriteLine("Received: {0}", receivedMessage);
            }
            while (!result.EndOfMessage);
        }

        /// <summary>
        /// Method serealize object and make call SendAsync
        /// </summary>
        /// <param name="data">Objec data Bitso needs to suscribe to channel</param>
        async void SendSubscribeAsync(object data)
        {
            string message = JsonConvert.SerializeObject(data);
            byte[] byteMessage = Encoding.UTF8.GetBytes(message);
            var segmnet = new ArraySegment<byte>(byteMessage);

            await client.SendAsync(segmnet, WebSocketMessageType.Text, true, cts.Token);
        }
    }
}
