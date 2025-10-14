using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace BookingService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccommodation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "address_id",
                table: "Accommodations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "max_number_of_guests",
                table: "Accommodations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "min_number_of_guests",
                table: "Accommodations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    street_number = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    street_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    city = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    post_number = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    country = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_address_id",
                table: "Accommodations",
                column: "address_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_Addresses_address_id",
                table: "Accommodations",
                column: "address_id",
                principalTable: "Addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_Addresses_address_id",
                table: "Accommodations");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Accommodations_address_id",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "address_id",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "max_number_of_guests",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "min_number_of_guests",
                table: "Accommodations");
        }
    }
}
