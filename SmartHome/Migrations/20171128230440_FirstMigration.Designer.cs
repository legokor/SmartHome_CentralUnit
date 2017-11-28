using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SmartHome;

namespace SmartHome.Migrations
{
    [DbContext(typeof(Model))]
    [Migration("20171128230440_FirstMigration")]
    partial class FirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("SmartHome.DataSample", b =>
                {
                    b.Property<Guid>("SamplingId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("CoLevel");

                    b.Property<double>("Humidity");

                    b.Property<double>("LpgLevel");

                    b.Property<int>("Movement");

                    b.Property<int>("SenderId");

                    b.Property<double>("SmokeLevel");

                    b.Property<double>("Temperature");

                    b.Property<DateTime>("TimeStamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasAnnotation("Sqlite:DefaultValueSql", "CURRENT_TIMESTAMP");

                    b.HasKey("SamplingId");

                    b.ToTable("DataSample");
                });

            modelBuilder.Entity("SmartHome.User", b =>
                {
                    b.Property<string>("email")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("accesLevel");

                    b.Property<string>("password")
                        .IsRequired();

                    b.Property<string>("salt")
                        .IsRequired();

                    b.HasKey("email");

                    b.ToTable("User");
                });
        }
    }
}
