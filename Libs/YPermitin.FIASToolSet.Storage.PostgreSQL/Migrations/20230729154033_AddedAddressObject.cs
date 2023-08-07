using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.PostgreSQL.Migrations
{
    public partial class AddedAddressObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FIASVersionInstallationsTypes_Id",
                table: "FIASVersionInstallationsTypes");

            migrationBuilder.DropIndex(
                name: "IX_FIASVersionInstallationStatuses_Id",
                table: "FIASVersionInstallationStatuses");

            migrationBuilder.DropIndex(
                name: "IX_FIASVersionInstallations_Id",
                table: "FIASVersionInstallations");

            migrationBuilder.DropIndex(
                name: "IX_FIASRoomTypes_Id",
                table: "FIASRoomTypes");

            migrationBuilder.DropIndex(
                name: "IX_FIASParameterTypes_Id",
                table: "FIASParameterTypes");

            migrationBuilder.DropIndex(
                name: "IX_FIASOperationTypes_Id",
                table: "FIASOperationTypes");

            migrationBuilder.DropIndex(
                name: "IX_FIASObjectLevels_Level",
                table: "FIASObjectLevels");

            migrationBuilder.DropIndex(
                name: "IX_FIASNormativeDocTypes_Id",
                table: "FIASNormativeDocTypes");

            migrationBuilder.DropIndex(
                name: "IX_FIASNormativeDocKinds_Id",
                table: "FIASNormativeDocKinds");

            migrationBuilder.DropIndex(
                name: "IX_FIASHouseTypes_Id",
                table: "FIASHouseTypes");

            migrationBuilder.DropIndex(
                name: "IX_FIASApartmentTypes_Id",
                table: "FIASApartmentTypes");

            migrationBuilder.DropIndex(
                name: "IX_FIASAddressObjectTypes_Id",
                table: "FIASAddressObjectTypes");

            migrationBuilder.CreateTable(
                name: "AddressObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ObjectId = table.Column<int>(type: "integer", nullable: false),
                    ObjectGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangeId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TypeName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LevelId = table.Column<int>(type: "integer", nullable: false),
                    OperationTypeId = table.Column<int>(type: "integer", nullable: false),
                    PreviousAddressObjectId = table.Column<int>(type: "integer", nullable: true),
                    NextAddressObjectId = table.Column<int>(type: "integer", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsActual = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddressObjects_AddressObjects_NextAddressObjectId",
                        column: x => x.NextAddressObjectId,
                        principalTable: "AddressObjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AddressObjects_AddressObjects_PreviousAddressObjectId",
                        column: x => x.PreviousAddressObjectId,
                        principalTable: "AddressObjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AddressObjects_FIASObjectLevels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "FIASObjectLevels",
                        principalColumn: "Level",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AddressObjects_FIASOperationTypes_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "FIASOperationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddressObjects_LevelId",
                table: "AddressObjects",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_AddressObjects_NextAddressObjectId",
                table: "AddressObjects",
                column: "NextAddressObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AddressObjects_OperationTypeId",
                table: "AddressObjects",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AddressObjects_PreviousAddressObjectId",
                table: "AddressObjects",
                column: "PreviousAddressObjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressObjects");

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallationsTypes_Id",
                table: "FIASVersionInstallationsTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallationStatuses_Id",
                table: "FIASVersionInstallationStatuses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallations_Id",
                table: "FIASVersionInstallations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FIASRoomTypes_Id",
                table: "FIASRoomTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FIASParameterTypes_Id",
                table: "FIASParameterTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FIASOperationTypes_Id",
                table: "FIASOperationTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FIASObjectLevels_Level",
                table: "FIASObjectLevels",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_FIASNormativeDocTypes_Id",
                table: "FIASNormativeDocTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FIASNormativeDocKinds_Id",
                table: "FIASNormativeDocKinds",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FIASHouseTypes_Id",
                table: "FIASHouseTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FIASApartmentTypes_Id",
                table: "FIASApartmentTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FIASAddressObjectTypes_Id",
                table: "FIASAddressObjectTypes",
                column: "Id");
        }
    }
}
