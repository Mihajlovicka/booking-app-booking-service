using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace BookingService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccommodation1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Accommodations",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Picture",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    url = table.Column<string>(type: "longtext", nullable: false),
                    accommodation_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picture", x => x.id);
                    table.ForeignKey(
                        name: "FK_Picture_Accommodations_accommodation_id",
                        column: x => x.accommodation_id,
                        principalTable: "Accommodations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Picture_accommodation_id",
                table: "Picture",
                column: "accommodation_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Picture");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Accommodations");
        }
    }
}
