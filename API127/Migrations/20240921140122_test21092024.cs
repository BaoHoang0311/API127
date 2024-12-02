using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API127.Migrations
{
    /// <inheritdoc />
    public partial class test21092024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "abc",
                table: "Villas",
                newName: "cde");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "cde",
                table: "Villas",
                newName: "abc");
        }
    }
}
