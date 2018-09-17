using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoBer.Models
{
    public class School
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public string Director { get; set; }
        public string Email { get; set; }
        public List<Departement> Departements { get; set; }
    }
}
