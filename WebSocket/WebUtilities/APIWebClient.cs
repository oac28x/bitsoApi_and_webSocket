using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using WebSocket.ExtensionMethods;

namespace WebSocket.WebUtilities
{
    public class APIWebClient
    {
        //Create your Bitso API auth data and replace
        const string bitsoKey = "BitsoKey";
        const string bitsoSecret = "BitsoSecret";

        const string baseUrl = "https://api.bitso.com";
        const string apiVersion = "/v3/";

        const string accStatus = "account_status";  //Account status, documents uploaded and transactions limits

        const string balance = "balance";     //Balance of all currencies
        const string fees = "fees";           //All books fee

        const string ledger = "ledger";       //All User operations
        const string ledgerTrades = "ledger/trades";           //All trades
        const string ledgerFees = "ledger/fees";               //All fees
        const string ledgerFundings = "ledger/fundings";       //All fundings
        const string ledgerWithdrawals = "ledger/withdrawals"; // All withdrawals
  

        public APIWebClient()
        {

        }

        public void RunTest()
        {
            //Test, retrives all currencies account balance.
            Client(null, balance, "GET");
        }

        private void Client(object payload, string reqPath, string method = "GET")
        {
            long nonce = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            string message = nonce + method + apiVersion + reqPath;

            if(method == "POST")
            {
                message += JsonConvert.SerializeObject(payload);
            }

            string signature = GetSHA(message, bitsoSecret);
            string authHeader = $"Bitso {bitsoKey}:{nonce}:{signature}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Concat(baseUrl, apiVersion, reqPath));
            request.Method = method;
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, authHeader);

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            // Do something with responseBody
                            Console.WriteLine(responseBody);
                            System.Diagnostics.Debug.WriteLine(responseBody);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
        }

        private string GetSHA(string message, string secret)
        {
            Encoding encoding = Encoding.UTF8;
            string result = string.Empty;
            byte[] keyByte = encoding.GetBytes(secret);

            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashArray = hmacsha256.ComputeHash(encoding.GetBytes(message));
                result = hashArray.ArrToString();
            }
            return result;
        }
    }  
}
