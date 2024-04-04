using RFID_Client.RFID_Server.RequestHandler;
using System.Diagnostics;
using System.Net;

namespace RFID_Client.RFID_Server
{
    internal class RfidServerHelper
    {
        public readonly ushort Port;

        private readonly HttpListener _Client = new();
        private CancellationTokenSource? _Cts;

        private static readonly IReadOnlyList<IRequestHandler> _Handlers =
        [
            new PingRequestHandler(),
            new DisplayRequestHandler(),
        ];

        public RfidServerHelper(ushort port)
        {
            _Client.Prefixes.Add($"http://+:{port}/");
            Port = port;
        }

        public void Start()
        {
            Stop();
            _Cts = new CancellationTokenSource();

            Debug.WriteLine("Started Server.");
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    _Client.Start();
                }
                catch (Exception ex)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.ResetColor();
                    Console.WriteLine(ex);

                    Console.ReadKey();
                }
                while (true)
                {
                    HttpListenerContext Context = _Client.GetContext();
                    IRequestHandler? Handler = _Handlers.FirstOrDefault(handler => handler.CanHandleRequest(Context.Request));
                    if (Handler != null) await Handler.HandleRequest(Context.Request, Context.Response);
                    else await Context.Response.NotFoundAsync();
                }

            }, _Cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public void Stop()
        {
            _Client?.Stop();
            _Cts?.Cancel();
            _Cts?.Dispose();
        }
    }
}
