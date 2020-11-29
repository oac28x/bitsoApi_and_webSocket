using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebSocket.DataModels
{
    public class Fee
    {
        [JsonProperty("book")]
        public string Book { get; set; }

        [JsonProperty("taker_fee_percent")]
        public string TakerFeePercet { get; set; }

        [JsonProperty("maker_fee_percent")]
        public string MakerFeePercet { get; set; }

        [JsonProperty("taker_fee_decimal")]
        public string TakerFeeDecimal { get; set; }

        [JsonProperty("maker_fee_decimal")]
        public string MakerFeeDecimal { get; set; }


        public decimal FeeTakerPercentDecimal { get { return Convert.ToDecimal(TakerFeePercet); } }
        public decimal FeeMakerPercentDecimal { get { return Convert.ToDecimal(MakerFeePercet); } }
        public decimal FeeTakerDecimalAsDecimal { get { return Convert.ToDecimal(TakerFeeDecimal); } }
        public decimal FeeMakerDecimalASDecimal { get { return Convert.ToDecimal(MakerFeeDecimal); } }
    }


    public class FeeInfo
    {
        [JsonProperty("fees")]
        public Fee[] Fees { get; set; }

        [JsonProperty("withdrawal_fees")]
        public Dictionary<string, string> WithdrawalFees { get; set; }

        public Dictionary<string, decimal> WithdrawalFeesAsDecimal
        {
            get
            {
                if (_withdrawalFeesAsDecimal == null)
                {
                    _withdrawalFeesAsDecimal = new Dictionary<string, decimal>();
                    foreach (var fee in WithdrawalFees)
                        _withdrawalFeesAsDecimal.Add(fee.Key, Convert.ToDecimal(fee.Value));
                }
                return _withdrawalFeesAsDecimal;
            }
        }
        private Dictionary<string, decimal> _withdrawalFeesAsDecimal = null;

    }
}
