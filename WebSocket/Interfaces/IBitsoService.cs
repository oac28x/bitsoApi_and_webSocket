using System;
namespace WebSocket.Interfaces
{
    public interface IBitsoService
    {
        void SetEnvironment(bool production);
        string SendRequest(string url, string method, bool signRequest = true, string body = null);
    }
}
