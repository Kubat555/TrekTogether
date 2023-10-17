using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTBack.Migrations
{
    public partial class Tada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTrip_Trip_TripId",
                table: "UserTrip");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTrip_User_UserId",
                table: "UserTrip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTrip",
                table: "UserTrip");

            migrationBuilder.RenameTable(
                name: "UserTrip",
                newName: "UserTrips");

            migrationBuilder.RenameIndex(
                name: "IX_UserTrip_TripId",
                table: "UserTrips",
                newName: "IX_UserTrips_TripId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTrips",
                table: "UserTrips",
                columns: new[] { "UserId", "TripId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserTrips_Trip_TripId",
                table: "UserTrips",
                column: "TripId",
                principalTable: "Trip",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTrips_User_UserId",
                table: "UserTrips",
                column: "UserId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTrips_Trip_TripId",
                table: "UserTrips");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTrips_User_UserId",
                table: "UserTrips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTrips",
                table: "UserTrips");

            migrationBuilder.RenameTable(
                name: "UserTrips",
                newName: "UserTrip");

            migrationBuilder.RenameIndex(
                name: "IX_UserTrips_TripId",
                table: "UserTrip",
                newName: "IX_UserTrip_TripId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTrip",
                table: "UserTrip",
                columns: new[] { "UserId", "TripId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserTrip_Trip_TripId",
                table: "UserTrip",
                column: "TripId",
                principalTable: "Trip",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTrip_User_UserId",
                table: "UserTrip",
                column: "UserId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
