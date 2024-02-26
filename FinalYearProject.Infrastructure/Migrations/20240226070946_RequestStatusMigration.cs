using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalYearProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RequestStatusMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "HospitalRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "TimeCreated", "TimeUpdated" },
                values: new object[] { "2f75ac3a-073e-43b9-989e-eb3913164571", "AQAAAAIAAYagAAAAEN1RsO1HUHvjSPtWpABEOeFuv+eJtMIf0ZBDrNosGI9OSY8nEHPnhGIJh5Q/bvSY6A==", "7EBDB23E1407CE499095F288387A7255", new DateTimeOffset(new DateTime(2024, 2, 26, 7, 9, 45, 706, DateTimeKind.Unspecified).AddTicks(8542), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 2, 26, 7, 9, 45, 706, DateTimeKind.Unspecified).AddTicks(8547), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "TimeCreated", "TimeUpdated" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 2, 26, 7, 9, 45, 708, DateTimeKind.Unspecified).AddTicks(3672), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 2, 26, 7, 9, 45, 708, DateTimeKind.Unspecified).AddTicks(3675), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "HospitalRequests");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "TimeCreated", "TimeUpdated" },
                values: new object[] { "5a7cbb53-e2a1-4086-94ff-3515cf037b44", "AQAAAAIAAYagAAAAEMOQUXxX+sYl9ilxXrSIOipnsPSTqxq8GTdVmjxdPud9flR91EPgqkqwy+CedwAgzw==", "5C53E81D1E453C4FB1FC824F245FD00D", new DateTimeOffset(new DateTime(2024, 2, 18, 10, 29, 32, 571, DateTimeKind.Unspecified).AddTicks(483), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 2, 18, 10, 29, 32, 571, DateTimeKind.Unspecified).AddTicks(490), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "TimeCreated", "TimeUpdated" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 2, 18, 10, 29, 32, 573, DateTimeKind.Unspecified).AddTicks(6104), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 2, 18, 10, 29, 32, 573, DateTimeKind.Unspecified).AddTicks(6107), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
