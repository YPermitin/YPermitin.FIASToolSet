using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.SQLServer.Migrations
{
    public partial class AddedFIASVersionToNotificationQueue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FIASVersionId",
                table: "NotificationsQueues",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationsQueues_FIASVersionId",
                table: "NotificationsQueues",
                column: "FIASVersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationsQueues_FIASVersions_FIASVersionId",
                table: "NotificationsQueues",
                column: "FIASVersionId",
                principalTable: "FIASVersions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationsQueues_FIASVersions_FIASVersionId",
                table: "NotificationsQueues");

            migrationBuilder.DropIndex(
                name: "IX_NotificationsQueues_FIASVersionId",
                table: "NotificationsQueues");

            migrationBuilder.DropColumn(
                name: "FIASVersionId",
                table: "NotificationsQueues");
        }
    }
}
