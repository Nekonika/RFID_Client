using System.Net;

namespace RFID_Client.RFID_Server.RequestHandler
{
    internal class PingRequestHandler : IRequestHandler
    {
        public HttpMethod HttpMethod => HttpMethod.Get;

        public string Endpoint => "/ping";

        public bool CanHandleRequest(HttpListenerRequest request)
        {
            if (request.HttpMethod != HttpMethod.Method) return false;
            if (request.Url?.AbsolutePath != Endpoint) return false;

            return true;
        }

        public async Task HandleRequest(HttpListenerRequest request, HttpListenerResponse response)
            => await response.OkAsync(new StringContent("pong"));
    }
}
