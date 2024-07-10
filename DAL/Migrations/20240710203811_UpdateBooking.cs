using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiptCode",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3b62472e-4f66-49fa-a20f-e7685b9565d8",
                columns: new[] { "ConcurrencyStamp", "CreateOn", "PasswordHash" },
                values: new object[] { "a3320aeb-1c38-4aea-96e6-ebfe68c6e1ea", new DateTime(2024, 7, 10, 23, 38, 10, 955, DateTimeKind.Local).AddTicks(3034), "AQAAAAIAAYagAAAAEOU9G9e9WySe5/C6CsxYzIJq1RfwYQoBxrkMSkHlqPmErxSpV34d4ju4h65NNN86wQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "86d55f40-9544-4d92-aa24-cc5693a5fd96",
                columns: new[] { "ConcurrencyStamp", "CreateOn", "PasswordHash" },
                values: new object[] { "6fdc3483-7f9c-4188-b8bc-9ddaffe813c5", new DateTime(2024, 7, 10, 23, 38, 10, 991, DateTimeKind.Local).AddTicks(4813), "AQAAAAIAAYagAAAAEGchl4SpDd2bGOlTv13DdLRnFfBBnK0zhLU9VMPy/TAx0P2TEKMQe1e8e2XRjpC6Mg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiptCode",
                table: "Bookings");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3b62472e-4f66-49fa-a20f-e7685b9565d8",
                columns: new[] { "ConcurrencyStamp", "CreateOn", "PasswordHash" },
                values: new object[] { "91bb819f-38b9-458d-8054-c6a75971ab9d", new DateTime(2024, 7, 1, 21, 21, 49, 642, DateTimeKind.Local).AddTicks(7307), "AQAAAAIAAYagAAAAENgXmOF4yyfLc4NKPwnMKVLal/RNk7Qtne8tS05ZubBWSTtUfHkIKVAGAW4yFxFxYQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "86d55f40-9544-4d92-aa24-cc5693a5fd96",
                columns: new[] { "ConcurrencyStamp", "CreateOn", "PasswordHash" },
                values: new object[] { "fc23069d-2c42-4d40-9b86-84897565a788", new DateTime(2024, 7, 1, 21, 21, 49, 678, DateTimeKind.Local).AddTicks(8309), "AQAAAAIAAYagAAAAEFZilgPMVUYmGXKx9VyCeyyoI6EpQcTQaYh3lu73OFYPROqRPKPaK26DnPcKxHtPng==" });
        }
    }
}
