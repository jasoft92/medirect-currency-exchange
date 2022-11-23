using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace medirectcurrencyexchange.Database.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Email", "IdCard", "Name", "Surname" },
                values: new object[,]
                {
                    { 1L, "jos.att@gmail.com", "13392G", "Joseph", "Attard" },
                    { 2L, "rog.fed@gmail.com", "11223S", "Roger", "Federer" },
                    { 3L, "raf.nad@gmail.com", "12121S", "Rafael", "Nadal" },
                    { 4L, "and.mur@gmail.com", "12312E", "Andy", "Murray" },
                    { 5L, "nov.djo@gmail.com", "54321S", "Novak", "Djokovic" }
                });

            migrationBuilder.InsertData(
                table: "CustomerWallets",
                columns: new[] { "CurrencyCode", "CustomerId", "Amount", "LastModified" },
                values: new object[,]
                {
                    { "EUR", 1L, 1000m, new DateTime(2022, 11, 23, 23, 12, 2, 890, DateTimeKind.Local).AddTicks(4223) },
                    { "GBP", 1L, 500m, new DateTime(2022, 11, 23, 23, 12, 2, 890, DateTimeKind.Local).AddTicks(4256) },
                    { "CHF", 2L, 10000m, new DateTime(2022, 11, 23, 23, 12, 2, 890, DateTimeKind.Local).AddTicks(4260) },
                    { "EUR", 2L, 5000m, new DateTime(2022, 11, 23, 23, 12, 2, 890, DateTimeKind.Local).AddTicks(4258) },
                    { "EUR", 3L, 6000m, new DateTime(2022, 11, 23, 23, 12, 2, 890, DateTimeKind.Local).AddTicks(4262) },
                    { "USD", 3L, 8000m, new DateTime(2022, 11, 23, 23, 12, 2, 890, DateTimeKind.Local).AddTicks(4264) },
                    { "EUR", 4L, 6000m, new DateTime(2022, 11, 23, 23, 12, 2, 890, DateTimeKind.Local).AddTicks(4265) },
                    { "GBP", 4L, 8000m, new DateTime(2022, 11, 23, 23, 12, 2, 890, DateTimeKind.Local).AddTicks(4267) },
                    { "AUD", 5L, 11000m, new DateTime(2022, 11, 23, 23, 12, 2, 890, DateTimeKind.Local).AddTicks(4270) },
                    { "EUR", 5L, 6000m, new DateTime(2022, 11, 23, 23, 12, 2, 890, DateTimeKind.Local).AddTicks(4268) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CustomerWallets",
                keyColumns: new[] { "CurrencyCode", "CustomerId" },
                keyValues: new object[] { "EUR", 1L });

            migrationBuilder.DeleteData(
                table: "CustomerWallets",
                keyColumns: new[] { "CurrencyCode", "CustomerId" },
                keyValues: new object[] { "GBP", 1L });

            migrationBuilder.DeleteData(
                table: "CustomerWallets",
                keyColumns: new[] { "CurrencyCode", "CustomerId" },
                keyValues: new object[] { "CHF", 2L });

            migrationBuilder.DeleteData(
                table: "CustomerWallets",
                keyColumns: new[] { "CurrencyCode", "CustomerId" },
                keyValues: new object[] { "EUR", 2L });

            migrationBuilder.DeleteData(
                table: "CustomerWallets",
                keyColumns: new[] { "CurrencyCode", "CustomerId" },
                keyValues: new object[] { "EUR", 3L });

            migrationBuilder.DeleteData(
                table: "CustomerWallets",
                keyColumns: new[] { "CurrencyCode", "CustomerId" },
                keyValues: new object[] { "USD", 3L });

            migrationBuilder.DeleteData(
                table: "CustomerWallets",
                keyColumns: new[] { "CurrencyCode", "CustomerId" },
                keyValues: new object[] { "EUR", 4L });

            migrationBuilder.DeleteData(
                table: "CustomerWallets",
                keyColumns: new[] { "CurrencyCode", "CustomerId" },
                keyValues: new object[] { "GBP", 4L });

            migrationBuilder.DeleteData(
                table: "CustomerWallets",
                keyColumns: new[] { "CurrencyCode", "CustomerId" },
                keyValues: new object[] { "AUD", 5L });

            migrationBuilder.DeleteData(
                table: "CustomerWallets",
                keyColumns: new[] { "CurrencyCode", "CustomerId" },
                keyValues: new object[] { "EUR", 5L });

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 5L);
        }
    }
}
