using System;

namespace GaiaDemo
{
    public class DeviceModel
    {
        public DeviceModel(Guid _uuid, string _name, int _rssi)
        {
            UUID = _uuid;
            Name = _name;
            Rssi = _rssi;
        }

        public Guid UUID { private set; get; }
        public string Name { private set; get; }
        public int Rssi { private set; get; }
    }
}
