using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sella_API.Migrations
{
    /// <inheritdoc />
    public partial class V99 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Customers_CustomerID",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerID",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                table: "Orders",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CustomerID",
                table: "Orders",
                newName: "IX_Orders_UserID");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                table: "Carts",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_CustomerID",
                table: "Carts",
                newName: "IX_Carts_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Sella Users_UserID",
                table: "Carts",
                column: "UserID",
                principalTable: "Sella Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Sella Users_UserID",
                table: "Orders",
                column: "UserID",
                principalTable: "Sella Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Sella Users_UserID",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Sella Users_UserID",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Orders",
                newName: "CustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserID",
                table: "Orders",
                newName: "IX_Orders_CustomerID");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Carts",
                newName: "CustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_UserID",
                table: "Carts",
                newName: "IX_Carts_CustomerID");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Governate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Customers_CustomerID",
                table: "Carts",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerID",
                table: "Orders",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
