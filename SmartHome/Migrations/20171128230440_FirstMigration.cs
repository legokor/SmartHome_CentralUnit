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
                name: "DataSample",
                columns: table => new
                {
                    SamplingId = table.Column<Guid>(nullable: false),
                    CoLevel = table.Column<double>(nullable: false),
                    Humidity = table.Column<double>(nullable: false),
                    LpgLevel = table.Column<double>(nullable: false),
                    Movement = table.Column<int>(nullable: false),
                    SenderId = table.Column<int>(nullable: false),
                    SmokeLevel = table.Column<double>(nullable: false),
                    Temperature = table.Column<double>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSample", x => x.SamplingId);
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
                name: "DataSample");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
