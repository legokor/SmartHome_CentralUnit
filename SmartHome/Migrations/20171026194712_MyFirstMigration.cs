using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartHome.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    adminID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    email = table.Column<string>(nullable: false),
                    hash = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.adminID);
                });

            migrationBuilder.CreateTable(
                name: "Sample",
                columns: table => new
                {
                    samplingId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    coLevel = table.Column<double>(nullable: false),
                    humidity = table.Column<double>(nullable: false),
                    lpgLevel = table.Column<double>(nullable: false),
                    movement = table.Column<int>(nullable: false),
                    senderId = table.Column<int>(nullable: false),
                    smokeLevel = table.Column<double>(nullable: false),
                    temperature = table.Column<double>(nullable: false),
                    time = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sample", x => x.samplingId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    userID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    accesLevel = table.Column<int>(nullable: false),
                    email = table.Column<string>(nullable: false),
                    hash = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.userID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "Sample");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
