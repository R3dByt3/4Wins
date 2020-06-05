using Networking.Contracts;
using System;

namespace Networking
{
    public class SocketPackage : ISocketPackage
    {
        public RequestType RequestType { get; set; }
        public int Value { get; set; }
    }
}