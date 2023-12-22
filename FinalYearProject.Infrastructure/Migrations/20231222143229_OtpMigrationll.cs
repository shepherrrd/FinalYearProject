using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalYearProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OtpMigrationll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AccountStatus",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "Email", "FirstName", "NormalizedUserName", "PasswordHash", "SecurityStamp", "TimeCreated", "TimeUpdated", "UserName" },
                values: new object[] { "efdecf43-6053-4b9b-9a10-84f33e754bc8", "technology@dataaggregator.com", "FInalYear", "TECHNOLOGY@DATAAGGREGATOR.COM", "AQAAAAIAAYagAAAAEBftYu9tlchbOVWE0kOpMqYLY41ZeZVp6HCnzT2rI4B9Op5wtkattKKexwUpSRyDLw==", "246C002F1E175A4D9573A6F15F84E4F5", new DateTimeOffset(new DateTime(2023, 12, 22, 14, 32, 28, 301, DateTimeKind.Unspecified).AddTicks(4535), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 12, 22, 14, 32, 28, 301, DateTimeKind.Unspecified).AddTicks(4540), new TimeSpan(0, 0, 0, 0, 0)), "TECHNOLOGY@DATAAGGREGATOR.COM" });

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "TimeCreated", "TimeUpdated" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 12, 22, 14, 32, 28, 303, DateTimeKind.Unspecified).AddTicks(2353), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 12, 22, 14, 32, 28, 303, DateTimeKind.Unspecified).AddTicks(2355), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AccountStatus",
                table: "AspNetUsers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "Email", "FirstName", "NormalizedUserName", "PasswordHash", "SecurityStamp", "TimeCreated", "TimeUpdated", "UserName" },
                values: new object[] { "0e950b28-f984-4889-9d39-0260ea607904", "technology@payultra.com", "PayUltra", "Shepherd", "AQAAAAIAAYagAAAAEAwVNZ6+HUSKPfi9oPhSkvxLSICbbVXLUWan8sTbPMTfzLVRhffi5AnBj7PmMVhsVw==", "1DEEEA0760FC82449DC7061B4A325F44", new DateTimeOffset(new DateTime(2023, 12, 21, 9, 57, 39, 706, DateTimeKind.Unspecified).AddTicks(4236), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 12, 21, 9, 57, 39, 706, DateTimeKind.Unspecified).AddTicks(4240), new TimeSpan(0, 0, 0, 0, 0)), "technology@payultra.com" });

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "TimeCreated", "TimeUpdated" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 12, 21, 9, 57, 39, 708, DateTimeKind.Unspecified).AddTicks(1925), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 12, 21, 9, 57, 39, 708, DateTimeKind.Unspecified).AddTicks(1928), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
