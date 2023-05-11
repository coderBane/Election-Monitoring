using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Election2023.MigrationsSqlite
{
    /// <inheritdoc />
    public partial class AddCandidacy : Migration
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
                    Abbrv = table.Column<int>(type: "INTEGER", nullable: false),
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
                    PoliticalPartyAbbrv = table.Column<int>(type: "INTEGER", nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    OneToWatch = table.Column<bool>(type: "INTEGER", nullable: false),
                    Incumbent = table.Column<bool>(type: "INTEGER", nullable: false),
                    Firstname = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Middlename = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Surname = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Gender = table.Column<int>(type: "INTEGER", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    StateOfOrigin = table.Column<string>(type: "TEXT", nullable: true),
                    Image = table.Column<string>(type: "TEXT", nullable: true),
                    Brief = table.Column<string>(type: "TEXT", nullable: true),
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
                        name: "FK_Candidates_PoliticalParties_PoliticalPartyAbbrv",
                        column: x => x.PoliticalPartyAbbrv,
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
                name: "IX_Candidates_PoliticalPartyAbbrv",
                table: "Candidates",
                column: "PoliticalPartyAbbrv");

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
