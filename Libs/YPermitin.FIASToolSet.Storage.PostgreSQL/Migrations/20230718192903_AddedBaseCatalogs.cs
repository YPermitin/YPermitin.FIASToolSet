using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.PostgreSQL.Migrations
{
    public partial class AddedBaseCatalogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FIASAddressObjectTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    ShortName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASAddressObjectTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASApartmentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    ShortName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASApartmentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASHouseTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    ShortName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASHouseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASNormativeDocKinds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASNormativeDocKinds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASNormativeDocTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASNormativeDocTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASObjectLevels",
                columns: table => new
                {
                    Level = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASObjectLevels", x => x.Level);
                });

            migrationBuilder.CreateTable(
                name: "FIASOperationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASOperationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASParameterTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Code = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASParameterTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASRoomTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASRoomTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FIASAddressObjectTypes_Id",
                table: "FIASAddressObjectTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FIASApartmentTypes_Id",
                table: "FIASApartmentTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FIASHouseTypes_Id",
                table: "FIASHouseTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FIASNormativeDocKinds_Id",
                table: "FIASNormativeDocKinds",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FIASNormativeDocTypes_Id",
                table: "FIASNormativeDocTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FIASObjectLevels_Level",
                table: "FIASObjectLevels",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_FIASOperationTypes_Id",
                table: "FIASOperationTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FIASParameterTypes_Id",
                table: "FIASParameterTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FIASRoomTypes_Id",
                table: "FIASRoomTypes",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FIASAddressObjectTypes");

            migrationBuilder.DropTable(
                name: "FIASApartmentTypes");

            migrationBuilder.DropTable(
                name: "FIASHouseTypes");

            migrationBuilder.DropTable(
                name: "FIASNormativeDocKinds");

            migrationBuilder.DropTable(
                name: "FIASNormativeDocTypes");

            migrationBuilder.DropTable(
                name: "FIASObjectLevels");

            migrationBuilder.DropTable(
                name: "FIASOperationTypes");

            migrationBuilder.DropTable(
                name: "FIASParameterTypes");

            migrationBuilder.DropTable(
                name: "FIASRoomTypes");
        }
    }
}
