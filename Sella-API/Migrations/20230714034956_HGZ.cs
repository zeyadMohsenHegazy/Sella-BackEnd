using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sella_API.Migrations
{
    /// <inheritdoc />
    public partial class HGZ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Sella Users");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Sella Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ConfirmPassword",
                table: "Sella Users",
                newName: "Token");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Sella Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Sella Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Sella Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Sella Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sella Users",
                table: "Sella Users",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Sella Users",
                table: "Sella Users");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Sella Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Sella Users");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Sella Users");

            migrationBuilder.RenameTable(
                name: "Sella Users",
                newName: "Users");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "Users",
                newName: "ConfirmPassword");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserID");
        }
    }
}
