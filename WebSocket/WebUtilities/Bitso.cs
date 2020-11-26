using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocket.Exceptions;
using WebSocket.Interfaces;
using WebSocket.Utilities;

namespace WebSocket.WebUtilities
{
    public class Bitso : IBitsoService
    {
        //Create your Bitso API auth data and replace
        const string BITSO_KEY = "";
        const string BITSO_SECRET = "";
        const string BITSO_API_VERSION = "/v3/";

        string BITSO_BASE_URL = string.Empty;

        public Bitso()
        {

        }

        public void SetEnvironment(bool production)
        {
            BITSO_BASE_URL = production ? "https://api.bitso.com" : "https://api-dev.bitso.com";
        }

        public string SendRequest(string url, string method, bool signRequest = true, string body = null)
        {
            if (string.IsNullOrEmpty(BITSO_API_VERSION))
            {
                Console.WriteLine("Se necesita establecer en ambiente");
            }

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(BITSO_BASE_URL + BITSO_API_VERSION + url);

            if (signRequest)
            {
                long nonce = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                string message = nonce + method + BITSO_API_VERSION + url + body;

                string signature = ExtraUtils.HMACSHA256(message, BITSO_SECRET);
                string authHeader = $"Bitso {BITSO_KEY}:{nonce}:{signature}";

                httpWebRequest.Headers.Add(HttpRequestHeader.Authorization, authHeader);
            }

            httpWebRequest.Method = method;
            httpWebRequest.ContentType = "application/json";

            if (!string.IsNullOrEmpty(body))
            {
                using (Stream req = httpWebRequest.GetRequestStream())
                {
                    byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
                    req.Write(bodyBytes, 0, bodyBytes.Length);
                }
            }

            string response = string.Empty;

            try
            {
                using (WebResponse res = httpWebRequest.GetResponse())
                {
                    using (Stream strReader = res.GetResponseStream())
                    {
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            response = objReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                using (HttpWebResponse res = (HttpWebResponse)ex.Response)
                {
                    if (res == null)
                        throw new BitsoException("No response was returned from Bitso.", "0");

                    using (var str = res.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(str))
                        {
                            response = reader.ReadToEnd();

                            if (res.StatusCode == HttpStatusCode.NotFound && response.StartsWith("<"))
                                throw new BitsoException("The requested resource was not found.", "-1");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BitsoException(ex.Message, "0");
            }

            if (string.IsNullOrEmpty(response) || response.StartsWith("<"))
                throw new BitsoException("A malformed response was returned from Bitso.", "-1");

            JObject responseObj = JsonConvert.DeserializeObject<JObject>(response);

            if (responseObj == null) throw new BitsoException("No response was returned from Bitso.", "0");

            if (responseObj["success"].Value<bool>())
            {
                if (method == "GET" && url == "balance") //This was hardcoded to mantain consistency in the response
                    return responseObj["payload"]["balances"].ToString();

                return responseObj["payload"].ToString();
            }

            throw new BitsoException(responseObj["error"]["message"].ToString(), responseObj["error"]["code"].ToString());
        }
    }
}
