using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;

namespace GaiaDemo
{
    public class DeviceListItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public IDevice Device { get; private set; }

        private Guid _id;
        private string _name;
        private int _rssi;
        private bool _isConnected;
        public DeviceListItemViewModel(IDevice _device)
        {
            Device = _device;
        }

        //public Guid Id => Device.Id;
        //public string Name => Device.Name;
        //public int Rssi => Device.Rssi;
        //public bool IsConnected => Device.State == DeviceState.Connected;
        
        public Guid Id
        {
            get { return _id; }
            set
            {
                _id = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Id"));
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }

        public int Rssi
        {
            get { return _rssi; }
            set
            {
                _rssi = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Rssi"));
            }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsConnected"));
            }
        }
    }
}
