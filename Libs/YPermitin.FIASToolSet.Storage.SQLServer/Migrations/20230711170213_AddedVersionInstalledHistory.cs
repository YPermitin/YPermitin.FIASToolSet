using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.SQLServer.Migrations
{
    public partial class AddedVersionInstalledHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FIASVersionInstallationStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASVersionInstallationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASVersionInstallationsTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASVersionInstallationsTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASVersionInstallations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FIASVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstallationTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASVersionInstallations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASVersionInstallations_FIASVersionInstallationStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "FIASVersionInstallationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FIASVersionInstallations_FIASVersionInstallationsTypes_InstallationTypeId",
                        column: x => x.InstallationTypeId,
                        principalTable: "FIASVersionInstallationsTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.InsertData(
                table: "FIASVersionInstallationsTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("4dba445f-ff47-4071-b9ae-6d3c56d6fe7d"), "Update" },
                    { new Guid("e4c31e19-cb2d-47cd-b96e-08a0876ac4f6"), "Full" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallations_FIASVersionId",
                table: "FIASVersionInstallations",
                column: "FIASVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallations_InstallationTypeId",
                table: "FIASVersionInstallations",
                column: "InstallationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallations_StatusId_Created_Id",
                table: "FIASVersionInstallations",
                columns: new[] { "StatusId", "Created", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallationStatuses_Name",
                table: "FIASVersionInstallationStatuses",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallationsTypes_Id",
                table: "FIASVersionInstallationsTypes",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FIASVersionInstallations");

            migrationBuilder.DropTable(
                name: "FIASVersionInstallationStatuses");

            migrationBuilder.DropTable(
                name: "FIASVersionInstallationsTypes");
        }
    }
}
