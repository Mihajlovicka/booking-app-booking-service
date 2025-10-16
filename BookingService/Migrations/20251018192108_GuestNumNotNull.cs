using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Migrations
{
    /// <inheritdoc />
    public partial class GuestNumNotNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestUsername",
                table: "ReservationRequests");

            migrationBuilder.AddColumn<Guid>(
                name: "external_id",
                table: "Reservations",
                type: "char(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ReservationRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "external_id",
                table: "ReservationRequests",
                type: "char(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRequests_UserId",
                table: "ReservationRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationRequests_Users_UserId",
                table: "ReservationRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationRequests_Users_UserId",
                table: "ReservationRequests");

            migrationBuilder.DropIndex(
                name: "IX_ReservationRequests_UserId",
                table: "ReservationRequests");

            migrationBuilder.DropColumn(
                name: "external_id",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ReservationRequests");

            migrationBuilder.DropColumn(
                name: "external_id",
                table: "ReservationRequests");

            migrationBuilder.AddColumn<string>(
                name: "GuestUsername",
                table: "ReservationRequests",
                type: "longtext",
                nullable: false);
        }
    }
}
