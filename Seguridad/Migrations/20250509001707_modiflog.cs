using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Seguridad.Migrations
{
    /// <inheritdoc />
    public partial class modiflog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "Logs");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicio",
                table: "Logs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Logs_UsuarioId",
                table: "Logs",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Usuarios_UsuarioId",
                table: "Logs",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Usuarios_UsuarioId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_UsuarioId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "FechaInicio",
                table: "Logs");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Fecha",
                table: "Logs",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
