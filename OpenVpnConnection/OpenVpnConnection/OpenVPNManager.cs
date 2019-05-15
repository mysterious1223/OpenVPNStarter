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
    class OpenVPNManager
    {
        NetworkInterface[] adapter;
        NetworkInterface vpn_adap = null;
        string devName;
        string NetLocName;
        string pathtoexe;
        public OpenVPNManager(string pathToVpnGUI)
        {
            devName = "TAP";
            NetLocName = "Local Area Connection";
            pathtoexe = pathToVpnGUI;

        }


        public bool ConnectToOpenVPN ()
        {
            try
            {
                Process.Start(pathtoexe, @"--connect client");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Process failed to start");
                Console.ReadKey();
                return false;
            }
            // must refresh list
            RefreshAdapterList();

            foreach (var adapt in adapter)
            {


                if (adapt.Description.Contains(devName) && adapt.Name.Contains(NetLocName))
                {
                    // Console.WriteLine("FOUND [i] " + adapt.Name + " [d] " + adapt.Description);

                    vpn_adap = adapt;

                    break;

                }

            }

            if (vpn_adap == null)
            {
                Console.Error.WriteLine("[!] Failed to locate / connect Open VPN Connection");
                return false;
            }

            Console.WriteLine("[+] VPN Adapter located! ");

            Console.WriteLine("[~] Waiting for adapter ...");

            bool good = false;

            while (true)
            {

                good = GetCurrentOpenVPNStatus();

                if (good)
                    break;

            }

            Console.WriteLine("[+] Connection completed! ");

            return true;
        }

        public bool DisconnectFromOpenVPN()
        {
            //RefreshAdapterList();

            try
            {
                Process.Start(pathtoexe, @"--command disconnect_all");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Process failed to stop");
                Console.ReadKey();
                return false;
            }

            Console.WriteLine("[+] Successfully disconnected from OpenVPN server!"); 
            return true;
        }
        public bool GetCurrentOpenVPNStatus ()
        {

            // will loop through and keep rechecking
            RefreshAdapterList();

            foreach (var adapt in adapter)
            {
                //Console.WriteLine("[i] " + adapt.Name + " [d] " + adapt.Description);
                Thread.Sleep(5000);

                if (adapt.Description.Contains(devName) && adapt.Name.Contains(NetLocName))
                {
                    // Console.WriteLine("FOUND [i] " + adapt.Name + " [d] " + adapt.Description);

                    vpn_adap = adapt;

                    if (vpn_adap.OperationalStatus != OperationalStatus.Up)
                    {
                        Console.WriteLine("Checking VPN connection... " + vpn_adap.OperationalStatus.ToString());
                        return false;
                    }
                    else
                    {

                        Console.WriteLine("Connection up!");

                        return true;
                    }

                    //break;

                }

            }


            throw new System.ArgumentException("Couldn't located indicated device");
           // return false;
        }
        public void RefreshAdapterList ()
        {
            adapter = NetworkInterface.GetAllNetworkInterfaces();

        }



    }
}
