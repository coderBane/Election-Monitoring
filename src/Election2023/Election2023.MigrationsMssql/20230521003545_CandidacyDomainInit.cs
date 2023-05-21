using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Election2023.MigrationsMssql
{
    /// <inheritdoc />
    public partial class CandidacyDomainInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateSequence(
                name: "DBSeqHiLo",
                schema: "app",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "PoliticalParties",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Abbrv = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Colour = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliticalParties", x => x.Id);
                    table.UniqueConstraint("AK_PoliticalParties_Abbrv", x => x.Abbrv);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PartyAbbrv = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OneToWatch = table.Column<bool>(type: "bit", nullable: false),
                    Incumbent = table.Column<bool>(type: "bit", nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Middlename = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Education = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brief = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Constituency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManifestoSnippets = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PoliticalPartyId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidates_PoliticalParties_PartyAbbrv",
                        column: x => x.PartyAbbrv,
                        principalSchema: "app",
                        principalTable: "PoliticalParties",
                        principalColumn: "Abbrv",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Candidates_PoliticalParties_PoliticalPartyId",
                        column: x => x.PoliticalPartyId,
                        principalSchema: "app",
                        principalTable: "PoliticalParties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_Firstname_Surname",
                schema: "app",
                table: "Candidates",
                columns: new[] { "Firstname", "Surname" });

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_PartyAbbrv",
                schema: "app",
                table: "Candidates",
                column: "PartyAbbrv");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_PoliticalPartyId",
                schema: "app",
                table: "Candidates",
                column: "PoliticalPartyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candidates",
                schema: "app");

            migrationBuilder.DropTable(
                name: "PoliticalParties",
                schema: "app");

            migrationBuilder.DropSequence(
                name: "DBSeqHiLo",
                schema: "app");

            migrationBuilder.EnsureSchema(
                name: "identity");
        }
    }
}
