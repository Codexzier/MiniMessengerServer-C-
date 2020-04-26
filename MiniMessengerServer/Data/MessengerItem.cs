using System;
using System.Collections.Generic;
using System.Text;

namespace MiniMessengerServer.Data
{
    public class MessengerItem
    {
        public long ID { get; set; }

        public string Text { get; set; }

        public DateTime SendTime { get; set; }

        public long UserId { get; set; }

        public long ToUserId { get; set; }

        public bool FromMe { get; set; }
    }
}
