using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashCards.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProgressFieldFromUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Progress",
                table: "FlashCardsUnits");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Progress",
                table: "FlashCardsUnits",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
