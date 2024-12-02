using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API127.Migrations
{
    /// <inheritdoc />
    public partial class LocalUser1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "LocalUsers",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LocalUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "LocalUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "LocalUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "LocalUsers");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "LocalUsers");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "LocalUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "LocalUsers",
                newName: "Id");
        }
    }
}
