using System.Net;
using System.Web;

namespace RFID_Client
{
    internal class RfidClientHelper
    {
        public readonly string Address;
        public readonly ushort Port;

        private readonly HttpClient _Client = new();

        public RfidClientHelper(string address, ushort port)
        {
            Address = address;
            Port = port;
        }

        public void Send(string message)
        {
            string Message = HttpUtility.UrlEncode(message);
            _Client.GetAsync($"http://{Address}:{Port}?serial={Message}");
        }
    }
}
