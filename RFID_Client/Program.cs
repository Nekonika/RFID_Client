using System.Text;
using RFID_Client.RFID_Server;

namespace RFID_Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 3 ||
                !byte.TryParse(args[1], out byte SubnetmaskBits) || SubnetmaskBits <= 0 || SubnetmaskBits >= 32 ||
                !ushort.TryParse(args[2], out ushort Port))
            {
                Console.WriteLine("""
                    Usage: RFID_Client [localIp] [subnetmaskBits] [port]
                    """);

                return;
            }

            // create server for display
            RfidServerHelper Server = new(80);
            Server.Start();

            // Clear Screen
            DisplayHelper.BufferToScreen(true);

            Console.ReadKey();

            while (true)
            {
                // Set Searching Server Display
                DisplayHelper.TextToBuffer(DisplayHelper.Screens.SearchingServer);
                DisplayHelper.SetForegroundColor(ConsoleColor.Red);
                DisplayHelper.BufferToScreen(true);

                string? IpAddress;
                while (true)
                { 
                    if (NetworkHelper.TryFindServerAddress(out IpAddress, args[0], Port, SubnetmaskBits))
                        break;
                }

                DisplayHelper.TextToBuffer(DisplayHelper.Screens.AwaitingRfid);
                DisplayHelper.SetForegroundColor(ConsoleColor.Green);
                DisplayHelper.BufferToScreen();
            
                RfidClientHelper ClientHelper = new(IpAddress!, Port);
                while (true)
                {
                    string? Input = ReadSerialSilent();
                    if (string.IsNullOrWhiteSpace(Input)) continue;

                    //DateTime Now = DateTime.Now;
                    //if (_SerialCooldown.TryGetValue(Input, out DateTime NextCoffeeAfter) && NextCoffeeAfter >= Now)
                    //{
                    //    TimeSpan Delta = NextCoffeeAfter - Now;
                    //    Console.WriteLine($"Your Serial Number is on cooldown. You've got {Delta:mm\\:ss} Minutes remaining.");

                    //    continue;
                    //}
                
                    //_SerialCooldown[Input] = Now.AddMinutes(5);

                    // TODO: maybe check the input first.

                    try { ClientHelper.Send(Input); }
                    catch (Exception) { break; }
                }
            }
        }

        private static string ReadSerialSilent()
        {
            StringBuilder Sb = new();
            while (true)
            { 
                ConsoleKeyInfo KeyInfo = Console.ReadKey(true);
                if (KeyInfo.Key == ConsoleKey.Enter) break;

                if (_AllowedKeys.Contains(KeyInfo.Key))
                    Sb.Append(KeyInfo.KeyChar);
            }

            return Sb.ToString();
        }

        private static ConsoleKey[] _AllowedKeys = [
            ConsoleKey.A,
            ConsoleKey.B,
            ConsoleKey.C,
            ConsoleKey.D,
            ConsoleKey.E,
            ConsoleKey.F,
            ConsoleKey.D0,
            ConsoleKey.D1,
            ConsoleKey.D2,
            ConsoleKey.D3,
            ConsoleKey.D4,
            ConsoleKey.D5,
            ConsoleKey.D6,
            ConsoleKey.D7,
            ConsoleKey.D8,
            ConsoleKey.D9,
        ];
    }
}
