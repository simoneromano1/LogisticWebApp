using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticWebApp.Migrations
{
    /// <inheritdoc />
    public partial class Renaming3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCliente",
                table: "Notifiche",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdSpedizione",
                table: "Notifiche",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdCliente",
                table: "Notifiche");

            migrationBuilder.DropColumn(
                name: "IdSpedizione",
                table: "Notifiche");
        }
    }
}
