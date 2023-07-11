using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.PostgreSQL.Migrations
{
    public partial class FixFIASInstallationEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FIASVersionInstallations_FIASVersionInstallationStatuses_FI~",
                table: "FIASVersionInstallations");

            migrationBuilder.DropIndex(
                name: "IX_FIASVersionInstallations_FIASVersionInstallationStatusId",
                table: "FIASVersionInstallations");

            migrationBuilder.DropColumn(
                name: "FIASVersionInstallationStatusId",
                table: "FIASVersionInstallations");

            migrationBuilder.AddForeignKey(
                name: "FK_FIASVersionInstallations_FIASVersionInstallationStatuses_St~",
                table: "FIASVersionInstallations",
                column: "StatusId",
                principalTable: "FIASVersionInstallationStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FIASVersionInstallations_FIASVersionInstallationStatuses_St~",
                table: "FIASVersionInstallations");

            migrationBuilder.AddColumn<Guid>(
                name: "FIASVersionInstallationStatusId",
                table: "FIASVersionInstallations",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallations_FIASVersionInstallationStatusId",
                table: "FIASVersionInstallations",
                column: "FIASVersionInstallationStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_FIASVersionInstallations_FIASVersionInstallationStatuses_FI~",
                table: "FIASVersionInstallations",
                column: "FIASVersionInstallationStatusId",
                principalTable: "FIASVersionInstallationStatuses",
                principalColumn: "Id");
        }
    }
}
