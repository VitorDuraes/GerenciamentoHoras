using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciamentoHoras.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "RegistrosHoras",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nome",
                table: "RegistrosHoras");
        }
    }
}
