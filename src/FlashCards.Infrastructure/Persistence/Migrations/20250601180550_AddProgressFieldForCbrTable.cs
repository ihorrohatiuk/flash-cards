using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashCards.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProgressFieldForCbrTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CbrProgresses_FlashCards_FlashCardId",
                table: "CbrProgresses");

            migrationBuilder.RenameColumn(
                name: "FlashCardId",
                table: "CbrProgresses",
                newName: "FlashCardsUnitId");

            migrationBuilder.RenameColumn(
                name: "Confidence",
                table: "CbrProgresses",
                newName: "Progress");

            migrationBuilder.RenameIndex(
                name: "IX_CbrProgresses_FlashCardId",
                table: "CbrProgresses",
                newName: "IX_CbrProgresses_FlashCardsUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_CbrProgresses_FlashCardsUnits_FlashCardsUnitId",
                table: "CbrProgresses",
                column: "FlashCardsUnitId",
                principalTable: "FlashCardsUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CbrProgresses_FlashCardsUnits_FlashCardsUnitId",
                table: "CbrProgresses");

            migrationBuilder.RenameColumn(
                name: "Progress",
                table: "CbrProgresses",
                newName: "Confidence");

            migrationBuilder.RenameColumn(
                name: "FlashCardsUnitId",
                table: "CbrProgresses",
                newName: "FlashCardId");

            migrationBuilder.RenameIndex(
                name: "IX_CbrProgresses_FlashCardsUnitId",
                table: "CbrProgresses",
                newName: "IX_CbrProgresses_FlashCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_CbrProgresses_FlashCards_FlashCardId",
                table: "CbrProgresses",
                column: "FlashCardId",
                principalTable: "FlashCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
