using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OCR.DAL.Migrations
{
    public partial class EditingCarDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarLicenseImage",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<byte[]>(
                name: "CarLicenseImage",
                table: "CarsData",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarLicenseImage",
                table: "CarsData");

            migrationBuilder.AddColumn<byte[]>(
                name: "CarLicenseImage",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
