using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FinalYearProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Newmigrationfordata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicalDataRecords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HospitalId = table.Column<long>(type: "bigint", nullable: false),
                    RecordType = table.Column<int>(type: "integer", nullable: false),
                    SDTMRecordBytes = table.Column<byte[]>(type: "bytea", nullable: false),
                    ICDRecordBytes = table.Column<byte[]>(type: "bytea", nullable: false),
                    TimeCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TimeUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalDataRecords", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalDataRecords");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "TimeCreated", "TimeUpdated" },
                values: new object[] { "3e8681c7-f329-4059-af7b-a90171160824", "AQAAAAIAAYagAAAAEEVY76dqhHetcv0pL/WtTnmzz++x16oMEKuF2J2ghW/LmcNORPUj4qJhhbuEp47/qg==", "C8B718300C2EAB4B9B54AFAE12518471", new DateTimeOffset(new DateTime(2024, 2, 18, 10, 28, 33, 383, DateTimeKind.Unspecified).AddTicks(1416), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 2, 18, 10, 28, 33, 383, DateTimeKind.Unspecified).AddTicks(1421), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "TimeCreated", "TimeUpdated" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 2, 18, 10, 28, 33, 384, DateTimeKind.Unspecified).AddTicks(6744), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 2, 18, 10, 28, 33, 384, DateTimeKind.Unspecified).AddTicks(6746), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
