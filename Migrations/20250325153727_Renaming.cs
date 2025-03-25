using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticWebApp.Migrations
{
    /// <inheritdoc />
    public partial class Renaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tipo_merce",
                table: "Spedizioni",
                newName: "TipoMerce");

            migrationBuilder.RenameColumn(
                name: "Richieste_speciali",
                table: "Spedizioni",
                newName: "RichiesteSpeciali");

            migrationBuilder.RenameColumn(
                name: "Indirizzo_partenza",
                table: "Spedizioni",
                newName: "IndirizzoPartenza");

            migrationBuilder.RenameColumn(
                name: "Indirizzo_destinazione",
                table: "Spedizioni",
                newName: "IndirizzoDestinazione");

            migrationBuilder.RenameColumn(
                name: "Id_corriere",
                table: "Spedizioni",
                newName: "IdCorriere");

            migrationBuilder.RenameColumn(
                name: "Id_cliente",
                table: "Spedizioni",
                newName: "IdCliente");

            migrationBuilder.RenameColumn(
                name: "Data_creazione",
                table: "Spedizioni",
                newName: "DataCreazione");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TipoMerce",
                table: "Spedizioni",
                newName: "Tipo_merce");

            migrationBuilder.RenameColumn(
                name: "RichiesteSpeciali",
                table: "Spedizioni",
                newName: "Richieste_speciali");

            migrationBuilder.RenameColumn(
                name: "IndirizzoPartenza",
                table: "Spedizioni",
                newName: "Indirizzo_partenza");

            migrationBuilder.RenameColumn(
                name: "IndirizzoDestinazione",
                table: "Spedizioni",
                newName: "Indirizzo_destinazione");

            migrationBuilder.RenameColumn(
                name: "IdCorriere",
                table: "Spedizioni",
                newName: "Id_corriere");

            migrationBuilder.RenameColumn(
                name: "IdCliente",
                table: "Spedizioni",
                newName: "Id_cliente");

            migrationBuilder.RenameColumn(
                name: "DataCreazione",
                table: "Spedizioni",
                newName: "Data_creazione");
        }
    }
}
