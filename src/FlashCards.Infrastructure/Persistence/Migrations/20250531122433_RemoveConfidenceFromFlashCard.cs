using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashCards.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveConfidenceFromFlashCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confidence",
                table: "FlashCards");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Confidence",
                table: "FlashCards",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
