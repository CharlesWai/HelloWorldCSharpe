using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppForAndroid.Services;

namespace MauiAppForAndroid.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        private IVibration vibrator = Vibration.Default;
        private ICANService _canService = null;

        private string _sendOutContent = string.Empty;
        public string SendOutContent { get => _sendOutContent; set => SetProperty(ref _sendOutContent,value); }

        private bool _connected = false;
        public bool Connected { get => _connected; set => SetProperty(ref _connected,value); }
        private ICommand _connectCommand;
        public ICommand ConnectCommand { get=>_connectCommand??new RelayCommand(() => {
            if (!_connected)
            {
                Connected = (bool)_canService?.Connect();
            }
            else
            {
                _canService?.Disconnect();
                Connected = _canService.Connected;
            }
        }); set=>SetProperty(ref _connectCommand,value); }
        private ICommand _sendCommand;
        public ICommand SendCommand
        {
            get => _sendCommand ?? new RelayCommand(() => {
                if (!_canService.Connected)
                {
                    return;
                }
                //_canService.GetCANInfo();
                _canService.SendOnce(SendOutContent,false);
            }); set => SetProperty(ref _sendCommand, value);
        }
        public MainPageViewModel()
        {
            //_canService = new CANservice();
        }
    }
}
