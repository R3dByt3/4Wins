using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Networking
{
    public class BroadCaster
    {
        public IEnumerable<string> GetAllLocalIPv4(NetworkInterfaceType type)
        {
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            yield return ip.Address.ToString();
                        }
                    }
                }
            }
        }

        public IPEndPoint Listen()
        {
            Stopwatch stopwatch = new Stopwatch();
            UdpClient udpServer = new UdpClient(11000);

            stopwatch.Start();
            while (stopwatch.Elapsed < TimeSpan.FromSeconds(10))
            {
                var endPoint = new IPEndPoint(IPAddress.Any, 11000);
                var data = udpServer.Receive(ref endPoint); // listen on port 11000

                var ipaddresses = GetAllLocalIPv4(NetworkInterfaceType.Ethernet).Union(GetAllLocalIPv4(NetworkInterfaceType.Wireless80211));
                var localIp = ipaddresses.FirstOrDefault();
                var bytesToSend = Encoding.ASCII.GetBytes(localIp);
                udpServer.Send(bytesToSend, bytesToSend.Length, endPoint); // reply back
                udpServer.Close();
                udpServer.Dispose();

                var ip = Encoding.ASCII.GetString(data);
                return new IPEndPoint(IPAddress.Parse(ip), 11001);
            }

            return null;
        }

        //public IPEndPoint Call()
        //{
        //    var client = new UdpClient();
        //    IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 11000); // endpoint where server is listening
        //    client.Connect(endPoint);

        //    var localIp = GetAllLocalIPv4(NetworkInterfaceType.Ethernet & NetworkInterfaceType.Wireless80211).FirstOrDefault();
        //    var bytesToSend = Encoding.ASCII.GetBytes(localIp);

        //    // send data
        //    client.Send(bytesToSend, bytesToSend.Length);

        //    // then receive data
        //    var receivedData = client.BeginReceive(ref endPoint);
        //}
    }
}
