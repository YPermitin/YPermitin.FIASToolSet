using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.SQLServer.Migrations
{
    public partial class FixMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NormativeDocType",
                table: "NormativeDocType");

            migrationBuilder.RenameTable(
                name: "NormativeDocType",
                newName: "FIASNormativeDocTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FIASNormativeDocTypes",
                table: "FIASNormativeDocTypes",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FIASNormativeDocTypes",
                table: "FIASNormativeDocTypes");

            migrationBuilder.RenameTable(
                name: "FIASNormativeDocTypes",
                newName: "NormativeDocType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NormativeDocType",
                table: "NormativeDocType",
                column: "Id");
        }
    }
}
