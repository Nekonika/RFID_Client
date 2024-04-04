using Newtonsoft.Json;
using System.Net;

namespace RFID_Client.RFID_Server.RequestHandler
{
    internal class DisplayRequestHandler : IRequestHandler
    {
        public HttpMethod HttpMethod => HttpMethod.Post;

        public string Endpoint => "/display";

        public bool CanHandleRequest(HttpListenerRequest request)
        {
            if (request.HttpMethod != HttpMethod.Method) return false;
            if (request.Url?.AbsolutePath != Endpoint) return false;
            if (!request.HasEntityBody) return false;

            return true;
        }

        public async Task HandleRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            StreamReader SR = new(request.InputStream);
            string Content = SR.ReadToEnd();
            SR.Dispose();

            try
            {
                List<List<DisplayHelper.Letter>> Letters = JsonConvert.DeserializeObject<List<List<DisplayHelper.Letter>>>(Content)!;
                DisplayHelper.SetBuffer(Letters);
                DisplayHelper.BufferToScreen(true);

                await response.OkAsync();
            }
            catch (Exception)
            {
                await response.InternalServerErrorAsync();
                return;
            }
        }
    }
}
