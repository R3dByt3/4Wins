using System;

namespace Networking.Contracts
{
    public interface IAsynchronousClient
    {
        int Port { get; set; }
        string Ip { get; set; }

        Action<ISocketPackage> ReceivedCallback { get; set; }

        void Connect();
        void Disconnect();
        void Send<T>(T data);
    }
}