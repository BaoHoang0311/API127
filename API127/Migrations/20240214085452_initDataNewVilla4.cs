using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API127.Migrations
{
    /// <inheritdoc />
    public partial class initDataNewVilla4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 2, 14, 15, 54, 51, 978, DateTimeKind.Local).AddTicks(6687));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 2, 14, 15, 54, 51, 978, DateTimeKind.Local).AddTicks(6700));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 2, 14, 15, 54, 51, 978, DateTimeKind.Local).AddTicks(6702));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 2, 14, 15, 54, 51, 978, DateTimeKind.Local).AddTicks(6704));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 2, 14, 15, 54, 51, 978, DateTimeKind.Local).AddTicks(6705));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "Name" },
                values: new object[] { new DateTime(2024, 2, 14, 15, 54, 51, 978, DateTimeKind.Local).AddTicks(6707), "TEST 1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 2, 14, 15, 54, 27, 684, DateTimeKind.Local).AddTicks(8146));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 2, 14, 15, 54, 27, 684, DateTimeKind.Local).AddTicks(8160));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 2, 14, 15, 54, 27, 684, DateTimeKind.Local).AddTicks(8162));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 2, 14, 15, 54, 27, 684, DateTimeKind.Local).AddTicks(8164));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 2, 14, 15, 54, 27, 684, DateTimeKind.Local).AddTicks(8166));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "Name" },
                values: new object[] { new DateTime(2024, 2, 14, 15, 54, 27, 684, DateTimeKind.Local).AddTicks(8167), "TEST" });
        }
    }
}
