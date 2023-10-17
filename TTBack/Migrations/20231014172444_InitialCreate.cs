using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTBack.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    phoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    isDriver = table.Column<bool>(type: "bit", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    carMake = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    carModel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    carYear = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car", x => x.id);
                    table.ForeignKey(
                        name: "FK_Car_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Trip",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    departureCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    arrivalCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    departureData = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    price = table.Column<int>(type: "int", nullable: true),
                    availableSeats = table.Column<int>(type: "int", nullable: true),
                    driverId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trip", x => x.id);
                    table.ForeignKey(
                        name: "FK_Trip_User",
                        column: x => x.driverId,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserTrip",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TripId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTrip", x => new { x.UserId, x.TripId });
                    table.ForeignKey(
                        name: "FK_UserTrip_Trip_TripId",
                        column: x => x.TripId,
                        principalTable: "Trip",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTrip_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Car_UserId",
                table: "Car",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Trip_driverId",
                table: "Trip",
                column: "driverId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTrip_TripId",
                table: "UserTrip",
                column: "TripId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropTable(
                name: "UserTrip");

            migrationBuilder.DropTable(
                name: "Trip");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
