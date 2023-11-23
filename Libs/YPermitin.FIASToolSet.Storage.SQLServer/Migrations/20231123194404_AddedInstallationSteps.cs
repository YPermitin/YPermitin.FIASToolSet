using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.SQLServer.Migrations
{
    /// <inheritdoc />
    public partial class AddedInstallationSteps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FIASVersionInstallationSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FIASVersionInstallationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileFullName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalItemsToLoad = table.Column<long>(type: "bigint", nullable: false),
                    TotalItemsLoaded = table.Column<long>(type: "bigint", nullable: false),
                    PercentageCompleted = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASVersionInstallationSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASVersionInstallationSteps_FIASVersionInstallationStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "FIASVersionInstallationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FIASVersionInstallationSteps_FIASVersionInstallations_FIASVersionInstallationId",
                        column: x => x.FIASVersionInstallationId,
                        principalTable: "FIASVersionInstallations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallationSteps_FIASVersionInstallationId",
                table: "FIASVersionInstallationSteps",
                column: "FIASVersionInstallationId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallationSteps_StatusId",
                table: "FIASVersionInstallationSteps",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FIASVersionInstallationSteps");
        }
    }
}
