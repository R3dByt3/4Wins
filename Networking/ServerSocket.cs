using NetStandard.IO.Compression;
using Networking.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Networking
{
    public class ServerSocket : IServerSocket
    {
        public int Port { get; set; }

        private readonly ICompressor _compressor;

        private ManualResetEvent _allDone = new ManualResetEvent(false);
        private Socket _listener;
        private int _responseLength;

        public Action<ISocketPackage> ReceivedCallback { get; set; }

        public ServerSocket(ICompressor compressor)
        {
            _compressor = compressor;
        }

        public void StartListening()
        {
            Thread thread = new Thread(() =>
            {
                // Data buffer for incoming data.  
                byte[] bytes = new byte[1024];

                // Establish the local endpoint for the socket.  
                // The DNS name of the computer  
                // running the listener is "host.contoso.com".  
                //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                //IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, Port);

                // Create a TCP/IP socket.  
                _listener = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Bind the socket to the local endpoint and listen for incoming connections.  
                try
                {
                    _listener.Bind(localEndPoint);
                    _listener.Listen(1);

                    while (true)
                    {
                        // Set the event to nonsignaled state.  
                        _allDone.Reset();

                        // Start an asynchronous socket to listen for connections.  
                        Console.WriteLine("Waiting for a connection... on {0}", localEndPoint.ToString());
                        _listener.BeginAccept(
                            new AsyncCallback(AcceptCallback),
                            _listener);

                        // Wait until a connection is made before continuing.  
                        _allDone.WaitOne();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                Console.WriteLine("NETWORKING FAILED!");
            });
            thread.Start();
        }

        public void Close()
        {
            _listener?.Close();
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                // Signal the main thread to continue.  
                _allDone.Set();
                Console.WriteLine("Client Connected!");
                // Get the socket that handles the client request.  
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                // Create the state object.  
                StateObject state = new StateObject()
                {
                    WorkSocket = handler
                };
                handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the handler socket  
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.WorkSocket;

                // Read data from the client socket.   
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead == 4 && _responseLength == 0)
                {
                    byte[] data = new byte[bytesRead];
                    Array.Copy(state.Buffer, data, bytesRead);
                    state.Data.AddRange(data);

                    _responseLength = BitConverter.ToInt32(state.Data.ToArray(), 0);

                    state.Data.Clear();

                    handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                }
                else if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.
                    byte[] data = new byte[bytesRead];
                    Array.Copy(state.Buffer, data, bytesRead);
                    state.Data.AddRange(data);

                    if (state.Data.Count == _responseLength)
                    {
                        _responseLength = 0;

                        SocketPackage request = _compressor.DeCompress<SocketPackage>(data);
                        // All the data has been read from the   
                        // client. Display it on the console.  

                        ReceivedCallback(request);

                        return;
                    }

                    handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Send<T>(T data)
        {
            var byteData = _compressor.Compress(data);
            var dataLenght = BitConverter.GetBytes(byteData.Length);

            Send(dataLenght);
            Send(byteData);
        }

        private void Send(byte[] byteData)
        {
            try
            {
                // Convert the string data to byte data using ASCII encoding.  
                //byte[] byteData = Encoding.ASCII.GetBytes(data);

                // Begin sending the data to the remote device.  
                _listener.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), _listener);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
