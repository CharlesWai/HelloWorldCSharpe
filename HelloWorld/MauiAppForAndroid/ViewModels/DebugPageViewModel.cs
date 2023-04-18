using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppForAndroid.Models;
using MauiAppForAndroid.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiAppForAndroid.ViewModels
{
    public class DebugPageViewModel : ObservableObject
    {
        private IVibration vibrator = Vibration.Default;
        private IDebugService _debugService = null;

        private ObservableCollection<SimpleData> _receivedData = new ObservableCollection<SimpleData>();
        public ObservableCollection<SimpleData> ReceivedData { get => _receivedData; set => SetProperty(ref _receivedData, value); }

        private string _debugIp = string.Empty;
        public string DebugIp
        {
            get => _debugIp;
            set
            {
                bool flag = SetProperty(ref _debugIp, value);
                if (flag)
                {
                    _debugService.DebugIp = value;
                }
            }
        }

        private string _debugPort = string.Empty;
        public string DebugPort
        {
            get => _debugPort;
            set
            {
                var num = int.Parse(value);
                if (num < UInt16.MinValue || num > UInt16.MaxValue)
                {
                    return;
                }
                bool flag = SetProperty(ref _debugPort, value);
                if (flag)
                {
                    _debugService.Port = num;
                }
            }
        }
        private bool _hexSendOut = false;
        public bool HexSendOut { get => _hexSendOut; set => SetProperty(ref _hexSendOut, value); }

        private string _sendOutContent = string.Empty;
        public string SendOutContent { get => _sendOutContent; set => SetProperty(ref _sendOutContent, value); }

        private bool _connected = false;
        public bool Connected { get => _connected; set => SetProperty(ref _connected, value); }
        private ICommand _connectCommand;
        public ICommand ConnectCommand
        {
            get => _connectCommand ?? new RelayCommand(() =>
            {
                if (!_connected)
                {
                    Connected = (bool)_debugService?.Connect();
                }
                else
                {
                    _debugService?.Disconnect();
                    Connected = _debugService.Connected;
                }
            }); set => SetProperty(ref _connectCommand, value);
        }
        private ICommand _sendCommand;
        public ICommand SendCommand
        {
            get => _sendCommand ?? new RelayCommand(() =>
            {
                if (!_debugService.Connected)
                {
                    return;
                }
                //_canService.GetCANInfo();
                _debugService.SendOnce(SendOutContent, HexSendOut);
            }); set => SetProperty(ref _sendCommand, value);
        }
        public DebugPageViewModel()
        {
            _debugService = new DebugService();
            _debugService.DataReceived += DataReceived;
        }

        private void DataReceived(SimpleData obj)
        {
            ReceivedData.Add(obj);
        }
    }
}
