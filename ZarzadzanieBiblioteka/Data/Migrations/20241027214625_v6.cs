using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZarzadzanieBiblioteka.Data.Migrations
{
    /// <inheritdoc />
    public partial class v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ksiazki_Biblioteki_BibliotekaId",
                table: "Ksiazki");

            migrationBuilder.AlterColumn<int>(
                name: "BibliotekaId",
                table: "Ksiazki",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Ksiazki_Biblioteki_BibliotekaId",
                table: "Ksiazki",
                column: "BibliotekaId",
                principalTable: "Biblioteki",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ksiazki_Biblioteki_BibliotekaId",
                table: "Ksiazki");

            migrationBuilder.AlterColumn<int>(
                name: "BibliotekaId",
                table: "Ksiazki",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ksiazki_Biblioteki_BibliotekaId",
                table: "Ksiazki",
                column: "BibliotekaId",
                principalTable: "Biblioteki",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
