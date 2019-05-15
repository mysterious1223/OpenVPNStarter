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

            // initialize
            OpenVpnConnection.OpenVPNManager vpn = new OpenVpnConnection.OpenVPNManager(@"C:\Program Files\OpenVPN\bin\openvpn-gui.exe");

            // 
            // connect to vpn\
            try
            {

                vpn.ConnectToOpenVPN();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Thread.Sleep(10000);

            vpn.DisconnectFromOpenVPN();


            Console.ReadKey();
        }

    }
}
