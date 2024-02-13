using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalYearProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DbChangesMigration5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "TimeCreated", "TimeUpdated" },
                values: new object[] { "949eec6b-6f77-459b-9cf0-6f3b164c07ba", "AQAAAAIAAYagAAAAEBhZJcJ3EHMK5DyRcdTjpZXxDodNZFu5fThBpMOyLq8FUd2X7bYdp6tO7OkTPX/0Sg==", "160265AFABAF6F48908F5A28EA808CD3", new DateTimeOffset(new DateTime(2024, 2, 13, 11, 39, 48, 943, DateTimeKind.Unspecified).AddTicks(9020), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 2, 13, 11, 39, 48, 943, DateTimeKind.Unspecified).AddTicks(9026), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "TimeCreated", "TimeUpdated" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 2, 13, 11, 39, 48, 945, DateTimeKind.Unspecified).AddTicks(7139), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 2, 13, 11, 39, 48, 945, DateTimeKind.Unspecified).AddTicks(7142), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "TimeCreated", "TimeUpdated" },
                values: new object[] { "508ce0ed-056e-426e-82a8-be8edcfc7c46", "AQAAAAIAAYagAAAAEF3Q6HyZUf0JVv8rWpZNxKAq99oYSd/7oWvmmOEKi6uA1fmqxvP3oHP83L2M8iTQBg==", "89AB5C7C0C73C14F81668919AD2491E0", new DateTimeOffset(new DateTime(2024, 2, 13, 9, 45, 37, 220, DateTimeKind.Unspecified).AddTicks(543), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 2, 13, 9, 45, 37, 220, DateTimeKind.Unspecified).AddTicks(549), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "TimeCreated", "TimeUpdated" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 2, 13, 9, 45, 37, 221, DateTimeKind.Unspecified).AddTicks(5603), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 2, 13, 9, 45, 37, 221, DateTimeKind.Unspecified).AddTicks(5605), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
