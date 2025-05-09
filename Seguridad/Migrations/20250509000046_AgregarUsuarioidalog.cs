using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Seguridad.Migrations
{
    /// <inheritdoc />
    public partial class AgregarUsuarioidalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Logs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Logs");
        }
    }
}
