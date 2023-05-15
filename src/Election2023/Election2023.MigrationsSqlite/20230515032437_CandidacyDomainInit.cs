using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Election2023.MigrationsSqlite
{
    /// <inheritdoc />
    public partial class CandidacyDomainInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PoliticalParties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Abbrv = table.Column<string>(type: "TEXT", nullable: false),
                    Logo = table.Column<string>(type: "TEXT", nullable: true),
                    Colour = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliticalParties", x => x.Id);
                    table.UniqueConstraint("AK_PoliticalParties_Abbrv", x => x.Abbrv);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    PartyAbbrv = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    OneToWatch = table.Column<bool>(type: "INTEGER", nullable: false),
                    Incumbent = table.Column<bool>(type: "INTEGER", nullable: false),
                    Firstname = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Middlename = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Surname = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "TEXT", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    Education = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: true),
                    Brief = table.Column<string>(type: "TEXT", nullable: true),
                    Constituency = table.Column<string>(type: "TEXT", nullable: true),
                    State = table.Column<string>(type: "TEXT", nullable: true),
                    ManifestoSnippets = table.Column<string>(type: "TEXT", nullable: false),
                    PoliticalPartyId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidates_PoliticalParties_PartyAbbrv",
                        column: x => x.PartyAbbrv,
                        principalTable: "PoliticalParties",
                        principalColumn: "Abbrv",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Candidates_PoliticalParties_PoliticalPartyId",
                        column: x => x.PoliticalPartyId,
                        principalTable: "PoliticalParties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_Firstname_Surname",
                table: "Candidates",
                columns: new[] { "Firstname", "Surname" });

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_PartyAbbrv",
                table: "Candidates",
                column: "PartyAbbrv");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_PoliticalPartyId",
                table: "Candidates",
                column: "PoliticalPartyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "PoliticalParties");
        }
    }
}
