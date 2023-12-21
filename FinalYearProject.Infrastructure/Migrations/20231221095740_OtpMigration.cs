using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FinalYearProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OtpMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "OtpVerifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Recipient = table.Column<string>(type: "text", nullable: false),
                    RecipientType = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Purpose = table.Column<int>(type: "integer", nullable: false),
                    ConfirmedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TimeCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TimeUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpVerifications", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "TimeCreated", "TimeUpdated" },
                values: new object[] { "0e950b28-f984-4889-9d39-0260ea607904", "AQAAAAIAAYagAAAAEAwVNZ6+HUSKPfi9oPhSkvxLSICbbVXLUWan8sTbPMTfzLVRhffi5AnBj7PmMVhsVw==", "1DEEEA0760FC82449DC7061B4A325F44", new DateTimeOffset(new DateTime(2023, 12, 21, 9, 57, 39, 706, DateTimeKind.Unspecified).AddTicks(4236), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 12, 21, 9, 57, 39, 706, DateTimeKind.Unspecified).AddTicks(4240), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "TimeCreated", "TimeUpdated" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 12, 21, 9, 57, 39, 708, DateTimeKind.Unspecified).AddTicks(1925), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 12, 21, 9, 57, 39, 708, DateTimeKind.Unspecified).AddTicks(1928), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OtpVerifications");

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "EmailAddress", "PasswordHash", "SecurityStamp", "TimeCreated", "TimeUpdated" },
                values: new object[] { "6bc607d8-3643-4826-bf18-ab5724cfc550", null, "AQAAAAIAAYagAAAAEHX4mhapxOsGtFvQq/gnLNxPzk0GDHtlz0SQ0SVVrb5gWGoQ8g4S/xEJxwesBynsHA==", "A90D0351EF78674BBBA68CC5167775F1", new DateTimeOffset(new DateTime(2023, 12, 20, 13, 15, 31, 823, DateTimeKind.Unspecified).AddTicks(5285), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 12, 20, 13, 15, 31, 823, DateTimeKind.Unspecified).AddTicks(5290), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "TimeCreated", "TimeUpdated" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 12, 20, 13, 15, 31, 825, DateTimeKind.Unspecified).AddTicks(5559), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 12, 20, 13, 15, 31, 825, DateTimeKind.Unspecified).AddTicks(5561), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
