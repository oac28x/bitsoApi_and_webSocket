using System;
using WebSocket.DataBase;
using WebSocket.Interfaces;
using WebSocket.WebUtilities;

namespace WebSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            TelegramReporter TelegramBot = new TelegramReporter();
            try
            {
                APIWebSoket BitoWebSocket = new APIWebSoket(TelegramBot);

                BitsoNotifier liveTrades = new BitsoNotifier(TelegramBot, BitoWebSocket);
                liveTrades.Init();
            }
            catch //(Exception ex)
            {
                TelegramBot.SendMessage("Error, revisar log...");
            }

            //Persistence Realm Testing ->>

            //RealmControler rc = new RealmControler();
            //rc.test();

            Console.ReadLine();
        }
    }
}
