using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API127.Migrations
{
    /// <inheritdoc />
    public partial class test21092024_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amenity",
                table: "Villas",
                newName: "Amenitys");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 39,
                column: "Amenitys",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amenitys",
                table: "Villas",
                newName: "Amenity");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 39,
                column: "Amenity",
                value: "");
        }
    }
}
