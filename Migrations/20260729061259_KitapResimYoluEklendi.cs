using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kutuphane_sistemi.Migrations
{
    /// <inheritdoc />
    public partial class KitapResimYoluEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResimYolu",
                table: "Kitaplar",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResimYolu",
                table: "Kitaplar");
        }
    }
}
