using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZarzadzanieBiblioteka.Data.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nazwa",
                table: "Ksiazki",
                newName: "Tytul");

            migrationBuilder.AddColumn<int>(
                name: "ISBN",
                table: "Ksiazki",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Oprawa",
                table: "Ksiazki",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Wydanie",
                table: "Ksiazki",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccessLevel",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "Ksiazki");

            migrationBuilder.DropColumn(
                name: "Oprawa",
                table: "Ksiazki");

            migrationBuilder.DropColumn(
                name: "Wydanie",
                table: "Ksiazki");

            migrationBuilder.DropColumn(
                name: "AccessLevel",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Tytul",
                table: "Ksiazki",
                newName: "Nazwa");
        }
    }
}
