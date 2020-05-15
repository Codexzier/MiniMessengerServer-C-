using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniMessengerServer
{
    internal class DeviceManager
    {
        private List<DeviceItem> _deviceItems = new List<DeviceItem>();

        public DeviceManager()
        {
        }

        internal object SendCommand(long deviceId, long deviceValue, out string message)
        {
            var deviceItem = this._deviceItems.FirstOrDefault(f => f.Id.Equals(deviceId));

            if(deviceItem == null)
            {
                message = "no device found";
                return false;
            }

            deviceItem.LastChange = DateTime.Now;
            deviceItem.Value = deviceValue;

            message = "OK";
            return true;
        }

        internal long GetValue(long deviceId, out string message)
        {
            message = "OK";
            var deviceItem = this._deviceItems.FirstOrDefault(f => f.Id.Equals(deviceId));

            if(deviceItem == null)
            {
                deviceItem = new DeviceItem();
                deviceItem.Id = deviceId;
                deviceItem.Value = 0L;
                deviceItem.LastChange = DateTime.Now;

                this._deviceItems.Add(deviceItem);
                message = "create item";
            }

            return deviceItem.Value;
        }

        internal IEnumerable<DeviceItem> GetDevices() => this._deviceItems;
    }
}