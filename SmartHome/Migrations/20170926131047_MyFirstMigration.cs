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
                name: "Sampling",
                columns: table => new
                {
                    samplingId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    coLevel = table.Column<int>(nullable: false),
                    humidity = table.Column<int>(nullable: false),
                    lpgLevel = table.Column<int>(nullable: false),
                    movement = table.Column<int>(nullable: false),
                    senderId = table.Column<int>(nullable: false),
                    smokeLevel = table.Column<int>(nullable: false),
                    temperature = table.Column<int>(nullable: false),
                    time = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sampling", x => x.samplingId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sampling");
        }
    }
}
