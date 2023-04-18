using MauiAppForAndroid.Extensions;
using MauiAppForAndroid.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppForAndroid.Services
{
    public class DebugService : IDebugService
    {
        private Socket _socket;
        public event Action<SimpleData> DataReceived;
        private byte[] _bytes = new byte[1024];
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();


        public bool Connected { get => (bool)_socket?.Connected; }
        private string _ip;
        private int _port;
        public string DebugIp { get => _ip; set => _ip = value; }
        public int Port { get => _port; set => _port = value; }

        public DebugService()
        {
            InitSoc();
        }
        private void InitSoc()
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _socket.ReceiveTimeout = 0;
            }
            catch (Exception ex)
            {

            }

            //Task.Run(() =>
            //{
            //    while (!_cancellationTokenSource.IsCancellationRequested)
            //    {
            //        Thread.Sleep(1);
            //        if (_socket.Connected)
            //        {
            //            int count = _socket.ReceiveAsync((ArraySegment<byte>)_bytes).Result;
            //            if (count > 0)
            //            {
            //                byte[] realBytes = null;
            //                Array.ConstrainedCopy(_bytes, 0, realBytes, 0, count);
            //                string msg = Encoding.UTF8.GetString(_bytes, 0, count);
            //                string hexMsg = string.Join(" ", realBytes);
            //                Debug.WriteLine($"Time: {DateTime.Now} , received message: {msg}, receive bytes:{hexMsg}");
            //                SimpleData data = new SimpleData() { DtNow = DateTime.Now, Message = msg, HexMessage = hexMsg };
            //                DataReceived?.Invoke(data);
            //            }
            //    }
            //}
            //}, _cancellationTokenSource.Token);
        }
        public bool Connect()
        {
            try
            {
                //_socket.Connect(Dns.GetHostName(), 58121);
                _socket.ConnectAsync(DebugIp, Port);
            }
            catch (Exception ex)
            {
                _socket?.Shutdown(SocketShutdown.Both);
            }
            return _socket.Connected;
        }

        public bool Disconnect()
        {
            try
            {
                _socket.Disconnect(false);
            }
            catch
            {
                return false;
            }
            return !_socket.Connected;
        }

        public void GetCANInfo()
        {
            try
            {
                //0xAA 01 AA BB CC DD 94
                _socket?.Send(new byte[] {
                0x94,
                0xdd,
                0xcc,
                0xbb,
                0xaa,
                0x01,
                0xaa
            });
            }
            catch (Exception ex)
            {
                _socket?.Shutdown(SocketShutdown.Both);
            }
        }

        public void Dispose()
        {
            _socket?.Dispose();
        }

        public void SendOnce(string msg, bool hexStr = false)
        {
            if (!hexStr)
            {
                _socket?.Send(Encoding.UTF8.GetBytes(msg));
            }
            else
            {
                byte[] bytes = msg.ToBytesFromHexString();
                _socket?.Send(bytes);
            }
        }
    }
}
