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
        public DbSet<Sample> Samples { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sample>()
                 .Property(b => b.time)
                 .ValueGeneratedOnAddOrUpdate()
                 .IsConcurrencyToken()
                 .ForSqliteHasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<Sample>()
                 .Property(p => p.samplingId)
                 .IsRequired();

        }
        protected override void OnConfiguring
   (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Samples.db");
        }
    }
    

    [Table("Sample")]
    class Sample
    {
        [Key]
        public int samplingId { get; set; }
        [Required]
        public int senderId { get; set; }
        public int temperature { get; set; }
        public int humidity { get; set; }
        public int coLevel { get; set; }
        public int smokeLevel { get; set; }
        public int lpgLevel { get; set; }
        public DateTime time { get; set; }

        public int movement { get; set; }
    }

}
