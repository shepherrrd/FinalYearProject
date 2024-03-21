using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FinalYearProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class rsamigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRSA",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicRSAParameters = table.Column<string>(type: "text", nullable: false),
                    PrivateRSAParameters = table.Column<string>(type: "text", nullable: false),
                    UserID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRSA", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRSA");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "TimeCreated", "TimeUpdated" },
                values: new object[] { "0262933e-bd4d-4e1c-82e0-2b33695a5b77", "AQAAAAIAAYagAAAAEPlb0TkiR06a7zEeCTraXwLC8aHWbGBLiyAYRlQ3JOJN8wDvv7nLzD5/Jnaz95eOpA==", "DF871A8A557B4049927912CA921BF579", new DateTimeOffset(new DateTime(2024, 3, 6, 22, 51, 26, 32, DateTimeKind.Unspecified).AddTicks(4457), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 3, 6, 22, 51, 26, 32, DateTimeKind.Unspecified).AddTicks(4463), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "TimeCreated", "TimeUpdated" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 3, 6, 22, 51, 26, 34, DateTimeKind.Unspecified).AddTicks(886), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 3, 6, 22, 51, 26, 34, DateTimeKind.Unspecified).AddTicks(889), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
