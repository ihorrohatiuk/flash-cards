using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashCards.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsPrivateForUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "FlashCardsUnits");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "FlashCardsUnits",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
