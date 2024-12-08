using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZarzadzanieBiblioteka.Data.Migrations
{
    /// <inheritdoc />
    public partial class v8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ksiazki_Autorzy_AutorId",
                table: "Ksiazki");

            migrationBuilder.AddForeignKey(
                name: "FK_Ksiazki_Autorzy_AutorId",
                table: "Ksiazki",
                column: "AutorId",
                principalTable: "Autorzy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ksiazki_Autorzy_AutorId",
                table: "Ksiazki");

            migrationBuilder.AddForeignKey(
                name: "FK_Ksiazki_Autorzy_AutorId",
                table: "Ksiazki",
                column: "AutorId",
                principalTable: "Autorzy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
