using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Wallet.Infrastructure.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddedLocalization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Wallet",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "1b022eb4-11f4-479e-9b89-03917e52688a");

            migrationBuilder.DeleteData(
                schema: "Wallet",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "9ea0e160-795e-4f2b-a04d-650f811c2e2a");

            migrationBuilder.DeleteData(
                schema: "Wallet",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "af876616-ae70-4148-81d7-15b550b8d1c4");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                schema: "Wallet",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                schema: "Wallet",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Localization",
                schema: "Wallet",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                schema: "Wallet",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "dbba033a-0ec0-4ac7-9451-adf0962baddd", null, "User", "USER" },
                    { "e8e60bb8-1771-42fd-8c4b-a974a6d8cb38", null, "Manager", "MANAGER" },
                    { "f1b7d66f-ca8a-406e-b68b-b2edc747017e", null, "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Wallet",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "dbba033a-0ec0-4ac7-9451-adf0962baddd");

            migrationBuilder.DeleteData(
                schema: "Wallet",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "e8e60bb8-1771-42fd-8c4b-a974a6d8cb38");

            migrationBuilder.DeleteData(
                schema: "Wallet",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "f1b7d66f-ca8a-406e-b68b-b2edc747017e");

            migrationBuilder.DropColumn(
                name: "Localization",
                schema: "Wallet",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                schema: "Wallet",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                schema: "Wallet",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.InsertData(
                schema: "Wallet",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1b022eb4-11f4-479e-9b89-03917e52688a", null, "User", "USER" },
                    { "9ea0e160-795e-4f2b-a04d-650f811c2e2a", null, "Administrator", "ADMINISTRATOR" },
                    { "af876616-ae70-4148-81d7-15b550b8d1c4", null, "Manager", "MANAGER" }
                });
        }
    }
}
