using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTracker.DL.Migrations
{
    public partial class NewModelUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_TodoItems_TodoItemId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_TodoItemId",
                table: "Activities");

            migrationBuilder.AddColumn<string>(
                name: "AssignedToEmail",
                table: "TodoItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                table: "TodoItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedToEmail",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "IsProcessed",
                table: "TodoItems");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_TodoItemId",
                table: "Activities",
                column: "TodoItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_TodoItems_TodoItemId",
                table: "Activities",
                column: "TodoItemId",
                principalTable: "TodoItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
