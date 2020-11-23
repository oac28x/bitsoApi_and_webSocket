using System;
using System.Collections.Generic;
using WebSocket.WebUtilities;

namespace WebSocket.DataModels
{
    public class CoinDataModel
    {
        public string CoinName { get; set; }

        private decimal price;
        public decimal Price
        {
            get => price;
            set
            {
                price = value;
                PriceHistory.Add(value);
            }
        }

        public decimal LastPrice { get; set; }

        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal Promedio { get; set; }

        public uint CountTradesUp { get; set; }
        public uint CountTradesDown { get; set; }
        public uint CountTradesTotal { get; set; }

        public uint LastCountTradesUp { get; set; }
        public uint LastCountTradesDown { get; set; }
        public uint LastCountTradesTotal { get; set; }

        public uint MinuteCountUp { get; set; }
        public uint MinuteCountDown { get; set; }
        public uint MinuteCountTotal { get; set; }

        public decimal MinuteAmountTrades { get; set; }
        public decimal MinuteAmountTradesUp { get; set; }
        public decimal MinuteAmountTradesDown { get; set; }

        public List<decimal> PriceHistory { get; private set; }

        public CoinDataModel(string coinName)
        {
            CoinName = coinName;
            PriceHistory = new List<decimal>();
            SetInitValues();
        }

        public void SetInitValues()
        {
            price = 0;
            LastPrice = 0;
            MinPrice = 0;
            MaxPrice = 0;
            Promedio = 0;
            CountTradesUp = 0;
            CountTradesDown = 0;
            CountTradesTotal = 0;
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
}
