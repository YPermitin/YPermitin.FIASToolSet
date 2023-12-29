using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.SQLServer.Migrations
{
    /// <inheritdoc />
    public partial class DbOptimizationPart1 : Migration
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
                type: "varbinary(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: new byte[0]);
            
            migrationBuilder.DropIndex(
                name: "IX_FIASChangeHistory_ObjectId_AddressObjectGuid_ChangeId",
                table: "FIASChangeHistory");
            
            migrationBuilder.DropPrimaryKey(
                name: "PK_FIASChangeHistory",
                table: "FIASChangeHistory");
            
            migrationBuilder.DropColumn(
                name: "Id",
                table: "FIASChangeHistory");
            
            migrationBuilder.AddColumn<byte[]>(
                name: "Id",
                table: "FIASChangeHistory",
                type: "varbinary(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "FIASObjectsRegistry",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(16)",
                oldMaxLength: 16)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "FIASChangeHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(16)",
                oldMaxLength: 16)
                .Annotation("SqlServer:Identity", "1, 1");

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
    }
}
