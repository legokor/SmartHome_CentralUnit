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

    [Table("DataSample")]
    class DataSample
    {
        [Key]
        public Guid SamplingId { get; set; }
        [Required]
        public int SenderId { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double CoLevel { get; set; }
        public double SmokeLevel { get; set; }
        public double LpgLevel { get; set; }
        public DateTime TimeStamp { get; set; }

        public int Movement { get; set; }
    }

    [Table("User")]
    class User
    {
        [Key]    
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public AccesLevel accesLevel { get; set; }
        [Required]
        public string salt { get; set; }
    }

 }
