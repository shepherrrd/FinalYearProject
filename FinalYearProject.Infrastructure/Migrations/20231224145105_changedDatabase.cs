using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalYearProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "TimeCreated", "TimeUpdated" },
                values: new object[] { "7df0d7d8-f5f7-4b78-bcc0-e6e8c18faadc", "AQAAAAIAAYagAAAAEMzdtY7g+NLh9CNuU+OLwD1B8oWmlH41uj+IAhrEPY0Yczt2X6q6eiymkxj8Idn1MQ==", "B5E07B2C21E026478FD07BD0EABAF37E", new DateTimeOffset(new DateTime(2023, 12, 24, 14, 51, 4, 831, DateTimeKind.Unspecified).AddTicks(7492), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 12, 24, 14, 51, 4, 831, DateTimeKind.Unspecified).AddTicks(7496), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "TimeCreated", "TimeUpdated" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 12, 24, 14, 51, 4, 833, DateTimeKind.Unspecified).AddTicks(9570), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 12, 24, 14, 51, 4, 833, DateTimeKind.Unspecified).AddTicks(9573), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "TimeCreated", "TimeUpdated" },
                values: new object[] { "efdecf43-6053-4b9b-9a10-84f33e754bc8", "AQAAAAIAAYagAAAAEBftYu9tlchbOVWE0kOpMqYLY41ZeZVp6HCnzT2rI4B9Op5wtkattKKexwUpSRyDLw==", "246C002F1E175A4D9573A6F15F84E4F5", new DateTimeOffset(new DateTime(2023, 12, 22, 14, 32, 28, 301, DateTimeKind.Unspecified).AddTicks(4535), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 12, 22, 14, 32, 28, 301, DateTimeKind.Unspecified).AddTicks(4540), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "TimeCreated", "TimeUpdated" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 12, 22, 14, 32, 28, 303, DateTimeKind.Unspecified).AddTicks(2353), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 12, 22, 14, 32, 28, 303, DateTimeKind.Unspecified).AddTicks(2355), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
