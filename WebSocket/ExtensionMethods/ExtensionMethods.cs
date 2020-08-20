using System;
using System.Text;

namespace WebSocket.ExtensionMethods
{
    public static class ExtensionMethods
    {
        public static string ArrToString(this byte[] arr)
        {
            string sBytes = string.Empty;
            for (int i = 0; i < arr.Length; i++)
            {
                sBytes += arr[i].ToString("x2");
            }
            return sBytes;
        }
    }
}
