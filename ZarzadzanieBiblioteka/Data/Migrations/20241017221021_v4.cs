using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZarzadzanieBiblioteka.Data.Migrations
{
    /// <inheritdoc />
    public partial class v4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Okladka",
                table: "Ksiazki",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Okladka",
                table: "Ksiazki");
        }
    }
}
