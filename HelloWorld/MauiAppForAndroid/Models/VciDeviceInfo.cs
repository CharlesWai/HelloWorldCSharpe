using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppForAndroid.Models
{
    public class VciDeviceInfo:ObservableObject
    {
        public UInt16 Opcode { get; set; }
        public byte[] Mac { get; set; }
        public string DeviceName { get; set; }
        public UInt16 HardwareVersion { get; set; }
        public UInt16 SoftWareVersion { get; set; }
        public byte[] SerialNumber { get; set; }
        public byte WifiWorkMode { get; set; }
        public byte WifiAuthEncry { get; set; }
        public string WifiSsid { get; set; }
        public string WifiPassword { get; set; }
        public byte[] VciIp { get; set; }
        public byte[] VciSubMask { get; set; }
        public byte[] VciGateIp { get; set; }
        public UInt16 VciPort { get; set; }
        public string UserName { get; set; }
        public UInt16 VciNotConnectTime { get; set; }
    }
}
