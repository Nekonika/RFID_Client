using System.Net;

namespace RFID_Client.RFID_Server
{
    internal static class HttpListenerResponseExt
    {
        public static async Task OkAsync(this HttpListenerResponse response, HttpContent? content = null)
        {
            response.SetStatus(HttpStatusCode.OK);
            await response.CloseResponseAsync(content);
        }

        public static async Task NotFoundAsync(this HttpListenerResponse response, HttpContent? content = null)
        {
            response.SetStatus(HttpStatusCode.NotFound);
            await response.CloseResponseAsync(content);
        }

        public static async Task BadRequestAsync(this HttpListenerResponse response, HttpContent? content = null)
        {
            response.SetStatus(HttpStatusCode.BadRequest);
            await response.CloseResponseAsync(content);
        }

        public static async Task InternalServerErrorAsync(this HttpListenerResponse response, HttpContent? content = null)
        {
            response.SetStatus(HttpStatusCode.InternalServerError);
            await response.CloseResponseAsync(content);
        }

        private static void SetStatus(this HttpListenerResponse response, HttpStatusCode status)
        {
            response.StatusCode = (int)status;
            response.StatusDescription = Enum.GetName(status) ?? string.Empty;
        }

        private static async Task CloseResponseAsync(this HttpListenerResponse response, HttpContent? content = null)
        {
            if (content != null)
            {
                byte[] Content = await content.ReadAsByteArrayAsync();
                response.Close(Content, false);
            }
            else
            {
                response.Close();
            }
        }
    }
}
