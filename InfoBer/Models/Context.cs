using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoBer.Models
{
    public class Context:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Departement> Departements { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public Context(DbContextOptions<Context> options)
            : base(options)
        { }
    }
}
