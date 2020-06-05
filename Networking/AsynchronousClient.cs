using NetStandard.IO.Compression;
using NetStandard.Logger;
using Networking.Contracts;
using Newtonsoft.Json;
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
    public class AsynchronousClient : IAsynchronousClient
    {
        // The port number for the remote device.  
        public int Port { get; set; } = 11000;
        public string Ip { get; set; }

        public Action<ISocketPackage> ReceivedCallback { get; set; }

        // ManualResetEvent instances signal completion.  
        private readonly ManualResetEvent _connected =
            new ManualResetEvent(false);
        private readonly ManualResetEvent _sended =
            new ManualResetEvent(false);
        private readonly ManualResetEvent _received =
            new ManualResetEvent(false);

        // The response from the remote device.  

        private readonly ICompressor _compressor;
        private readonly ILogger _logger;

        private int _responseLength;

        private Socket _client;

        public AsynchronousClient(ICompressor compressor, ILoggerFactory loggerFactory)
        {
            _compressor = compressor;
            _logger = loggerFactory.CreateFileLogger();
        }

        public void Connect()
        {
            try
            {
                // Establish the remote endpoint for the socket.  
                // The name of the   
                // remote device is "host.contoso.com".
                IPAddress ipAddress = IPAddress.Parse(Ip);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, Port);

                Console.WriteLine("Attempting to connect on {0}", remoteEP.ToString());

                // Create a TCP/IP socket.  
                _client = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.  
                _client.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), _client);
            }
            catch (Exception ex)
            {
                _logger.Error("Could not establish connection", ex);
            }
        }

        public void Disconnect()
        {
            // Release the socket.  
            _client.Shutdown(SocketShutdown.Both);
            _client.Close();
        }

        /// <summary>
        /// Sends the request to the server.
        /// </summary>
        /// <typeparam name="ObjectType">The type of the object type.</typeparam>
        /// <param name="Method">int for request type { 1: </param>
        /// <returns></returns>
        public void Send<T>(T data)
        {
            // Connect to a remote device.  
            try
            {
                bool isConnected = false;
                for (int counter = 0; counter < 15; counter++)
                {
                    if (_client.Connected)
                    {
                        isConnected = true;
                        break;
                    }
                    Thread.Sleep(100);
                }
                if (!isConnected)
                {
                    _client.Shutdown(SocketShutdown.Both);
                    _client.Close();
                    throw new Exception("Failed to connect.");
                }
                _connected.WaitOne();

                var bytes = _compressor.Compress(data);

                byte[] length = BitConverter.GetBytes(bytes.Length);

                Send(_client, length);
                _sended.WaitOne();

                // Send data to the remote device.  
                Send(_client, bytes);
                _sended.WaitOne();

                //for (int counter = 0; counter < 15; counter++)
                //{
                //    if (client.Connected)
                //    {
                //        isConnected = true;
                //        break;
                //    }
                //    Thread.Sleep(100);
                //}
                //if (!isConnected)
                //{
                //    client.Shutdown(SocketShutdown.Both);
                //    client.Close();
                //    return default(T2);
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                _connected.Set();
            }
            catch (Exception ex)
            {
                _logger.Error("Connect callback error", ex);
            }
        }

        private void Receive(Socket client)
        {
            try
            {
                // Create the state object.  
                StateObject state = new StateObject()
                {
                    WorkSocket = client
                };

                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception ex)
            {
                _logger.Error("Receive error", ex);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket   
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.WorkSocket;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead == 4 && _responseLength == 0)
                {
                    byte[] data = new byte[bytesRead];
                    Array.Copy(state.Buffer, data, bytesRead);
                    state.Data.AddRange(data);

                    _responseLength = BitConverter.ToInt32(state.Data.ToArray(), 0);

                    state.Data.Clear();

                    client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else if (bytesRead > 0)
                {
                    byte[] data = new byte[bytesRead];
                    Array.Copy(state.Buffer, data, bytesRead);
                    state.Data.AddRange(data);

                    if (state.Data.Count == _responseLength)
                    {
                        var response = _compressor.DeCompress<SocketPackage>(state.Data.ToArray());
                        _received.Set();

                        _responseLength = 0;

                        ReceivedCallback(response);

                        return;
                    }

                    client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Receive error", ex);
            }
        }

        private void Send(Socket client, byte[] bytes)
        {
            // Convert the string data to byte data using ASCII encoding.  
            //byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device. 

            client.BeginSend(bytes, 0, bytes.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);

                //Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.  
                _sended.Set();
            }
            catch (Exception ex)
            {
                _logger.Error("Send error", ex);
            }
        }

        //private bool IsConnected(Socket socket)
        //{
        //    try
        //    {
        //        return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
        //    }
        //    catch (SocketException)
        //    {
        //        return false;
        //    }
        //}
    }
}
