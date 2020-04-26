using System;
using System.Collections.Generic;
using System.Text;

namespace MiniMessengerServer.Data
{
    public class UserItem
    {
        public long ID { get; set; }

        public string Username { get; set; }

        public bool IsOnline { get; set; }

        public DateTime LastGet { get; set; }
    }
}
