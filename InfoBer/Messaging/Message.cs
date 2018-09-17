using InfoBer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoBer.Messaging
{
    public class Message
    {
        public User User;
        public string text;
        public DateTime Date;
        public Message(User u,string text,string date)
        {
            User = u;
            this.text = text;
            DateTime.TryParse(date,out Date);
            Date = Date.AddHours(2);
        }
    }
}
