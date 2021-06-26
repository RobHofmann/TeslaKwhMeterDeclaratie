using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeslaKwhMeter.Base.Models.Data;

namespace TeslaKwhMeter.Data
{
    public class KwhStandContext : DbContext
    {
        public KwhStandContext(DbContextOptions<KwhStandContext> options) : base(options)
        { }

        public DbSet<KwhStand> KwhStands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KwhStand>().ToTable("KwhStand");
        }
    }
}
