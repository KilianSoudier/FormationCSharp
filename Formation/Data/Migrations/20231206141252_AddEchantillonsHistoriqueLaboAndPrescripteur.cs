using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Formation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEchantillonsHistoriqueLaboAndPrescripteur : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Statut",
                table: "Echantillons",
                newName: "PrescripteurId");

            migrationBuilder.AddColumn<int>(
                name: "LaboratoireId",
                table: "Echantillons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Laboratoires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Laboratoires", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prescripteurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denomination = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescripteurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EchantillonHistoriques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EchantillonId = table.Column<int>(type: "int", nullable: false),
                    Statut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LaboratoireId = table.Column<int>(type: "int", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EchantillonHistoriques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EchantillonHistoriques_Echantillons_EchantillonId",
                        column: x => x.EchantillonId,
                        principalTable: "Echantillons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EchantillonHistoriques_Laboratoires_LaboratoireId",
                        column: x => x.LaboratoireId,
                        principalTable: "Laboratoires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Echantillons_LaboratoireId",
                table: "Echantillons",
                column: "LaboratoireId");

            migrationBuilder.CreateIndex(
                name: "IX_Echantillons_PrescripteurId",
                table: "Echantillons",
                column: "PrescripteurId");

            migrationBuilder.CreateIndex(
                name: "IX_EchantillonHistoriques_EchantillonId",
                table: "EchantillonHistoriques",
                column: "EchantillonId");

            migrationBuilder.CreateIndex(
                name: "IX_EchantillonHistoriques_LaboratoireId",
                table: "EchantillonHistoriques",
                column: "LaboratoireId");

            migrationBuilder.AddForeignKey(
                name: "FK_Echantillons_Laboratoires_LaboratoireId",
                table: "Echantillons",
                column: "LaboratoireId",
                principalTable: "Laboratoires",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Echantillons_Prescripteurs_PrescripteurId",
                table: "Echantillons",
                column: "PrescripteurId",
                principalTable: "Prescripteurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Echantillons_Laboratoires_LaboratoireId",
                table: "Echantillons");

            migrationBuilder.DropForeignKey(
                name: "FK_Echantillons_Prescripteurs_PrescripteurId",
                table: "Echantillons");

            migrationBuilder.DropTable(
                name: "EchantillonHistoriques");

            migrationBuilder.DropTable(
                name: "Prescripteurs");

            migrationBuilder.DropTable(
                name: "Laboratoires");

            migrationBuilder.DropIndex(
                name: "IX_Echantillons_LaboratoireId",
                table: "Echantillons");

            migrationBuilder.DropIndex(
                name: "IX_Echantillons_PrescripteurId",
                table: "Echantillons");

            migrationBuilder.DropColumn(
                name: "LaboratoireId",
                table: "Echantillons");

            migrationBuilder.RenameColumn(
                name: "PrescripteurId",
                table: "Echantillons",
                newName: "Statut");
        }
    }
}
