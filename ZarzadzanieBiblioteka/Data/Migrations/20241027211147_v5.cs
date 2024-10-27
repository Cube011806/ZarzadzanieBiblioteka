using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZarzadzanieBiblioteka.Data.Migrations
{
    /// <inheritdoc />
    public partial class v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezerwacje_Ksiazki_KsiazkaId",
                table: "Rezerwacje");

            migrationBuilder.DropForeignKey(
                name: "FK_Wypozyczenia_Ksiazki_KsiazkaId",
                table: "Wypozyczenia");

            migrationBuilder.DropIndex(
                name: "IX_Wypozyczenia_KsiazkaId",
                table: "Wypozyczenia");

            migrationBuilder.DropIndex(
                name: "IX_Rezerwacje_KsiazkaId",
                table: "Rezerwacje");

            migrationBuilder.DropColumn(
                name: "KsiazkaId",
                table: "Wypozyczenia");

            migrationBuilder.DropColumn(
                name: "KsiazkaId",
                table: "Rezerwacje");

            migrationBuilder.AlterColumn<string>(
                name: "Oprawa",
                table: "Ksiazki",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Gatunek",
                table: "Ksiazki",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KsiazkaId",
                table: "Wypozyczenia",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KsiazkaId",
                table: "Rezerwacje",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Oprawa",
                table: "Ksiazki",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Gatunek",
                table: "Ksiazki",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_Wypozyczenia_KsiazkaId",
                table: "Wypozyczenia",
                column: "KsiazkaId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezerwacje_KsiazkaId",
                table: "Rezerwacje",
                column: "KsiazkaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezerwacje_Ksiazki_KsiazkaId",
                table: "Rezerwacje",
                column: "KsiazkaId",
                principalTable: "Ksiazki",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wypozyczenia_Ksiazki_KsiazkaId",
                table: "Wypozyczenia",
                column: "KsiazkaId",
                principalTable: "Ksiazki",
                principalColumn: "Id");
        }
    }
}
