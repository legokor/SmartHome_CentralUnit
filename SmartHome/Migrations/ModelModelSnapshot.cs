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

            modelBuilder.Entity("SmartHome.Sampling", b =>
                {
                    b.Property<int>("samplingId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("coLevel");

                    b.Property<int>("humidity");

                    b.Property<int>("lpgLevel");

                    b.Property<int>("movement");

                    b.Property<int>("senderId");

                    b.Property<int>("smokeLevel");

                    b.Property<int>("temperature");

                    b.Property<DateTime>("time")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasAnnotation("Sqlite:DefaultValueSql", "CURRENT_TIMESTAMP");

                    b.HasKey("samplingId");

                    b.ToTable("Sampling");
                });
        }
    }
}
