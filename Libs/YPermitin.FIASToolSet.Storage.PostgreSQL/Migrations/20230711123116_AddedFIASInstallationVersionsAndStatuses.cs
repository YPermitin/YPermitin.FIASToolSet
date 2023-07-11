using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.PostgreSQL.Migrations
{
    public partial class AddedFIASInstallationVersionsAndStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FIASVersionInstallationStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASVersionInstallationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASVersionInstallations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FIASVersionId = table.Column<Guid>(type: "uuid", nullable: false),
                    FIASVersionInstallationStatusId = table.Column<Guid>(type: "uuid", nullable: true),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASVersionInstallations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASVersionInstallations_FIASVersionInstallationStatuses_FI~",
                        column: x => x.FIASVersionInstallationStatusId,
                        principalTable: "FIASVersionInstallationStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FIASVersionInstallations_FIASVersions_FIASVersionId",
                        column: x => x.FIASVersionId,
                        principalTable: "FIASVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FIASVersionInstallationStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("090cc6b8-a5c3-451c-b8fd-e5522ba9ce6a"), "New" },
                    { new Guid("4dba445f-ff47-4071-b9ae-6d3c56d6fe7d"), "Installing" },
                    { new Guid("b0473a78-2743-4f64-b2ea-683b97cc55c5"), "Installed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallations_FIASVersionId",
                table: "FIASVersionInstallations",
                column: "FIASVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallations_FIASVersionInstallationStatusId",
                table: "FIASVersionInstallations",
                column: "FIASVersionInstallationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallations_Id",
                table: "FIASVersionInstallations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallations_StatusId_Created_Id",
                table: "FIASVersionInstallations",
                columns: new[] { "StatusId", "Created", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallationStatuses_Id",
                table: "FIASVersionInstallationStatuses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallationStatuses_Name",
                table: "FIASVersionInstallationStatuses",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FIASVersionInstallations");

            migrationBuilder.DropTable(
                name: "FIASVersionInstallationStatuses");
        }
    }
}
