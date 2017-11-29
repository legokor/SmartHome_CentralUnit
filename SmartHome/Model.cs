using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Data;

namespace SmartHome
{
    class Model : DbContext
    {
        public DbSet<DataSample> DataSamples { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Unit> Units { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataSample>()
                 .Property(b => b.TimeStamp)
                 .ValueGeneratedOnAddOrUpdate()
                 .IsConcurrencyToken()
                 .ForSqliteHasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<DataSample>()
                 .Property(p => p.SamplingId)
                 .IsRequired();



        }
        protected override void OnConfiguring
   (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=database.db");
        }
    }
}
