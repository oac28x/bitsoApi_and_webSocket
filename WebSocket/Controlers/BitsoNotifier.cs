using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebSocket.DataModels;
using WebSocket.Interfaces;

namespace WebSocket.Controlers
{
    public class BitsoNotifier : IBitsoNotifier, IDisposable
    {
        //Coin with dataStructure
        protected readonly Dictionary<string, CoinDataModel> coinsData;

        //Services
        protected ITelegramReporter tr;
        protected IAPIWebSocket APIWs;

        private DateTime controlDate;
        private DateTime timeTrading;

        private Timer timer;

        public event EventHandler<CoinDataModel> OnTradeUp;
        public event EventHandler<CoinDataModel> OnTradeDown;
        public event EventHandler<CoinDataModel> OnLotTradeUp;

        public BitsoNotifier(ITelegramReporter _tr, IAPIWebSocket _APIWs)
        {
            tr = _tr;
            APIWs = _APIWs;

            coinsData = new Dictionary<string, CoinDataModel>();
        }

        public void Init(params string[] _coinsSuscription)
        {

            if (_coinsSuscription?.Length >= 1)
            {
                Console.WriteLine("Inicializando notifier...");
            }
            else
            {
                Console.WriteLine("Es necesario seleccionar monedas.");
                return;
            }

            controlDate = DateTime.Now;
            timeTrading = DateTime.Now;

            foreach (string coin in _coinsSuscription)
            {
                coinsData.Add(coin, new CoinDataModel(coin.ToUpper()));
            }

            //APIWs.Init(coinTypes);
            APIWs.OnTradeMessage += OnTrade;

            
            TimerState timerState = new TimerState { Counter = 0 };
            timer = new Timer(
                callback: new TimerCallback(CheckMinuteData),
                state: timerState,
                dueTime: 0,
                period: 60000);
        }

        private void CheckMinuteData(object timerState)
        {
            Task.Run(() => {
                foreach (KeyValuePair<string, CoinDataModel> cd in coinsData)
                {
                    CoinDataModel coinInfo = cd.Value;

                    coinInfo.MinuteCountTotal = coinInfo.CountTradesTotal - coinInfo.LastCountTradesTotal;
                    coinInfo.MinuteCountDown = coinInfo.CountTradesDown - coinInfo.LastCountTradesDown;
                    coinInfo.MinuteCountUp = coinInfo.CountTradesUp - coinInfo.LastCountTradesUp;

                    if (coinInfo.MinuteCountUp > 20 && coinInfo.LastPrice > 0)
                    {
                        tr?.SendCoinMessage(coinInfo, true);
                        OnLotTradeUp?.Invoke(this, coinInfo);
                    }

                    if (coinInfo.MinuteCountDown > 20 && coinInfo.LastPrice > 0)
                    {
                        tr?.SendCoinMessage(coinInfo, false);
                    }

                    coinInfo.LastCountTradesTotal = coinInfo.CountTradesTotal;
                    coinInfo.LastCountTradesUp = coinInfo.CountTradesUp;
                    coinInfo.LastCountTradesDown = coinInfo.CountTradesDown;
                    coinInfo.LastPrice = coinInfo.Price;

                    coinInfo.MinuteAmountTrades = 0;
                    coinInfo.MinuteAmountTradesUp = 0;
                    coinInfo.MinuteAmountTradesDown = 0;
                }
            }).ConfigureAwait(false);
            

            if (controlDate.Day != DateTime.Now.Day)
            {
                ResetData();
                controlDate = DateTime.Now;
                tr?.SendMessage(">> Limpieza de datos, media noche...");
            }

            if (DateTime.Now.Subtract(timeTrading).TotalMinutes > 5)
            {
                APIWs.Restart();
                timeTrading = DateTime.Now;
                tr?.SendMessage(">> Servicio se reinició...");
            }

        }

        private void OnTrade(object sender, string book)
        {
            BitsoTradeDataModel bookData = sender as BitsoTradeDataModel;
            timeTrading = DateTime.Now;

            CoinMath(bookData.Payload, coinsData[book]);
        }

        private void CoinMath(BookPayload[] bd, CoinDataModel cd)
        {
            foreach (BookPayload p in bd)
            {
                string Ttype;

                cd.CountTradesTotal++;
                cd.Price = p.PriceMXN;
                cd.MinuteAmountTrades += p.AmountMXN;

                cd.Promedio = decimal.Round(cd.PriceHistory.Average(), 2, MidpointRounding.AwayFromZero);
                cd.MinPrice = cd.PriceHistory.Min();
                cd.MaxPrice = cd.PriceHistory.Max();

                if (p.Type)
                {
                    Ttype = "Venta  - Alza";
                    cd.CountTradesUp++;
                    cd.MinuteAmountTradesUp += p.AmountMXN;
                    OnTradeUp?.Invoke(this, cd);
                }
                else
                {
                    Ttype = "Compra - Baja";
                    cd.CountTradesDown++;
                    cd.MinuteAmountTradesDown += p.AmountMXN;
                    OnTradeDown?.Invoke(this, cd);
                }

                Console.WriteLine($"{timeTrading.Hour.ToString("D2")}:{timeTrading.Minute.ToString("D2")}:{timeTrading.Second.ToString("D2")} {cd.CoinName} {Ttype}");
            }
        }

        private void ResetData()
        {
            foreach (KeyValuePair<string, CoinDataModel> cd in coinsData)
            {
                cd.Value.SetInitValues();
            }
        }

        public void Dispose()
        {
            //Disposing process call GC

            timer?.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Class used in timer parameter
        /// </summary>
        class TimerState
        {
            public int Counter;
        }
    }
}
