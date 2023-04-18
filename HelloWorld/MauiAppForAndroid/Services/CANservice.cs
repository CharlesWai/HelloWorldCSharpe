using MauiAppForAndroid.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppForAndroid.Services
{
    public class CANservice : ICANService
    {
        private Socket _socket;
        private byte[] _bytes = new byte[1024];
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public bool Connected { get => (bool)_socket?.Connected; }
        public CANservice()
        {
            //InitSoc();
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
            
            Task.Run(() =>
            {
                while (_socket.Connected)
                {
                    int count = _socket.ReceiveAsync((ArraySegment<byte>)_bytes).Result;
                    if (count > 0)
                    {
                        Debug.WriteLine($"Time: {DateTime.Now} , received message: {Encoding.UTF8.GetString(_bytes, 0, count)}");
                    }
                }                
            }, _cancellationTokenSource.Token);
        }
        public bool Connect()
        {
            try
            {
                //_socket.Connect(Dns.GetHostName(), 58121);
                _socket.ConnectAsync("192.168.1.7", 58121);
            }
            catch(Exception ex) 
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

        public void SendOnce(string msg,bool hexStr = false)
        {
            if (!hexStr)
            {
                _socket?.Send(Encoding.UTF8.GetBytes(msg));
            }
            else
            {
                byte[] bytes= msg.ToBytesFromHexString();
                _socket?.Send(bytes);
            }
        }

        
    }
}
