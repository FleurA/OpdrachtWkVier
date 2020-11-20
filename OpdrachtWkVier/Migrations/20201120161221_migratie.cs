using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpdrachtWkVier.Migrations
{
    public partial class migratie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locaties",
                columns: table => new
                {
                    postcode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    huisnummer = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locaties", x => new { x.postcode, x.huisnummer });
                });

            migrationBuilder.CreateTable(
                name: "Gigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    prijsPerKwartier = table.Column<double>(type: "float", nullable: false),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Omschrijving = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    duurInMin = table.Column<int>(type: "int", nullable: false),
                    BoekingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BoekingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArtiestGig",
                columns: table => new
                {
                    GigsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    artiestenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtiestGig", x => new { x.GigsId, x.artiestenId });
                    table.ForeignKey(
                        name: "FK_ArtiestGig_Gigs_GigsId",
                        column: x => x.GigsId,
                        principalTable: "Gigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtiestGig_Users_artiestenId",
                        column: x => x.artiestenId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Boekingen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    locatiepostcode = table.Column<string>(type: "nvarchar(6)", nullable: true),
                    locatiehuisnummer = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BoekerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boekingen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boekingen_Locaties_locatiepostcode_locatiehuisnummer",
                        columns: x => new { x.locatiepostcode, x.locatiehuisnummer },
                        principalTable: "Locaties",
                        principalColumns: new[] { "postcode", "huisnummer" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Boekingen_Users_BoekerId",
                        column: x => x.BoekerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Profielen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtiestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomCss = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profielen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profielen_Users_ArtiestId",
                        column: x => x.ArtiestId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtiestGig_artiestenId",
                table: "ArtiestGig",
                column: "artiestenId");

            migrationBuilder.CreateIndex(
                name: "IX_Boekingen_BoekerId",
                table: "Boekingen",
                column: "BoekerId");

            migrationBuilder.CreateIndex(
                name: "IX_Boekingen_locatiepostcode_locatiehuisnummer",
                table: "Boekingen",
                columns: new[] { "locatiepostcode", "locatiehuisnummer" });

            migrationBuilder.CreateIndex(
                name: "IX_Gigs_BoekingId",
                table: "Gigs",
                column: "BoekingId");

            migrationBuilder.CreateIndex(
                name: "IX_Profielen_ArtiestId",
                table: "Profielen",
                column: "ArtiestId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_BoekingId",
                table: "Users",
                column: "BoekingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gigs_Boekingen_BoekingId",
                table: "Gigs",
                column: "BoekingId",
                principalTable: "Boekingen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Boekingen_BoekingId",
                table: "Users",
                column: "BoekingId",
                principalTable: "Boekingen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boekingen_Users_BoekerId",
                table: "Boekingen");

            migrationBuilder.DropTable(
                name: "ArtiestGig");

            migrationBuilder.DropTable(
                name: "Profielen");

            migrationBuilder.DropTable(
                name: "Gigs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Boekingen");

            migrationBuilder.DropTable(
                name: "Locaties");
        }
    }
}
