using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoBer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Token { get; set; }
        public School School { get; set; }
        public Departement Departement { get; set; }
        public bool Admin { get; set; }
        public virtual List<Rating> Ratings { get; set; }
    }
}
