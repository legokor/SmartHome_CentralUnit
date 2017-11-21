using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SmartHome;

namespace SmartHome.Migrations
{
    [DbContext(typeof(Model))]
    partial class ModelModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("SmartHome.Sample", b =>
                {
                    b.Property<int>("samplingId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("coLevel");

                    b.Property<double>("humidity");

                    b.Property<double>("lpgLevel");

                    b.Property<int>("movement");

                    b.Property<int>("senderId");

                    b.Property<double>("smokeLevel");

                    b.Property<double>("temperature");

                    b.Property<DateTime>("time")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasAnnotation("Sqlite:DefaultValueSql", "CURRENT_TIMESTAMP");

                    b.HasKey("samplingId");

                    b.ToTable("Sample");
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
