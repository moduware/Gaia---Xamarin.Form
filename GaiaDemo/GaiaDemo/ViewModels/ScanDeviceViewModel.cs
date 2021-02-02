using System;
using Plugin.BLE.Abstractions.Contracts;

namespace GaiaDemo
{
    public class ScanDeviceViewModel
    {
        public IDevice Device { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Rssi { get; set; }

        // public event PropertyChangedEventHandler PropertyChanged;
    }
}
