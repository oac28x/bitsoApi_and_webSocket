using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebSocket.DataModels;
using WebSocket.Enums;
using WebSocket.Interfaces;

namespace WebSocket
{
    public class BitsoNotifier : IBitsoNotifier, IDisposable
    {
        //Enum coins to array of coins
        private readonly Coin[] coinTypes = (Coin[])Enum.GetValues(typeof(Coin));

        //Coin with dataStructure
        private readonly Dictionary<Coin, CoinDataModel> coinsData;

        //Services
        private ITelegramReporter tr;
        private IAPIWebSocket APIWs;

        private DateTime controlDate;
        private DateTime timeTrading;

        public BitsoNotifier(ITelegramReporter tr, IAPIWebSocket APIWs)
        {
            this.tr = tr;
            this.APIWs = APIWs;

            coinsData = new Dictionary<Coin, CoinDataModel>();
            controlDate = DateTime.Now;
        }

        public void Init()
        {
            foreach (Coin coin in coinTypes)
            {
                coinsData.Add(coin, new CoinDataModel(coin.ToString().ToUpper()));
            }

            APIWs.Init(coinTypes);
            APIWs.OnTradeMessage += OnTrade;

            timeTrading = DateTime.Now;

            TimerState timerState = new TimerState { Counter = 0 };
            Timer timer = new Timer(
                callback: new TimerCallback(CheckMinuteData),
                state: timerState,
                dueTime: 0,
                period: 60000);
        }

        private void CheckMinuteData(object timerState)
        {
            Task.Run(() => {
                foreach (KeyValuePair<Coin, CoinDataModel> cd in coinsData)
                {
                    CoinDataModel coinInfo = cd.Value;

                    coinInfo.MinuteCountTotal = coinInfo.CountTradesTotal - coinInfo.LastCountTradesTotal;
                    coinInfo.MinuteCountDown = coinInfo.CountTradesDown - coinInfo.LastCountTradesDown;
                    coinInfo.MinuteCountUp = coinInfo.CountTradesUp - coinInfo.LastCountTradesUp;

                    if (coinInfo.MinuteCountUp > 20 && coinInfo.LastPrice > 0)
                    {
                        tr?.SendCoinMessage(coinInfo, true);
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
                tr?.SendMessage(">> Limpieza de datos, media noche..");
            }

            if (DateTime.Now.Subtract(timeTrading).TotalMinutes > 5)
            {
                APIWs.Restart();
                timeTrading = DateTime.Now;
                tr?.SendMessage(">> Servicio se reinició...");
            }

        }

        private void OnTrade(object sender, string dataMessage)
        {
            BitsoTradeDataModel bookData = sender as BitsoTradeDataModel;
            timeTrading = DateTime.Now;

            switch (bookData.Book)
            {
                case "btc_mxn":
                    CoinMath(bookData.Payload, coinsData[Coin.btc]);
                    break;
                case "eth_mxn":
                    CoinMath(bookData.Payload, coinsData[Coin.eth]);
                    break;
                case "xrp_mxn":
                    CoinMath(bookData.Payload, coinsData[Coin.xrp]);
                    break;
                case "mana_mxn":
                    CoinMath(bookData.Payload, coinsData[Coin.mana]);
                    break;
                case "ltc_mxn":
                    CoinMath(bookData.Payload, coinsData[Coin.ltc]);
                    break;
                case "bch_mxn":
                    CoinMath(bookData.Payload, coinsData[Coin.bch]);
                    break;
                case "gnt_mxn":
                    CoinMath(bookData.Payload, coinsData[Coin.gnt]);
                    break;
                case "bat_mxn":
                    CoinMath(bookData.Payload, coinsData[Coin.bat]);
                    break;
                default:

                    break;
            }
        }

        private void CoinMath(BookPayload[] bd, CoinDataModel cd)
        {
            foreach (BookPayload p in bd)
            {
                cd.CountTradesTotal++;
                cd.Price = p.PriceMXN;
                cd.MinuteAmountTrades += p.AmountMXN;

                string Ttype;

                if (p.Type)
                {
                    Ttype = "Venta  - Alza";
                    cd.CountTradesUp++;
                    cd.MinuteAmountTradesUp += p.AmountMXN;
                }
                else
                {
                    Ttype = "Compra - Baja";
                    cd.CountTradesDown++;
                    cd.MinuteAmountTradesDown += p.AmountMXN;
                }

                cd.Promedio = decimal.Round(cd.PriceHistory.Average(), 2, MidpointRounding.AwayFromZero);
                cd.MinPrice = cd.PriceHistory.Min();
                cd.MaxPrice = cd.PriceHistory.Max();

                Console.WriteLine($"{timeTrading.Hour.ToString("D2")}:{timeTrading.Minute.ToString("D2")}:{timeTrading.Second.ToString("D2")} {cd.CoinName} {Ttype}");
            }
        }

        private void ResetData()
        {
            foreach (KeyValuePair<Coin, CoinDataModel> cd in coinsData)
            {
                cd.Value.SetInitValues();
            }
        }

        public void Dispose()
        {
            //Disposing process call GC
            
            //GC.SuppressFinalize(this);
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
