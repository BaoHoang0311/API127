using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API127.Migrations
{
    /// <inheritdoc />
    public partial class newdatavilla_id_38 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenity", "CreatedDate", "Details", "ImageUrl", "Name", "Occupancy", "Rate", "Sqft", "UpdatedDate", "abc" },
                values: new object[] { 39, "", new DateTime(2024, 3, 22, 0, 34, 11, 955, DateTimeKind.Local).AddTicks(5358), "dsadsad auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.", "https://dotnetmastery.com/bluevillaimages/villa2.jpg", "39", 4, 1600.0, 1100, new DateTime(2024, 3, 22, 0, 34, 11, 955, DateTimeKind.Local).AddTicks(5375), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 39);
        }
    }
}
