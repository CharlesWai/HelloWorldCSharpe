using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppForAndroid.Models;
using MauiAppForAndroid.Services;

namespace MauiAppForAndroid.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        private IVibration vibrator = Vibration.Default;
        private ICANService _canService = null;
        private ObservableCollection<SimpleData> _receivedData = new ObservableCollection<SimpleData>();
        public ObservableCollection<SimpleData> ReceivedData { get => _receivedData; set => SetProperty(ref _receivedData, value); }

        public ICANService Service { get => _canService; set => SetProperty(ref _canService,value); }

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
                    Connected = (bool)_canService?.Connect();
                    if (Connected)
                    {
                        _canService.DataReceived += OnDataReceived;
                    }
                }
                else
                {
                    _canService?.Disconnect();
                    Connected = _canService.Connected;
                    if (!Connected)
                    {
                        _canService.DataReceived -= OnDataReceived;
                    }
                }
            }); 
            set => SetProperty(ref _connectCommand, value);
        }
        private ICommand _sendCommand;
        public ICommand SendCommand
        {
            get => _sendCommand ?? new RelayCommand(() =>
            {
                if (!_canService.Connected)
                {
                    return;
                }
                //_canService.GetCANInfo();
                _canService.SendOnce(SendOutContent, HexSendOut);
            }); set => SetProperty(ref _sendCommand, value);
        }
        public MainPageViewModel()
        {
            _canService = new CANservice();
        }

        private void OnDataReceived(SimpleData obj)
        {
            App.Current.Dispatcher.Dispatch(() => { ReceivedData.Add(obj); });
        }
    }
}
