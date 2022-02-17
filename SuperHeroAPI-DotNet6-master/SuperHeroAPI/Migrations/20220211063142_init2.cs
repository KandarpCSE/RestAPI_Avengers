using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperHeroAPI.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Place",
                table: "SuperHeroes",
                newName: "RealName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "SuperHeroes",
                newName: "Power");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "SuperHeroes",
                newName: "ImageURl");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "SuperHeroes",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "CharacterName",
                table: "SuperHeroes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CharacterName",
                table: "SuperHeroes");

            migrationBuilder.RenameColumn(
                name: "RealName",
                table: "SuperHeroes",
                newName: "Place");

            migrationBuilder.RenameColumn(
                name: "Power",
                table: "SuperHeroes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ImageURl",
                table: "SuperHeroes",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "SuperHeroes",
                newName: "FirstName");
        }
    }
}
