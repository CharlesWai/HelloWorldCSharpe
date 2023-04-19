using MauiAppForAndroid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppForAndroid.Services
{
    public interface ICANService
    {
        event Action<SimpleData> DataReceived;
        string DebugIp { get; set; }
        int Port { get; set; }
        bool Connected { get; }
        bool Connect();
        bool Disconnect();
        void GetCANInfo();
        void Dispose();
        void SendOnce(string msg,bool hexStr);
        void StartListen();
    }
}
