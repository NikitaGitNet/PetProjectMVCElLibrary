using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserEmailToEmailIntoBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserEmail",
                table: "Bookings",
                newName: "Email");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3b62472e-4f66-49fa-a20f-e7685b9565d8",
                columns: new[] { "ConcurrencyStamp", "CreateOn", "PasswordHash" },
                values: new object[] { "38bd9dec-d40e-4b4c-85c3-05477bc1a607", new DateTime(2024, 7, 12, 14, 52, 55, 848, DateTimeKind.Local).AddTicks(5342), "AQAAAAIAAYagAAAAEGxd3s8XvK6rGlqCcVyedPGDdIaW3iUWwzWVG7iy3v/MyL1KR72QALGInwQrFcpeJA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "86d55f40-9544-4d92-aa24-cc5693a5fd96",
                columns: new[] { "ConcurrencyStamp", "CreateOn", "PasswordHash" },
                values: new object[] { "ad7a9fd7-783a-4cb6-9db9-67a798b101f4", new DateTime(2024, 7, 12, 14, 52, 55, 905, DateTimeKind.Local).AddTicks(8760), "AQAAAAIAAYagAAAAEISufKxRke5l0dL1KxzGIlvenVW9Pp8onBmSQ6xJyudcidM2y2d0QgZt4hz1vJOEyQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Bookings",
                newName: "UserEmail");

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
    }
}
