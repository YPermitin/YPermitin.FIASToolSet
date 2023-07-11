using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.PostgreSQL.Migrations
{
    public partial class AddedInstallationTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InstallationTypeId",
                table: "FIASVersionInstallations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "FIASVersionInstallationsTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASVersionInstallationsTypes", x => x.Id);
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
                name: "IX_FIASVersionInstallations_InstallationTypeId",
                table: "FIASVersionInstallations",
                column: "InstallationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallationsTypes_Id",
                table: "FIASVersionInstallationsTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FIASVersionInstallations_FIASVersionInstallationsTypes_Inst~",
                table: "FIASVersionInstallations",
                column: "InstallationTypeId",
                principalTable: "FIASVersionInstallationsTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FIASVersionInstallations_FIASVersionInstallationsTypes_Inst~",
                table: "FIASVersionInstallations");

            migrationBuilder.DropTable(
                name: "FIASVersionInstallationsTypes");

            migrationBuilder.DropIndex(
                name: "IX_FIASVersionInstallations_InstallationTypeId",
                table: "FIASVersionInstallations");

            migrationBuilder.DropColumn(
                name: "InstallationTypeId",
                table: "FIASVersionInstallations");
        }
    }
}
