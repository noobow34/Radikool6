using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radikool6.Entities
{
    public class Db : DbContext
    {
        public DbSet<Program> Programs { get; set; }
        public DbSet<Station> Stations { get; set; }

        public Db(DbContextOptions<Db> options) : base(options)
        {
        }
    }
}
