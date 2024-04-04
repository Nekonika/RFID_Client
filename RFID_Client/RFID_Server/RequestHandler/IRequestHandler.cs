using System.Net;

namespace RFID_Client.RFID_Server.RequestHandler
{
    internal interface IRequestHandler
    {
        public HttpMethod HttpMethod { get; }

        public string Endpoint { get; }

        public bool CanHandleRequest(HttpListenerRequest request);

        public Task HandleRequest(HttpListenerRequest request, HttpListenerResponse response);
    }
}
