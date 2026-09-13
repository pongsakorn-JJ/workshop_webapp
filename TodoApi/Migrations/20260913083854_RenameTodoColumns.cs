using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApi.Migrations
{
    /// <inheritdoc />
    public partial class RenameTodoColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Todo",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MyProperty",
                table: "Todo",
                newName: "IsCompleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Todo",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                table: "Todo",
                newName: "MyProperty");
        }
    }
}
