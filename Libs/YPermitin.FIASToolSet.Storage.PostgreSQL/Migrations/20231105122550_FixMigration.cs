using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class FixMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FIASHouseParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ObjectId = table.Column<int>(type: "integer", nullable: false),
                    ChangeId = table.Column<int>(type: "integer", nullable: false),
                    ChangeIdEnd = table.Column<int>(type: "integer", nullable: false),
                    TypeId = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASHouseParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASHouseParameters_FIASParameterTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "FIASParameterTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FIASHouseParameters_TypeId",
                table: "FIASHouseParameters",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FIASHouseParameters");
        }
    }
}
