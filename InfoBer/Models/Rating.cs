using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoBer.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public double Result { get; set; }
        public string Commentary { get; set; }
        public  User User { get; set; }
    }
}
