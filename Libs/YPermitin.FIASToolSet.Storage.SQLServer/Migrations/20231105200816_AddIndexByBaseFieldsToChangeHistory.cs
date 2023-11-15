using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.SQLServer.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexByBaseFieldsToChangeHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FIASChangeHistory_ObjectId_AddressObjectGuid_ChangeId",
                table: "FIASChangeHistory",
                columns: new[] { "ObjectId", "AddressObjectGuid", "ChangeId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FIASChangeHistory_ObjectId_AddressObjectGuid_ChangeId",
                table: "FIASChangeHistory");
        }
    }
}
