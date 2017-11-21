using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartHome.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    email = table.Column<string>(nullable: false),
                    accesLevel = table.Column<int>(nullable: false),
                    password = table.Column<string>(nullable: false),
                    salt = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.email);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sample");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
