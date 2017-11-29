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

            modelBuilder.Entity("SmartHome.DataSample", b =>
                {
                    b.Property<Guid>("SamplingId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("CoLevel");

                    b.Property<double>("Humidity");

                    b.Property<double>("LpgLevel");

                    b.Property<int>("Movement");

                    b.Property<string>("SenderId")
                        .IsRequired();

                    b.Property<double>("SmokeLevel");

                    b.Property<double>("Temperature");

                    b.Property<DateTime>("TimeStamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasAnnotation("Sqlite:DefaultValueSql", "CURRENT_TIMESTAMP");

                    b.HasKey("SamplingId");

                    b.ToTable("DataSample");
                });

            modelBuilder.Entity("SmartHome.Unit", b =>
                {
                    b.Property<string>("BuiltinID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Auto");

                    b.Property<string>("Id");

                    b.Property<string>("Location");

                    b.Property<string>("Type");

                    b.HasKey("BuiltinID");

                    b.ToTable("Unit");
                });

            modelBuilder.Entity("SmartHome.User", b =>
                {
                    b.Property<string>("Email")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EncryptedPassword")
                        .IsRequired();

                    b.Property<int>("Level");

                    b.Property<string>("Salt")
                        .IsRequired();

                    b.HasKey("Email");

                    b.ToTable("User");
                });
        }
    }
}
