using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalYearProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DbChangesMigration8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicalDataRecord",
                table: "MedicalDataRecord");

            migrationBuilder.RenameTable(
                name: "MedicalDataRecord",
                newName: "MedicalDataRecords");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicalDataRecords",
                table: "MedicalDataRecords",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "TimeCreated", "TimeUpdated" },
                values: new object[] { "9ac3cad5-0409-42c5-a8b5-1cbd4dac0130", "AQAAAAIAAYagAAAAEPsLgxDZoi85n2jE5oEc1GwtM/uYzQbIEy+/KVYV8g1BnrPh99qBjQ9LaDhhoc3IBw==", "7679494FA435444E91D59BB3F5FD1B1A", new DateTimeOffset(new DateTime(2024, 2, 13, 11, 48, 57, 683, DateTimeKind.Unspecified).AddTicks(9776), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 2, 13, 11, 48, 57, 683, DateTimeKind.Unspecified).AddTicks(9781), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "TimeCreated", "TimeUpdated" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 2, 13, 11, 48, 57, 685, DateTimeKind.Unspecified).AddTicks(4589), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 2, 13, 11, 48, 57, 685, DateTimeKind.Unspecified).AddTicks(4590), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicalDataRecords",
                table: "MedicalDataRecords");

            migrationBuilder.RenameTable(
                name: "MedicalDataRecords",
                newName: "MedicalDataRecord");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicalDataRecord",
                table: "MedicalDataRecord",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "TimeCreated", "TimeUpdated" },
                values: new object[] { "91878b96-319d-4eb8-bc0c-529b20db9b6e", "AQAAAAIAAYagAAAAEKXDGriiaSulX7YBKrsbn0VTe6KG/TobyupfX5UoQ9AwJD9AsoOmmab9bGnbLigoZA==", "58D10BD6C09FC74E9AFD268835A526D5", new DateTimeOffset(new DateTime(2024, 2, 13, 11, 44, 5, 561, DateTimeKind.Unspecified).AddTicks(6282), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 2, 13, 11, 44, 5, 561, DateTimeKind.Unspecified).AddTicks(6288), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "TimeCreated", "TimeUpdated" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 2, 13, 11, 44, 5, 563, DateTimeKind.Unspecified).AddTicks(5057), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 2, 13, 11, 44, 5, 563, DateTimeKind.Unspecified).AddTicks(5060), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
