using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTBack.Migrations
{
    public partial class addCarToTrip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Trip",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trip_CarId",
                table: "Trip",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_Car",
                table: "Trip",
                column: "CarId",
                principalTable: "Car",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Car",
                table: "Trip");

            migrationBuilder.DropIndex(
                name: "IX_Trip_CarId",
                table: "Trip");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Trip");
        }
    }
}
