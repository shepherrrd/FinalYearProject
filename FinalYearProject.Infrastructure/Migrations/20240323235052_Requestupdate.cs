using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalYearProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Requestupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "IrbApproval",
                table: "HospitalRequests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Reason",
                table: "HospitalRequests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "TimeCreated", "TimeUpdated" },
                values: new object[] { "31f19f22-360d-45db-b9fa-d41a4935fdb9", "AQAAAAIAAYagAAAAEJA22WyOLU5mgjK5CO9A+6l8rCIopWn9BAGUBXX0a5ZehZkqdTyayNCb8TzXfC0Olw==", "ABBB1230DB2971439D60CF0965B13A42", new DateTimeOffset(new DateTime(2024, 3, 23, 23, 50, 50, 933, DateTimeKind.Unspecified).AddTicks(2820), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 3, 23, 23, 50, 50, 933, DateTimeKind.Unspecified).AddTicks(2826), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "TimeCreated", "TimeUpdated" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 3, 23, 23, 50, 50, 934, DateTimeKind.Unspecified).AddTicks(8476), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 3, 23, 23, 50, 50, 934, DateTimeKind.Unspecified).AddTicks(8478), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IrbApproval",
                table: "HospitalRequests");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "HospitalRequests");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "TimeCreated", "TimeUpdated" },
                values: new object[] { "279c6978-7127-4dd2-9b5f-c90ddcae58a0", "AQAAAAIAAYagAAAAEDHXgKnz7I1vBXFuIb+RIY2Cyjy2/9ixRPXZRu6HPBKCUfMuGn0k6ICGbCT5LpvoiA==", "4A6D773986063A418E998BE3627DF2EF", new DateTimeOffset(new DateTime(2024, 3, 21, 17, 5, 37, 901, DateTimeKind.Unspecified).AddTicks(8524), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 3, 21, 17, 5, 37, 901, DateTimeKind.Unspecified).AddTicks(8531), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "TimeCreated", "TimeUpdated" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 3, 21, 17, 5, 37, 903, DateTimeKind.Unspecified).AddTicks(7074), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 3, 21, 17, 5, 37, 903, DateTimeKind.Unspecified).AddTicks(7076), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
