using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.PostgreSQL.Migrations
{
    public partial class RemoveUnnecessaryRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FIASAddressObjects_FIASAddressObjects_NextAddressObjectId",
                table: "FIASAddressObjects");

            migrationBuilder.DropForeignKey(
                name: "FK_FIASAddressObjects_FIASAddressObjects_PreviousAddressObject~",
                table: "FIASAddressObjects");

            migrationBuilder.DropIndex(
                name: "IX_FIASAddressObjects_NextAddressObjectId",
                table: "FIASAddressObjects");

            migrationBuilder.DropIndex(
                name: "IX_FIASAddressObjects_PreviousAddressObjectId",
                table: "FIASAddressObjects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FIASAddressObjects_NextAddressObjectId",
                table: "FIASAddressObjects",
                column: "NextAddressObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASAddressObjects_PreviousAddressObjectId",
                table: "FIASAddressObjects",
                column: "PreviousAddressObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_FIASAddressObjects_FIASAddressObjects_NextAddressObjectId",
                table: "FIASAddressObjects",
                column: "NextAddressObjectId",
                principalTable: "FIASAddressObjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FIASAddressObjects_FIASAddressObjects_PreviousAddressObject~",
                table: "FIASAddressObjects",
                column: "PreviousAddressObjectId",
                principalTable: "FIASAddressObjects",
                principalColumn: "Id");
        }
    }
}
