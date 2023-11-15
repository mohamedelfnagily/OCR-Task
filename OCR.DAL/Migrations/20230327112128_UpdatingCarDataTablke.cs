using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OCR.DAL.Migrations
{
    public partial class UpdatingCarDataTablke : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriverFullName",
                table: "CarsData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LicensceEndDate",
                table: "CarsData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LicensceStartDate",
                table: "CarsData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverFullName",
                table: "CarsData");

            migrationBuilder.DropColumn(
                name: "LicensceEndDate",
                table: "CarsData");

            migrationBuilder.DropColumn(
                name: "LicensceStartDate",
                table: "CarsData");
        }
    }
}
