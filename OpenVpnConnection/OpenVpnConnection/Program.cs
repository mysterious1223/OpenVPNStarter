using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenVpnConnection
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                Process.Start("C:\\Program Files\\OpenVPN\bin\\openvpn-gui.exe", $"--connect client");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Process failed to start");
                Console.ReadKey();
                return;
            }

            NetworkInterface vpn_adap = null;

            Console.WriteLine("[+] Searching for Open VPN network adapter... ");


            var adapter = NetworkInterface.GetAllNetworkInterfaces();




            foreach (var adapt in adapter)
            {
                Console.WriteLine("[i] "+adapt.Name+" [d] "+adapt.Description);

                if (adapt.Description.Contains ("TAP") && adapt.Name.Contains ("Local Area Connection"))
                {
                   // Console.WriteLine("FOUND [i] " + adapt.Name + " [d] " + adapt.Description);

                    vpn_adap = adapt;

                    break;

                }

            }


            if (vpn_adap == null)
            {
                Console.Error.WriteLine("[!] Failed to locate Open VPN Connection");
                return;
            }


            Console.WriteLine("[+] VPN Adapter located! ");

            Console.WriteLine("[~] Waiting for adapter ...");
            while (vpn_adap.OperationalStatus != OperationalStatus.Up)
            {

                Task.Delay(2000);

                Console.Write(".");
                

            }

            Console.WriteLine("[+] Connection completed! ");

            Console.ReadKey();

        }
    }
}
