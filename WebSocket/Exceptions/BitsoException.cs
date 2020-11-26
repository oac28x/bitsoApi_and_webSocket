using System;
namespace WebSocket.Exceptions
{
    public class BitsoException : Exception
    {
        public string ErrorCode { get; set; }

        public BitsoException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
