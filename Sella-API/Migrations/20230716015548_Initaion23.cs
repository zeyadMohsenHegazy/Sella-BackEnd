using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sella_API.Migrations
{
    /// <inheritdoc />
    public partial class Initaion23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Sella Users",
                newName: "Phone");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Sella Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Sella Users");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Sella Users",
                newName: "UserName");
        }
    }
}
