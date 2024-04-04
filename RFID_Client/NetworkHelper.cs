using NetTools;
using System.Diagnostics;
using System.Net;

namespace RFID_Client
{
    internal static class NetworkHelper
    {
        public static bool TryFindServerAddress(out string? ipAddress, string localIp, ushort port, byte subnetMaskBits = 24)
        {
            ipAddress = null;

            IPAddressRange IpAddresses = IPAddressRange.Parse($"{localIp}/{subnetMaskBits}");
            IEnumerator<IPAddress> IpAddressesEnumerator = IpAddresses.GetEnumerator();

            HttpClient Client = new() { Timeout = TimeSpan.FromMilliseconds(200) };

            CancellationTokenSource IpFoundCts = new();
            while (IpAddressesEnumerator.MoveNext())
            {
                Debug.WriteLine($"Checking IP: " + IpAddressesEnumerator.Current.ToString());

                try
                {
                    HttpResponseMessage Message = Client.GetAsync($"http://{IpAddressesEnumerator.Current}:{port}/ping", IpFoundCts.Token).GetAwaiter().GetResult();
                    if (Message.StatusCode != HttpStatusCode.OK)
                        continue;

                    string ResponseMessage = Message.Content.ReadAsStringAsync(IpFoundCts.Token).GetAwaiter().GetResult();
                    if (ResponseMessage != "pong")
                        continue;

                    ipAddress = IpAddressesEnumerator.Current.ToString();
                    return true;
                } catch { }
            }

            return false;
        }

        //private static void Test()
        //{
        //    foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        //    {
        //        Console.WriteLine(ni.Name);
        //        Console.WriteLine("Operational? {0}", ni.OperationalStatus == OperationalStatus.Up);
        //        Console.WriteLine("MAC: {0}", ni.GetPhysicalAddress());
        //        Console.WriteLine("Gateways:");
        //        foreach (GatewayIPAddressInformation gipi in ni.GetIPProperties().GatewayAddresses)
        //        {
        //            Console.WriteLine("\t{0}", gipi.Address);
        //        }
        //        Console.WriteLine("IP Addresses:");
        //        foreach (UnicastIPAddressInformation uipi in ni.GetIPProperties().UnicastAddresses)
        //        {
        //            Console.WriteLine("\t{0} / {1}", uipi.Address, uipi.IPv4Mask);
        //        }
        //    }
        //}
    }
}
