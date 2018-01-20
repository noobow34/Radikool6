using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Radikool6.Classes;

namespace Radikool6.Entities
{
    public class Db : DbContext
    {
        public DbSet<Hash> Hashes { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<Station> Stations { get; set; }

        private bool _notInjection = false;
        public Db(DbContextOptions<Db> options) : base(options)
        {
        }

        public Db() : base()
        {
            _notInjection = true;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (_notInjection)
            {
                options.UseSqlite($"Data Source={Define.File.DbFile}");
            }
        }
    }
}
