using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.SQLServer.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FIASAddressObjectTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASAddressObjectTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASApartmentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASApartmentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASHouseTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASHouseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASNormativeDocKinds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASNormativeDocKinds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASNormativeDocTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASNormativeDocTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASObjectLevels",
                columns: table => new
                {
                    Level = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASObjectLevels", x => x.Level);
                });

            migrationBuilder.CreateTable(
                name: "FIASOperationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASOperationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASParameterTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASParameterTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASRoomTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASRoomTypes", x => x.Id);
                });

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
                name: "FIASVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Period = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VersionId = table.Column<int>(type: "int", nullable: false),
                    TextVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FIASDbfComplete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FIASDbfDelta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FIASXmlComplete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FIASXmlDelta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GARFIASXmlComplete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GARFIASXmlDelta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KLADR4ArjComplete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KLADR47zComplete = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationsStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationsStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASAddressObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    ObjectGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LevelId = table.Column<int>(type: "int", nullable: false),
                    OperationTypeId = table.Column<int>(type: "int", nullable: false),
                    PreviousAddressObjectId = table.Column<int>(type: "int", nullable: true),
                    NextAddressObjectId = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActual = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASAddressObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASAddressObjects_FIASObjectLevels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "FIASObjectLevels",
                        principalColumn: "Level",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FIASAddressObjects_FIASOperationTypes_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "FIASOperationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FIASVersionInstallations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FIASVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstallationTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinishDate = table.Column<DateTime>(type: "datetime2", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "NotificationsQueues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Period = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FIASVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationsQueues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationsQueues_FIASVersions_FIASVersionId",
                        column: x => x.FIASVersionId,
                        principalTable: "FIASVersions",
                        principalColumn: "Id");
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
                name: "IX_FIASAddressObjects_LevelId",
                table: "FIASAddressObjects",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASAddressObjects_OperationTypeId",
                table: "FIASAddressObjects",
                column: "OperationTypeId");

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
                name: "IX_FIASVersions_Period_Id",
                table: "FIASVersions",
                columns: new[] { "Period", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationsQueues_FIASVersionId",
                table: "NotificationsQueues",
                column: "FIASVersionId");

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
                name: "FIASAddressObjects");

            migrationBuilder.DropTable(
                name: "FIASAddressObjectTypes");

            migrationBuilder.DropTable(
                name: "FIASApartmentTypes");

            migrationBuilder.DropTable(
                name: "FIASHouseTypes");

            migrationBuilder.DropTable(
                name: "FIASNormativeDocKinds");

            migrationBuilder.DropTable(
                name: "FIASNormativeDocTypes");

            migrationBuilder.DropTable(
                name: "FIASParameterTypes");

            migrationBuilder.DropTable(
                name: "FIASRoomTypes");

            migrationBuilder.DropTable(
                name: "FIASVersionInstallations");

            migrationBuilder.DropTable(
                name: "NotificationsQueues");

            migrationBuilder.DropTable(
                name: "FIASObjectLevels");

            migrationBuilder.DropTable(
                name: "FIASOperationTypes");

            migrationBuilder.DropTable(
                name: "FIASVersionInstallationStatuses");

            migrationBuilder.DropTable(
                name: "FIASVersionInstallationsTypes");

            migrationBuilder.DropTable(
                name: "FIASVersions");

            migrationBuilder.DropTable(
                name: "NotificationsStatuses");

            migrationBuilder.DropTable(
                name: "NotificationType");
        }
    }
}
