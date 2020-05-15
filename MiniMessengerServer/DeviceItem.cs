using System;

namespace MiniMessengerServer
{
    internal class DeviceItem
    {
        public long Id { get; set; }
        public long Value { get; set; }
        public DateTime LastChange { get; internal set; }
    }
}