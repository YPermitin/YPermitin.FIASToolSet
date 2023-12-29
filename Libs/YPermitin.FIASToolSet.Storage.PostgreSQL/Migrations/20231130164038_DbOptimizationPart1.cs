using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class DbOptimizationPart1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FIASChangeHistory",
                table: "FIASChangeHistory");

            migrationBuilder.DropIndex(
                name: "IX_FIASChangeHistory_ObjectId_AddressObjectGuid_ChangeId",
                table: "FIASChangeHistory");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FIASChangeHistory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FIASChangeHistory",
                table: "FIASChangeHistory",
                columns: new[] { "ChangeId", "ObjectId", "AddressObjectGuid" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FIASChangeHistory",
                table: "FIASChangeHistory");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "FIASChangeHistory",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FIASChangeHistory",
                table: "FIASChangeHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FIASChangeHistory_ObjectId_AddressObjectGuid_ChangeId",
                table: "FIASChangeHistory",
                columns: new[] { "ObjectId", "AddressObjectGuid", "ChangeId" },
                unique: true);
        }
    }
}
