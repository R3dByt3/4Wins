using System;

namespace Networking.Contracts
{
    public interface IServerSocket
    {
        int Port { get; set; }
        Action<ISocketPackage> ReceivedCallback { get; set; }

        void Close();
        void Send<T>(T data);
        void StartListening();
    }
}