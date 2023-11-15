using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class FixObjectRegistryAndChangeHistoryIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FIASChangeHistory_ObjectId_AddressObjectGuid_ChangeId",
                table: "FIASChangeHistory");

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

            migrationBuilder.CreateIndex(
                name: "IX_FIASChangeHistory_ObjectId_AddressObjectGuid_ChangeId",
                table: "FIASChangeHistory",
                columns: new[] { "ObjectId", "AddressObjectGuid", "ChangeId" });
        }
    }
}
