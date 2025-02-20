using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PruebaTecnica.Migrations
{
    /// <inheritdoc />
    public partial class tarjetayoperacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tarjeta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroTarjeta = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    PIN = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Bloqueada = table.Column<bool>(type: "bit", nullable: false),
                    IntentosFallidos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarjeta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TarjetaId = table.Column<int>(type: "int", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoOperacion = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operaciones_Tarjeta_TarjetaId",
                        column: x => x.TarjetaId,
                        principalTable: "Tarjeta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operaciones_FechaHora",
                table: "Operaciones",
                column: "FechaHora");

            migrationBuilder.CreateIndex(
                name: "IX_Operaciones_TarjetaId",
                table: "Operaciones",
                column: "TarjetaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tarjeta_NumeroTarjeta",
                table: "Tarjeta",
                column: "NumeroTarjeta",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operaciones");

            migrationBuilder.DropTable(
                name: "Tarjeta");
        }
    }
}
