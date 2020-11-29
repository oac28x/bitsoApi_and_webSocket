using System;
using System.Net.WebSockets;
using System.Threading;
using Autofac;
using WebSocket.Controlers;
using WebSocket.DataBase;
using WebSocket.DataBase.ODMs;
using WebSocket.Interfaces;
using WebSocket.WebUtilities;



namespace WebSocket
{
    static class Program
    {
        private static IContainer container;
        public static IContainer IoC
        {
            get
            {
                if (container != null) return container;
                Init_IoC();
                return container;
            }
        }

        static void Main(string[] args)
        {
            GetInstance<Application>().Start();

            #region TestCodeCommented
            //TelegramReporter TelegramBot = new TelegramReporter();
            //try
            //{
            //    APIWebSoket BitoWebSocket = new APIWebSoket(TelegramBot);

            //    BitsoNotifier liveTrades = new BitsoNotifier(TelegramBot, BitoWebSocket);
            //    liveTrades.Init();
            //}
            //catch //(Exception ex)
            //{
            //    TelegramBot.SendMessage("Error, revisar log...");
            //}


            //Realm used for logg service
            //RealmControler realm = new RealmControler();



            //Persistence SQLite

            //SQLiteControler sqlite = new SQLiteControler();

            //sqlite.DeleteTable<BitsoBuy>();

            //sqlite.CreateTable<BitsoBuy>();

            //sqlite.SaveItemAsync<BitsoBuy>(new BitsoBuy() { Book = "mxn_btc", Date = DateTime.Now, AmountMXN = 1000.45m });


            //var x = sqlite.GetTable<BitsoBuy>().ToListAsync().Result;


            //Console.WriteLine("Cuenta = " + x.Count);

            //Persistence Realm Testing ->>
            //RealmControler rc = new RealmControler();
            //rc.test();

            #endregion

            Console.ReadLine();
        }


        public static T GetInstance<T>()
        {
            return IoC.Resolve<T>();
        }

        private static void Init_IoC()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<Application>();

            builder.RegisterType<TelegramReporter>().As<ITelegramReporter>().As<IPrivateTelegramReporter>().SingleInstance();

            builder.RegisterType<BitsoAPIWebSocket>().As<IAPIWebSocket>().SingleInstance();
            builder.RegisterType<BitsoNotifier>().As<IBitsoNotifier>().SingleInstance();

            builder.RegisterType<Bitso>().As<IBitsoService>().SingleInstance();
            builder.RegisterType<BitsoAPIPrivate>().As<IAPIPrivate>().SingleInstance();
            builder.RegisterType<BitsoAPIPublic>().As<IAPIPublic>().SingleInstance();
            builder.RegisterType<BitsoTradingControler>().As<IBitsoTrading>().SingleInstance();

            builder.RegisterType<SQLiteControler>().As<ISQLiteService>().SingleInstance();

            //Test to pass client and cts through constructor
            builder.RegisterType<ClientWebSocket>();
            builder.RegisterType<CancellationTokenSource>();

            container = builder.Build();
        }
    }
}
