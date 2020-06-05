using NetStandard.Logger;
using Networking.Contracts;
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
    public class BroadCaster : IBroadCaster
    {
        private readonly ILogger _logger;
        private readonly string _ip;

        public BroadCaster(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateFileLogger();
            _ip = GetAllLocalIPv4(NetworkInterfaceType.Ethernet).Union(GetAllLocalIPv4(NetworkInterfaceType.Wireless80211)).First();
        }

        private IEnumerable<string> GetAllLocalIPv4(NetworkInterfaceType type)
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

        public string Listen()
        {
            var server = new UdpClient(8888);
            var response = Encoding.ASCII.GetBytes("LISTEN");

            var clientEp = new IPEndPoint(IPAddress.Any, 0);
            var clientRequestData = server.Receive(ref clientEp);
            var clientRequest = Encoding.ASCII.GetString(clientRequestData);

            Console.WriteLine("Received {0} from {1}, sending response", clientRequest, clientEp.Address.ToString());
            server.Send(response, response.Length, clientEp);
            server.Close();

            return clientEp.Address.ToString();
        }

        public string Search()
        {
            var client = new UdpClient();
            var requestData = Encoding.ASCII.GetBytes("SEARCH");
            var serverEp = new IPEndPoint(IPAddress.Any, 0);

            client.EnableBroadcast = true;
            client.Send(requestData, requestData.Length, new IPEndPoint(IPAddress.Broadcast, 8888));

            var serverResponseData = client.Receive(ref serverEp);
            var serverResponse = Encoding.ASCII.GetString(serverResponseData);
            Console.WriteLine("Received {0} from {1}", serverResponse, serverEp.Address.ToString());

            client.Close();

            return serverEp.Address.ToString();
        }
    }
}
