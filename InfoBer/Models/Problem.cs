using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InfoBer.Models
{
    public class Problem
    {
        public int Id { get; set; }
        public string ProblemTitle { get; set; }
        public User Writer { get; set; }
        public User Taker { get; set; }
        public bool status { get; set; }
        public string IdChat { get; set; }
    }
}
