using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class DbOptimizationPart3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FIASObjectsRegistry_ObjectId_ObjectGuid_ChangeId",
                table: "FIASObjectsRegistry");
            
            migrationBuilder.DropPrimaryKey(
                name: "PK_FIASObjectsRegistry",
                table: "FIASObjectsRegistry");
            
            migrationBuilder.DropColumn(
                name: "Id",
                table: "FIASObjectsRegistry");
            
            migrationBuilder.AddColumn<byte[]>(
                name: "Id",
                table: "FIASObjectsRegistry",
                type: "bytea",
                maxLength: 16,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FIASObjectsRegistry",
                table: "FIASObjectsRegistry",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "FIASObjectsRegistry",
                type: "integer",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldMaxLength: 16)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "IX_FIASObjectsRegistry_ObjectId_ObjectGuid_ChangeId",
                table: "FIASObjectsRegistry",
                columns: new[] { "ObjectId", "ObjectGuid", "ChangeId" },
                unique: true);
        }
    }
}
