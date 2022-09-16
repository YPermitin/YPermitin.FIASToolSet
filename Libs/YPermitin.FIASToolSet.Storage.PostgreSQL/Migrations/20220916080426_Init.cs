using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.PostgreSQL.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FIASVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Period = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    VersionId = table.Column<int>(type: "integer", nullable: false),
                    TextVersion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FIASDbfComplete = table.Column<string>(type: "text", nullable: true),
                    FIASDbfDelta = table.Column<string>(type: "text", nullable: true),
                    FIASXmlComplete = table.Column<string>(type: "text", nullable: true),
                    FIASXmlDelta = table.Column<string>(type: "text", nullable: true),
                    GARFIASXmlComplete = table.Column<string>(type: "text", nullable: true),
                    GARFIASXmlDelta = table.Column<string>(type: "text", nullable: true),
                    KLADR4ArjComplete = table.Column<string>(type: "text", nullable: true),
                    KLADR47zComplete = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationsStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationsStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationsQueues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Period = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    NotificationTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationsQueues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationsQueues_NotificationsStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "NotificationsStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationsQueues_NotificationType_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "NotificationType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("50be368c-0f06-483a-a5b8-2de9113a4f27"), "New version of FIAS" },
                    { new Guid("749041e9-f51d-48b7-abe0-14ba50436431"), "Custom" }
                });

            migrationBuilder.InsertData(
                table: "NotificationsStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("7d3064ab-45fb-48c0-ac44-a91d1b2369b1"), "Canceled" },
                    { new Guid("f9ae7dcd-f55a-4810-8e96-62e1c0ad1923"), "Sent" },
                    { new Guid("fbb1221b-9a20-4672-b872-730810dbd7d5"), "Added" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersions_Period_Id",
                table: "FIASVersions",
                columns: new[] { "Period", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationsQueues_NotificationTypeId",
                table: "NotificationsQueues",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationsQueues_StatusId_Period_Id",
                table: "NotificationsQueues",
                columns: new[] { "StatusId", "Period", "Id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FIASVersions");

            migrationBuilder.DropTable(
                name: "NotificationsQueues");

            migrationBuilder.DropTable(
                name: "NotificationsStatuses");

            migrationBuilder.DropTable(
                name: "NotificationType");
        }
    }
}
