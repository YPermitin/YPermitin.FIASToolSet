using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.SQLServer.Migrations
{
    /// <inheritdoc />
    public partial class ActualizeMigrationV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FIASChangeHistory_FIASNormativeDocuments_NormativeDocId",
                table: "FIASChangeHistory");

            migrationBuilder.DropIndex(
                name: "IX_FIASChangeHistory_NormativeDocId",
                table: "FIASChangeHistory");

            migrationBuilder.DropIndex(
                name: "IX_FIASChangeHistory_ObjectId_AddressObjectGuid_ChangeId",
                table: "FIASChangeHistory");

            migrationBuilder.AlterColumn<string>(
                name: "RegNumber",
                table: "FIASNormativeDocuments",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OrgName",
                table: "FIASNormativeDocuments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "FIASNormativeDocuments",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FIASObjectsRegistry_ObjectId_ObjectGuid_ChangeId",
                table: "FIASObjectsRegistry",
                columns: new[] { "ObjectId", "ObjectGuid", "ChangeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FIASChangeHistory_ObjectId_AddressObjectGuid_ChangeId",
                table: "FIASChangeHistory",
                columns: new[] { "ObjectId", "AddressObjectGuid", "ChangeId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FIASObjectsRegistry_ObjectId_ObjectGuid_ChangeId",
                table: "FIASObjectsRegistry");

            migrationBuilder.DropIndex(
                name: "IX_FIASChangeHistory_ObjectId_AddressObjectGuid_ChangeId",
                table: "FIASChangeHistory");

            migrationBuilder.AlterColumn<string>(
                name: "RegNumber",
                table: "FIASNormativeDocuments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OrgName",
                table: "FIASNormativeDocuments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "FIASNormativeDocuments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FIASChangeHistory_NormativeDocId",
                table: "FIASChangeHistory",
                column: "NormativeDocId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASChangeHistory_ObjectId_AddressObjectGuid_ChangeId",
                table: "FIASChangeHistory",
                columns: new[] { "ObjectId", "AddressObjectGuid", "ChangeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FIASChangeHistory_FIASNormativeDocuments_NormativeDocId",
                table: "FIASChangeHistory",
                column: "NormativeDocId",
                principalTable: "FIASNormativeDocuments",
                principalColumn: "Id");
        }
    }
}
