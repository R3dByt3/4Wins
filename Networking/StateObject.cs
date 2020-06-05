using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public class StateObject
    {
        private Socket _workSocket = null;
        private const int _bufferSize = 1024;
        private List<byte> _data = new List<byte>();
        private byte[] _buffer = new byte[_bufferSize];

        public Socket WorkSocket { get => _workSocket; set => _workSocket = value; }

        public static int BufferSize => _bufferSize;

#pragma warning disable CA1819 // Properties should not return arrays
        public byte[] Buffer { get => _buffer; set => _buffer = value; }
#pragma warning restore CA1819 // Properties should not return arrays
        public List<byte> Data { get => _data; set => _data = value; }
    }
}
