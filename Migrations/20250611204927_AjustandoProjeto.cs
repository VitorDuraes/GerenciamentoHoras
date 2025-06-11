using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciamentoHoras.Migrations
{
    /// <inheritdoc />
    public partial class AjustandoProjeto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Projeto",
                table: "RegistrosHoras",
                newName: "EXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EXT",
                table: "RegistrosHoras",
                newName: "Projeto");
        }
    }
}
