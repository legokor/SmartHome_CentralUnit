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
        public DbSet<User> Users { get; set; }

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
            optionsBuilder.UseSqlite("Filename=database.db");
        }
    }
    

    [Table("Sample")]
    class Sample
    {
        [Key]
        public int samplingId { get; set; }
        [Required]
        public int senderId { get; set; }
        public double temperature { get; set; }
        public double humidity { get; set; }
        public double coLevel { get; set; }
        public double smokeLevel { get; set; }
        public double lpgLevel { get; set; }
        public DateTime time { get; set; }

        public int movement { get; set; }
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
