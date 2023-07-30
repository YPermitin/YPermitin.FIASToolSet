using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.SQLServer.Migrations
{
    public partial class FixTableNaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddressObjects_AddressObjects_NextAddressObjectId",
                table: "AddressObjects");

            migrationBuilder.DropForeignKey(
                name: "FK_AddressObjects_AddressObjects_PreviousAddressObjectId",
                table: "AddressObjects");

            migrationBuilder.DropForeignKey(
                name: "FK_AddressObjects_FIASObjectLevels_LevelId",
                table: "AddressObjects");

            migrationBuilder.DropForeignKey(
                name: "FK_AddressObjects_FIASOperationTypes_OperationTypeId",
                table: "AddressObjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AddressObjects",
                table: "AddressObjects");

            migrationBuilder.RenameTable(
                name: "AddressObjects",
                newName: "FIASAddressObjects");

            migrationBuilder.RenameIndex(
                name: "IX_AddressObjects_PreviousAddressObjectId",
                table: "FIASAddressObjects",
                newName: "IX_FIASAddressObjects_PreviousAddressObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_AddressObjects_OperationTypeId",
                table: "FIASAddressObjects",
                newName: "IX_FIASAddressObjects_OperationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_AddressObjects_NextAddressObjectId",
                table: "FIASAddressObjects",
                newName: "IX_FIASAddressObjects_NextAddressObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_AddressObjects_LevelId",
                table: "FIASAddressObjects",
                newName: "IX_FIASAddressObjects_LevelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FIASAddressObjects",
                table: "FIASAddressObjects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FIASAddressObjects_FIASAddressObjects_NextAddressObjectId",
                table: "FIASAddressObjects",
                column: "NextAddressObjectId",
                principalTable: "FIASAddressObjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FIASAddressObjects_FIASAddressObjects_PreviousAddressObjectId",
                table: "FIASAddressObjects",
                column: "PreviousAddressObjectId",
                principalTable: "FIASAddressObjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FIASAddressObjects_FIASObjectLevels_LevelId",
                table: "FIASAddressObjects",
                column: "LevelId",
                principalTable: "FIASObjectLevels",
                principalColumn: "Level",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FIASAddressObjects_FIASOperationTypes_OperationTypeId",
                table: "FIASAddressObjects",
                column: "OperationTypeId",
                principalTable: "FIASOperationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FIASAddressObjects_FIASAddressObjects_NextAddressObjectId",
                table: "FIASAddressObjects");

            migrationBuilder.DropForeignKey(
                name: "FK_FIASAddressObjects_FIASAddressObjects_PreviousAddressObjectId",
                table: "FIASAddressObjects");

            migrationBuilder.DropForeignKey(
                name: "FK_FIASAddressObjects_FIASObjectLevels_LevelId",
                table: "FIASAddressObjects");

            migrationBuilder.DropForeignKey(
                name: "FK_FIASAddressObjects_FIASOperationTypes_OperationTypeId",
                table: "FIASAddressObjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FIASAddressObjects",
                table: "FIASAddressObjects");

            migrationBuilder.RenameTable(
                name: "FIASAddressObjects",
                newName: "AddressObjects");

            migrationBuilder.RenameIndex(
                name: "IX_FIASAddressObjects_PreviousAddressObjectId",
                table: "AddressObjects",
                newName: "IX_AddressObjects_PreviousAddressObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_FIASAddressObjects_OperationTypeId",
                table: "AddressObjects",
                newName: "IX_AddressObjects_OperationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_FIASAddressObjects_NextAddressObjectId",
                table: "AddressObjects",
                newName: "IX_AddressObjects_NextAddressObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_FIASAddressObjects_LevelId",
                table: "AddressObjects",
                newName: "IX_AddressObjects_LevelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AddressObjects",
                table: "AddressObjects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AddressObjects_AddressObjects_NextAddressObjectId",
                table: "AddressObjects",
                column: "NextAddressObjectId",
                principalTable: "AddressObjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AddressObjects_AddressObjects_PreviousAddressObjectId",
                table: "AddressObjects",
                column: "PreviousAddressObjectId",
                principalTable: "AddressObjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AddressObjects_FIASObjectLevels_LevelId",
                table: "AddressObjects",
                column: "LevelId",
                principalTable: "FIASObjectLevels",
                principalColumn: "Level",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AddressObjects_FIASOperationTypes_OperationTypeId",
                table: "AddressObjects",
                column: "OperationTypeId",
                principalTable: "FIASOperationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
