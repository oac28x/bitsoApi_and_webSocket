using System;
using System.Collections.Generic;
using System.Linq;
using WebSocket.WebUtilities;

namespace WebSocket.DataModels
{
    public class CoinDataModel
    {
        public string CoinName { get; set; }

        public decimal LastPrice { get; set; }

        public decimal MinPrice { get { return PriceHistory.Min(); } }
        public decimal MaxPrice { get { return PriceHistory.Max(); } }
        public decimal Promedio { get { return decimal.Round(PriceHistory.Select(x => (decimal)x).Average(), 2, MidpointRounding.AwayFromZero); } }

        public int CountTradesUp { get { return PriceHistory.Where(x => x.TradeType).Count(); } }
        public int CountTradesDown { get { return PriceHistory.Where(x => !x.TradeType).Count(); } }
        public int CountTradesTotal { get { return PriceHistory.Count(); } }

        public int LastCountTradesUp { get; set; }
        public int LastCountTradesDown { get; set; }
        public int LastCountTradesTotal { get; set; }

        public int MinuteCountUp { get; set; }
        public int MinuteCountDown { get; set; }
        public int MinuteCountTotal { get; set; }

        public decimal MinuteAmountTrades { get; set; }
        public decimal MinuteAmountTradesUp { get; set; }
        public decimal MinuteAmountTradesDown { get; set; }

        private TradePrice price = 0;
        public TradePrice Price
        {
            get => price;
            set
            {
                price = value;
                if (price.Price > 0) PriceHistory.Add(value);
                if (PriceHistory.Count > 5120) PriceHistory.RemoveAt(PriceHistory.Count - 1);
            }
        }

        public List<TradePrice> PriceHistory { get; private set; } = new List<TradePrice>();

        public CoinDataModel(string coinName) { CoinName = coinName; }

        public double RSI(int period = 50)
        {
            if (PriceHistory.Count < period) return default(float);

            List<double> sampleData = PriceHistory.Skip(PriceHistory.Count - period).Select(x => (double)x).ToList();  //Take last period

            double gainSum = 0;
            double lossSum = 0;
            for (int i = 1; i < period; i++)
            {
                double thisChange = sampleData[i] - sampleData[i - 1];
                if (thisChange > 0)
                {
                    gainSum += thisChange;
                }
                else
                {
                    lossSum += (-1) * thisChange;
                }
            }

            double averageGain = gainSum / period;
            double averageLoss = lossSum / period;

            double rs = averageGain / averageLoss;  //Here you can get RS if needed
            double rsi = 100 - (100 / (1 + rs));

            return Math.Round(rsi, 2, MidpointRounding.AwayFromZero);
        }


        public CoinDataModel Clone()
        {
            CoinDataModel cdm = new CoinDataModel(this.CoinName);
            cdm.Price = Price;
            cdm.LastPrice = LastPrice;
            cdm.LastCountTradesUp = LastCountTradesUp;
            cdm.LastCountTradesDown = LastCountTradesDown;
            cdm.LastCountTradesTotal = LastCountTradesTotal;
            cdm.MinuteCountUp = MinuteCountUp;
            cdm.MinuteCountDown = MinuteCountDown;
            cdm.MinuteCountTotal = MinuteCountTotal;
            cdm.MinuteAmountTrades = MinuteAmountTrades;
            cdm.MinuteAmountTradesUp = MinuteAmountTradesUp;
            cdm.MinuteAmountTradesDown = MinuteAmountTradesDown;
            cdm.PriceHistory = PriceHistory;
            return cdm;
        }

        public void SetInitValues()
        {
            //Price = 0;
            LastPrice = 0;
            LastCountTradesUp = 0;
            LastCountTradesDown = 0;
            LastCountTradesTotal = 0;
            MinuteCountUp = 0;
            MinuteCountDown = 0;
            MinuteCountTotal = 0;
            MinuteAmountTrades = 0;
            MinuteAmountTradesUp = 0;
            MinuteAmountTradesDown = 0;
            PriceHistory.Clear();
        }
    }

    public class TradePrice
    {
        public bool TradeType { get; set; } //True Sold - False Bought
        public decimal Price { get; set; }

        public TradePrice(decimal price, bool type)
        {
            TradeType = type;
            Price = price;
        }

        public static implicit operator decimal(TradePrice p)
        {
            return p.Price;
        }

        public static implicit operator double(TradePrice p)
        {
            return (double)p.Price;
        }

        public static implicit operator TradePrice(decimal price)
        {
            return new TradePrice(price, true);
        }

        //public override string ToString()
        //{
        //    return Price.ToString("F");
        //}
    }
}
