using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreTask.Migrations
{
    public partial class AddColumnDateBirth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateBirth",
                table: "Customers",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateBirth",
                table: "Customers");
        }
    }
}
