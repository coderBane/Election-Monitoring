using System;
using Election2023.Domain.Enums;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Election2023.MigrationsNpgsql
{
    /// <inheritdoc />
    public partial class AddCandidacyDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:app.election_type", "presidential,gubernatorial,senatorial,house_of_representatives,house_of_assembly")
                .Annotation("Npgsql:Enum:app.gender", "male,female,unspecified")
                .Annotation("Npgsql:Enum:app.party", "a,aa,aac,adc,adp,apc,apm,app,apga,bp,lp,nnpp,nrm,pdp,prp,sdp,ypp,zlp")
                .Annotation("Npgsql:Enum:app.state_name", "abia,adamawa,akwa_ibom,anambra,bauchi,bayelsa,benue,borno,cross_river,delta,ebonyi,edo,ekiti,enugu,gombe,imo,jigawa,kaduna,kano,katsina,kebbi,kogi,kwara,lagos,nasarawa,niger,ogun,ondo,osun,oyo,plateau,rivers,sokoto,taraba,yobe,zamfara,fct");

            migrationBuilder.CreateSequence(
                name: "db_seq_hilo",
                schema: "app",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "political_parties",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    abbrv = table.Column<Party>(type: "app.party", nullable: false),
                    logo = table.Column<string>(type: "text", nullable: true),
                    colour = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_political_parties", x => x.id);
                    table.UniqueConstraint("ak_political_parties_abbrv", x => x.abbrv);
                });

            migrationBuilder.CreateTable(
                name: "candidates",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    party_abbrv = table.Column<Party>(type: "app.party", nullable: false),
                    category = table.Column<ElectionType>(type: "app.election_type", nullable: false),
                    one_to_watch = table.Column<bool>(type: "boolean", nullable: false),
                    incumbent = table.Column<bool>(type: "boolean", nullable: false),
                    firstname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    middlename = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    surname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    gender = table.Column<Gender>(type: "app.gender", nullable: false),
                    age = table.Column<int>(type: "integer", nullable: false),
                    education = table.Column<string>(type: "text", nullable: false),
                    image = table.Column<string>(type: "text", nullable: true),
                    brief = table.Column<string>(type: "text", nullable: true),
                    constituency = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    manifesto_snippets = table.Column<string[]>(type: "text[]", nullable: false),
                    political_party_id = table.Column<int>(type: "integer", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_by = table.Column<string>(type: "text", nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_candidates", x => x.id);
                    table.ForeignKey(
                        name: "fk_candidates_political_parties_party_id",
                        column: x => x.party_abbrv,
                        principalSchema: "app",
                        principalTable: "political_parties",
                        principalColumn: "abbrv",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_candidates_political_parties_political_party_id",
                        column: x => x.political_party_id,
                        principalSchema: "app",
                        principalTable: "political_parties",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_candidates_firstname_surname",
                schema: "app",
                table: "candidates",
                columns: new[] { "firstname", "surname" });

            migrationBuilder.CreateIndex(
                name: "ix_candidates_party_abbrv",
                schema: "app",
                table: "candidates",
                column: "party_abbrv");

            migrationBuilder.CreateIndex(
                name: "ix_candidates_political_party_id",
                schema: "app",
                table: "candidates",
                column: "political_party_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "candidates",
                schema: "app");

            migrationBuilder.DropTable(
                name: "political_parties",
                schema: "app");

            migrationBuilder.DropSequence(
                name: "db_seq_hilo",
                schema: "app");

            migrationBuilder.EnsureSchema(
                name: "identity");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:app.election_type", "presidential,gubernatorial,senatorial,house_of_representatives,house_of_assembly")
                .OldAnnotation("Npgsql:Enum:app.gender", "male,female,unspecified")
                .OldAnnotation("Npgsql:Enum:app.party", "a,aa,aac,adc,adp,apc,apm,app,apga,bp,lp,nnpp,nrm,pdp,prp,sdp,ypp,zlp")
                .OldAnnotation("Npgsql:Enum:app.state_name", "abia,adamawa,akwa_ibom,anambra,bauchi,bayelsa,benue,borno,cross_river,delta,ebonyi,edo,ekiti,enugu,gombe,imo,jigawa,kaduna,kano,katsina,kebbi,kogi,kwara,lagos,nasarawa,niger,ogun,ondo,osun,oyo,plateau,rivers,sokoto,taraba,yobe,zamfara,fct");
        }
    }
}
