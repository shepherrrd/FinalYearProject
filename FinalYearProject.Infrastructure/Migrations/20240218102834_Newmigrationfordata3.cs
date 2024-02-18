using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FinalYearProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Newmigrationfordata3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicalDataRecords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HospitalId = table.Column<long>(type: "bigint", nullable: false),
                    ICDRecordBytes = table.Column<byte[]>(type: "bytea", nullable: false),
                    RecordType = table.Column<int>(type: "integer", nullable: false),
                    SDTMRecordBytes = table.Column<byte[]>(type: "bytea", nullable: false),
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
                values: new object[] { "3d82c650-0eb1-455b-aaf3-c5c0bfde2e38", "AQAAAAIAAYagAAAAEKxdJwuTb6IJciLIu++CUQggK5gXWhZE0Ml9ddzkwnHFT5pssMoeQdnVh+lz2zN7hw==", "24A4B506E607B247BD5A20275ADB14CD", new DateTimeOffset(new DateTime(2024, 2, 18, 10, 24, 27, 772, DateTimeKind.Unspecified).AddTicks(4772), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 2, 18, 10, 24, 27, 772, DateTimeKind.Unspecified).AddTicks(4779), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "TimeCreated", "TimeUpdated" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 2, 18, 10, 24, 27, 775, DateTimeKind.Unspecified).AddTicks(1480), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 2, 18, 10, 24, 27, 775, DateTimeKind.Unspecified).AddTicks(1483), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
