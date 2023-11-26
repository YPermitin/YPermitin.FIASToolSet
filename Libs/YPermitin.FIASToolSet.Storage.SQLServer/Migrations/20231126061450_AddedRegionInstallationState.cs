using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.SQLServer.Migrations
{
    /// <inheritdoc />
    public partial class AddedRegionInstallationState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FIASVersionInstallationRegions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FIASVersionInstallationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegionCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASVersionInstallationRegions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASVersionInstallationRegions_FIASVersionInstallationStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "FIASVersionInstallationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FIASVersionInstallationRegions_FIASVersionInstallations_FIASVersionInstallationId",
                        column: x => x.FIASVersionInstallationId,
                        principalTable: "FIASVersionInstallations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallationRegions_FIASVersionInstallationId",
                table: "FIASVersionInstallationRegions",
                column: "FIASVersionInstallationId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallationRegions_RegionCode_FIASVersionInstallationId",
                table: "FIASVersionInstallationRegions",
                columns: new[] { "RegionCode", "FIASVersionInstallationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallationRegions_StatusId",
                table: "FIASVersionInstallationRegions",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FIASVersionInstallationRegions");
        }
    }
}
