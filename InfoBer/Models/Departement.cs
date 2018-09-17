using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoBer.Models
{
    public class Departement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public virtual School School { get; set; }
    }
}
