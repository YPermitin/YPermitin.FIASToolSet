using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class DbOptimizationPart2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FIASChangeHistory",
                table: "FIASChangeHistory");

            migrationBuilder.AddColumn<byte[]>(
                name: "Id",
                table: "FIASChangeHistory",
                type: "bytea",
                maxLength: 16,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FIASChangeHistory",
                table: "FIASChangeHistory",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FIASChangeHistory",
                table: "FIASChangeHistory");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FIASChangeHistory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FIASChangeHistory",
                table: "FIASChangeHistory",
                columns: new[] { "ChangeId", "ObjectId", "AddressObjectGuid" });
        }
    }
}
