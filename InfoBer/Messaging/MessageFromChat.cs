using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoBer.Messaging
{
    public class MessageFromChat
    {
        public int Id { get; set; }
        public string user_id { get; set; }
        public string text { get; set; }
        public string created_at { get; set; }
    }
}
