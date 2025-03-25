using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticWebApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clienti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Cognome = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clienti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Corrieri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Cognome = table.Column<string>(type: "TEXT", nullable: false),
                    Telefono = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corrieri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Spedizioni",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Id_cliente = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_corriere = table.Column<int>(type: "INTEGER", nullable: false),
                    Mittente = table.Column<string>(type: "TEXT", nullable: false),
                    Destinatario = table.Column<string>(type: "TEXT", nullable: false),
                    Indirizzo_partenza = table.Column<string>(type: "TEXT", nullable: false),
                    Indirizzo_destinazione = table.Column<string>(type: "TEXT", nullable: false),
                    Tipo_merce = table.Column<string>(type: "TEXT", nullable: false),
                    Richieste_speciali = table.Column<string>(type: "TEXT", nullable: false),
                    Stato = table.Column<string>(type: "TEXT", nullable: false),
                    Data_creazione = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    CorriereId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spedizioni", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Spedizioni_Clienti_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clienti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Spedizioni_Corrieri_CorriereId",
                        column: x => x.CorriereId,
                        principalTable: "Corrieri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SpedizioneId = table.Column<int>(type: "INTEGER", nullable: false),
                    Valutazione = table.Column<int>(type: "INTEGER", nullable: false),
                    Commento = table.Column<string>(type: "TEXT", nullable: false),
                    DataFeedback = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Spedizioni_SpedizioneId",
                        column: x => x.SpedizioneId,
                        principalTable: "Spedizioni",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifiche",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    SpedizioneId = table.Column<int>(type: "INTEGER", nullable: false),
                    Messaggio = table.Column<string>(type: "TEXT", nullable: false),
                    DataNotifica = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StatoNotifica = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifiche", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifiche_Clienti_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clienti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifiche_Spedizioni_SpedizioneId",
                        column: x => x.SpedizioneId,
                        principalTable: "Spedizioni",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clienti_Email",
                table: "Clienti",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Corrieri_Email",
                table: "Corrieri",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_SpedizioneId",
                table: "Feedbacks",
                column: "SpedizioneId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifiche_ClienteId",
                table: "Notifiche",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifiche_SpedizioneId",
                table: "Notifiche",
                column: "SpedizioneId");

            migrationBuilder.CreateIndex(
                name: "IX_Spedizioni_ClienteId",
                table: "Spedizioni",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Spedizioni_CorriereId",
                table: "Spedizioni",
                column: "CorriereId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Notifiche");

            migrationBuilder.DropTable(
                name: "Spedizioni");

            migrationBuilder.DropTable(
                name: "Clienti");

            migrationBuilder.DropTable(
                name: "Corrieri");
        }
    }
}
