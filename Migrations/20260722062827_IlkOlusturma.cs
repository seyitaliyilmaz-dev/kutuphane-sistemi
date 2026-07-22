using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kutuphane_sistemi.Migrations
{
    /// <inheritdoc />
    public partial class IlkOlusturma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ogrenciler",
                columns: table => new
                {
                    OgrenciID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdSoyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OgrenciNo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ogrenciler", x => x.OgrenciID);
                });

            migrationBuilder.CreateTable(
                name: "Yazarlar",
                columns: table => new
                {
                    YazarID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdSoyad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yazarlar", x => x.YazarID);
                });

            migrationBuilder.CreateTable(
                name: "Kitaplar",
                columns: table => new
                {
                    KitapID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OduncDurumu = table.Column<bool>(type: "bit", nullable: false),
                    YazarID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kitaplar", x => x.KitapID);
                    table.ForeignKey(
                        name: "FK_Kitaplar_Yazarlar_YazarID",
                        column: x => x.YazarID,
                        principalTable: "Yazarlar",
                        principalColumn: "YazarID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OdunAlmalar",
                columns: table => new
                {
                    OdunAlmaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KitapID = table.Column<int>(type: "int", nullable: false),
                    OgrenciID = table.Column<int>(type: "int", nullable: false),
                    AlisTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SonTeslimTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IadeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdunAlmalar", x => x.OdunAlmaID);
                    table.ForeignKey(
                        name: "FK_OdunAlmalar_Kitaplar_KitapID",
                        column: x => x.KitapID,
                        principalTable: "Kitaplar",
                        principalColumn: "KitapID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OdunAlmalar_Ogrenciler_OgrenciID",
                        column: x => x.OgrenciID,
                        principalTable: "Ogrenciler",
                        principalColumn: "OgrenciID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kitaplar_YazarID",
                table: "Kitaplar",
                column: "YazarID");

            migrationBuilder.CreateIndex(
                name: "IX_OdunAlmalar_KitapID",
                table: "OdunAlmalar",
                column: "KitapID");

            migrationBuilder.CreateIndex(
                name: "IX_OdunAlmalar_OgrenciID",
                table: "OdunAlmalar",
                column: "OgrenciID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OdunAlmalar");

            migrationBuilder.DropTable(
                name: "Kitaplar");

            migrationBuilder.DropTable(
                name: "Ogrenciler");

            migrationBuilder.DropTable(
                name: "Yazarlar");
        }
    }
}
