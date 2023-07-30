using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.SQLServer.Migrations
{
    public partial class AddedAddressObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddressObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    ObjectGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LevelId = table.Column<int>(type: "int", nullable: false),
                    OperationTypeId = table.Column<int>(type: "int", nullable: false),
                    PreviousAddressObjectId = table.Column<int>(type: "int", nullable: true),
                    NextAddressObjectId = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActual = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
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
        }
    }
}
