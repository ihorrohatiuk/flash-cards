using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashCards.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUnitInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlashCardsUnits_Users_UserId",
                table: "FlashCardsUnits");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "FlashCardsUnits");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "FlashCardsUnits",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "Theme",
                table: "FlashCardsUnits",
                newName: "Subject");

            migrationBuilder.RenameIndex(
                name: "IX_FlashCardsUnits_UserId",
                table: "FlashCardsUnits",
                newName: "IX_FlashCardsUnits_OwnerId");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "FlashCardsUnits",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FlashCardsUnits",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "Progress",
                table: "FlashCardsUnits",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddForeignKey(
                name: "FK_FlashCardsUnits_Users_OwnerId",
                table: "FlashCardsUnits",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlashCardsUnits_Users_OwnerId",
                table: "FlashCardsUnits");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "FlashCardsUnits");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "FlashCardsUnits");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "FlashCardsUnits");

            migrationBuilder.RenameColumn(
                name: "Subject",
                table: "FlashCardsUnits",
                newName: "Theme");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "FlashCardsUnits",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FlashCardsUnits_OwnerId",
                table: "FlashCardsUnits",
                newName: "IX_FlashCardsUnits_UserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "FlashCardsUnits",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_FlashCardsUnits_Users_UserId",
                table: "FlashCardsUnits",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
