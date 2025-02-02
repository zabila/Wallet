using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Wallet.Infrastructure.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLocationInTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Wallet",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "10ec5678-8ebd-4644-bdbc-cfd2906a24ca");

            migrationBuilder.DeleteData(
                schema: "Wallet",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "36fb7adf-65d2-45b5-8886-d0ea26e96534");

            migrationBuilder.DeleteData(
                schema: "Wallet",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4c3c22b4-4cf6-499b-98c8-5f5e60c37406");

            migrationBuilder.DropColumn(
                name: "Location",
                schema: "Wallet",
                table: "Transactions");

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                schema: "Wallet",
                table: "Transactions",
                type: "numeric(9,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                schema: "Wallet",
                table: "Transactions",
                type: "numeric(9,6)",
                nullable: false,
                defaultValue: 0m);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "Wallet",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "Wallet",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                schema: "Wallet",
                table: "Transactions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.InsertData(
                schema: "Wallet",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "10ec5678-8ebd-4644-bdbc-cfd2906a24ca", null, "Manager", "MANAGER" },
                    { "36fb7adf-65d2-45b5-8886-d0ea26e96534", null, "Administrator", "ADMINISTRATOR" },
                    { "4c3c22b4-4cf6-499b-98c8-5f5e60c37406", null, "User", "USER" }
                });
        }
    }
}
