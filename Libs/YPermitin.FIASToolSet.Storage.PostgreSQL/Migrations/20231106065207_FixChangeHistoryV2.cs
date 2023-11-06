using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class FixChangeHistoryV2 : Migration
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FIASChangeHistory_NormativeDocId",
                table: "FIASChangeHistory",
                column: "NormativeDocId");

            migrationBuilder.AddForeignKey(
                name: "FK_FIASChangeHistory_FIASNormativeDocuments_NormativeDocId",
                table: "FIASChangeHistory",
                column: "NormativeDocId",
                principalTable: "FIASNormativeDocuments",
                principalColumn: "Id");
        }
    }
}
