using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZarzadzanieBiblioteka.Data.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataUrodzenia",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Autorzy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autorzy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Biblioteki",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Miejscowosc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ulica = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NumerBudynku = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    KodPocztowy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biblioteki", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ksiazki",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gatunek = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DataWydania = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LiczbaStron = table.Column<int>(type: "int", nullable: false),
                    BibliotekaId = table.Column<int>(type: "int", nullable: false),
                    AutorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ksiazki", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ksiazki_Autorzy_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Autorzy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ksiazki_Biblioteki_BibliotekaId",
                        column: x => x.BibliotekaId,
                        principalTable: "Biblioteki",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Opinie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Komentarz = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Ocena = table.Column<byte>(type: "tinyint", nullable: false),
                    KsiazkaId = table.Column<int>(type: "int", nullable: false),
                    UzytkownikId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opinie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Opinie_AspNetUsers_UzytkownikId",
                        column: x => x.UzytkownikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Opinie_Ksiazki_KsiazkaId",
                        column: x => x.KsiazkaId,
                        principalTable: "Ksiazki",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Woluminy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KsiazkaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Woluminy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Woluminy_Ksiazki_KsiazkaId",
                        column: x => x.KsiazkaId,
                        principalTable: "Ksiazki",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rezerwacje",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataRezerwacji = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataWygasniecia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WoluminId = table.Column<int>(type: "int", nullable: false),
                    UzytkownikId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    KsiazkaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezerwacje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rezerwacje_AspNetUsers_UzytkownikId",
                        column: x => x.UzytkownikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rezerwacje_Ksiazki_KsiazkaId",
                        column: x => x.KsiazkaId,
                        principalTable: "Ksiazki",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rezerwacje_Woluminy_WoluminId",
                        column: x => x.WoluminId,
                        principalTable: "Woluminy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wypozyczenia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataWypozyczenia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataZwrotu = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WoluminId = table.Column<int>(type: "int", nullable: false),
                    UzytkownikId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    KsiazkaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wypozyczenia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wypozyczenia_AspNetUsers_UzytkownikId",
                        column: x => x.UzytkownikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wypozyczenia_Ksiazki_KsiazkaId",
                        column: x => x.KsiazkaId,
                        principalTable: "Ksiazki",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Wypozyczenia_Woluminy_WoluminId",
                        column: x => x.WoluminId,
                        principalTable: "Woluminy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ksiazki_AutorId",
                table: "Ksiazki",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Ksiazki_BibliotekaId",
                table: "Ksiazki",
                column: "BibliotekaId");

            migrationBuilder.CreateIndex(
                name: "IX_Opinie_KsiazkaId",
                table: "Opinie",
                column: "KsiazkaId");

            migrationBuilder.CreateIndex(
                name: "IX_Opinie_UzytkownikId",
                table: "Opinie",
                column: "UzytkownikId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezerwacje_KsiazkaId",
                table: "Rezerwacje",
                column: "KsiazkaId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezerwacje_UzytkownikId",
                table: "Rezerwacje",
                column: "UzytkownikId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezerwacje_WoluminId",
                table: "Rezerwacje",
                column: "WoluminId");

            migrationBuilder.CreateIndex(
                name: "IX_Woluminy_KsiazkaId",
                table: "Woluminy",
                column: "KsiazkaId");

            migrationBuilder.CreateIndex(
                name: "IX_Wypozyczenia_KsiazkaId",
                table: "Wypozyczenia",
                column: "KsiazkaId");

            migrationBuilder.CreateIndex(
                name: "IX_Wypozyczenia_UzytkownikId",
                table: "Wypozyczenia",
                column: "UzytkownikId");

            migrationBuilder.CreateIndex(
                name: "IX_Wypozyczenia_WoluminId",
                table: "Wypozyczenia",
                column: "WoluminId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Opinie");

            migrationBuilder.DropTable(
                name: "Rezerwacje");

            migrationBuilder.DropTable(
                name: "Wypozyczenia");

            migrationBuilder.DropTable(
                name: "Woluminy");

            migrationBuilder.DropTable(
                name: "Ksiazki");

            migrationBuilder.DropTable(
                name: "Autorzy");

            migrationBuilder.DropTable(
                name: "Biblioteki");

            migrationBuilder.DropColumn(
                name: "DataUrodzenia",
                table: "AspNetUsers");
        }
    }
}
