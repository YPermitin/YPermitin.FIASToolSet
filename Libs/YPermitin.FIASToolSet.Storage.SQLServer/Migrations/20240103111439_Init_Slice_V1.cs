using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YPermitin.FIASToolSet.Storage.SQLServer.Migrations
{
    /// <inheritdoc />
    public partial class Init_Slice_V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddressObjectAdmHierarchy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    ParentObjectId = table.Column<int>(type: "int", nullable: false),
                    ChangeId = table.Column<int>(type: "int", nullable: false),
                    RegionCode = table.Column<int>(type: "int", nullable: true),
                    AreaCode = table.Column<int>(type: "int", nullable: true),
                    CityCode = table.Column<int>(type: "int", nullable: true),
                    PlaceCode = table.Column<int>(type: "int", nullable: true),
                    PlanCode = table.Column<int>(type: "int", nullable: true),
                    StreetCode = table.Column<int>(type: "int", nullable: true),
                    PreviousAddressObjectId = table.Column<int>(type: "int", nullable: true),
                    NextAddressObjectId = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressObjectAdmHierarchy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AddressObjectDivision",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    ChildId = table.Column<int>(type: "int", nullable: false),
                    ChangeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressObjectDivision", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AddressObjectType",
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
                    table.PrimaryKey("PK_AddressObjectType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentType",
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
                    table.PrimaryKey("PK_ApartmentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASVersion",
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
                    table.PrimaryKey("PK_FIASVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASVersionInstallationStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASVersionInstallationStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASVersionInstallationType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASVersionInstallationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HouseType",
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
                    table.PrimaryKey("PK_HouseType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MunHierarchy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    ParentObjectId = table.Column<int>(type: "int", nullable: true),
                    ChangeId = table.Column<int>(type: "int", nullable: false),
                    OKTMO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviousAddressObjectId = table.Column<int>(type: "int", nullable: true),
                    NextAddressObjectId = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunHierarchy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NormativeDocKind",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NormativeDocKind", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NormativeDocType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NormativeDocType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationStatus", x => x.Id);
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
                name: "ObjectLevel",
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
                    table.PrimaryKey("PK_ObjectLevel", x => x.Level);
                });

            migrationBuilder.CreateTable(
                name: "OperationType",
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
                    table.PrimaryKey("PK_OperationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParameterType",
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
                    table.PrimaryKey("PK_ParameterType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomType",
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
                    table.PrimaryKey("PK_RoomType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASVersionInstallation",
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
                    table.PrimaryKey("PK_FIASVersionInstallation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASVersionInstallation_FIASVersionInstallationStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "FIASVersionInstallationStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FIASVersionInstallation_FIASVersionInstallationType_InstallationTypeId",
                        column: x => x.InstallationTypeId,
                        principalTable: "FIASVersionInstallationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FIASVersionInstallation_FIASVersion_FIASVersionId",
                        column: x => x.FIASVersionId,
                        principalTable: "FIASVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NormativeDocument",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    KindId = table.Column<int>(type: "int", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrgName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RegNumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    RegDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NormativeDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NormativeDocument_NormativeDocKind_KindId",
                        column: x => x.KindId,
                        principalTable: "NormativeDocKind",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NormativeDocument_NormativeDocType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "NormativeDocType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationQueue",
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
                    table.PrimaryKey("PK_NotificationQueue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationQueue_FIASVersion_FIASVersionId",
                        column: x => x.FIASVersionId,
                        principalTable: "FIASVersion",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NotificationQueue_NotificationStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "NotificationStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationQueue_NotificationType_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ObjectRegistry",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", maxLength: 16, nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    ObjectGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangeId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LevelId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectRegistry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectRegistry_ObjectLevel_LevelId",
                        column: x => x.LevelId,
                        principalTable: "ObjectLevel",
                        principalColumn: "Level",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AddressObject",
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
                    table.PrimaryKey("PK_AddressObject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddressObject_ObjectLevel_LevelId",
                        column: x => x.LevelId,
                        principalTable: "ObjectLevel",
                        principalColumn: "Level",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AddressObject_OperationType_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "OperationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Apartment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    ObjectGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangeId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApartmentTypeId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Apartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Apartment_ApartmentType_ApartmentTypeId",
                        column: x => x.ApartmentTypeId,
                        principalTable: "ApartmentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Apartment_OperationType_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "OperationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CarPlace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    ObjectGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangeId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_CarPlace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarPlace_OperationType_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "OperationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChangeHistory",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", maxLength: 16, nullable: false),
                    ChangeId = table.Column<int>(type: "int", nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    AddressObjectGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperationTypeId = table.Column<int>(type: "int", nullable: false),
                    NormativeDocId = table.Column<int>(type: "int", nullable: true),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeHistory_OperationType_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "OperationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "House",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    ObjectGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangeId = table.Column<int>(type: "int", nullable: false),
                    HouseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AddedHouseNumber1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AddedHouseNumber2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HouseTypeId = table.Column<int>(type: "int", nullable: true),
                    AddedHouseTypeId1 = table.Column<int>(type: "int", nullable: true),
                    AddedHouseTypeId2 = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_House", x => x.Id);
                    table.ForeignKey(
                        name: "FK_House_HouseType_AddedHouseTypeId1",
                        column: x => x.AddedHouseTypeId1,
                        principalTable: "HouseType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_House_HouseType_AddedHouseTypeId2",
                        column: x => x.AddedHouseTypeId2,
                        principalTable: "HouseType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_House_HouseType_HouseTypeId",
                        column: x => x.HouseTypeId,
                        principalTable: "HouseType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_House_OperationType_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "OperationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stead",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    ObjectGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangeId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
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
                    table.PrimaryKey("PK_Stead", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stead_OperationType_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "OperationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AddressObjectParameter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    ChangeId = table.Column<int>(type: "int", nullable: false),
                    ChangeIdEnd = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressObjectParameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddressObjectParameter_ParameterType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ParameterType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentParameter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    ChangeId = table.Column<int>(type: "int", nullable: false),
                    ChangeIdEnd = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentParameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApartmentParameter_ParameterType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ParameterType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CarPlaceParameter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    ChangeId = table.Column<int>(type: "int", nullable: false),
                    ChangeIdEnd = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarPlaceParameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarPlaceParameter_ParameterType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ParameterType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HouseParameter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    ChangeId = table.Column<int>(type: "int", nullable: false),
                    ChangeIdEnd = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseParameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HouseParameter_ParameterType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ParameterType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoomParameter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    ChangeId = table.Column<int>(type: "int", nullable: false),
                    ChangeIdEnd = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomParameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomParameter_ParameterType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ParameterType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SteadParameter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    ChangeId = table.Column<int>(type: "int", nullable: false),
                    ChangeIdEnd = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SteadParameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SteadParameter_ParameterType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ParameterType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    ObjectGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangeId = table.Column<int>(type: "int", nullable: false),
                    RoomNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RoomTypeId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Room", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Room_OperationType_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "OperationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Room_RoomType_RoomTypeId",
                        column: x => x.RoomTypeId,
                        principalTable: "RoomType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FIASVersionInstallationRegion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FIASVersionInstallationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegionCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASVersionInstallationRegion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASVersionInstallationRegion_FIASVersionInstallationStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "FIASVersionInstallationStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FIASVersionInstallationRegion_FIASVersionInstallation_FIASVersionInstallationId",
                        column: x => x.FIASVersionInstallationId,
                        principalTable: "FIASVersionInstallation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FIASVersionInstallationStep",
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
                    table.PrimaryKey("PK_FIASVersionInstallationStep", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASVersionInstallationStep_FIASVersionInstallationStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "FIASVersionInstallationStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FIASVersionInstallationStep_FIASVersionInstallation_FIASVersionInstallationId",
                        column: x => x.FIASVersionInstallationId,
                        principalTable: "FIASVersionInstallation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "FIASVersionInstallationStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("090cc6b8-a5c3-451c-b8fd-e5522ba9ce6a"), "New" },
                    { new Guid("4dba445f-ff47-4071-b9ae-6d3c56d6fe7d"), "Installing" },
                    { new Guid("b0473a78-2743-4f64-b2ea-683b97cc55c5"), "Installed" }
                });

            migrationBuilder.InsertData(
                table: "FIASVersionInstallationType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("4dba445f-ff47-4071-b9ae-6d3c56d6fe7d"), "Update" },
                    { new Guid("e4c31e19-cb2d-47cd-b96e-08a0876ac4f6"), "Full" }
                });

            migrationBuilder.InsertData(
                table: "NotificationStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("7d3064ab-45fb-48c0-ac44-a91d1b2369b1"), "Canceled" },
                    { new Guid("f9ae7dcd-f55a-4810-8e96-62e1c0ad1923"), "Sent" },
                    { new Guid("fbb1221b-9a20-4672-b872-730810dbd7d5"), "Added" }
                });

            migrationBuilder.InsertData(
                table: "NotificationType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("50be368c-0f06-483a-a5b8-2de9113a4f27"), "New version of FIAS" },
                    { new Guid("749041e9-f51d-48b7-abe0-14ba50436431"), "Custom" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddressObject_LevelId",
                table: "AddressObject",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_AddressObject_OperationTypeId",
                table: "AddressObject",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AddressObjectParameter_TypeId",
                table: "AddressObjectParameter",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Apartment_ApartmentTypeId",
                table: "Apartment",
                column: "ApartmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Apartment_OperationTypeId",
                table: "Apartment",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentParameter_TypeId",
                table: "ApartmentParameter",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CarPlace_OperationTypeId",
                table: "CarPlace",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CarPlaceParameter_TypeId",
                table: "CarPlaceParameter",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeHistory_OperationTypeId",
                table: "ChangeHistory",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersion_Period_Id",
                table: "FIASVersion",
                columns: new[] { "Period", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallation_FIASVersionId",
                table: "FIASVersionInstallation",
                column: "FIASVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallation_InstallationTypeId",
                table: "FIASVersionInstallation",
                column: "InstallationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallation_StatusId_Created_Id",
                table: "FIASVersionInstallation",
                columns: new[] { "StatusId", "Created", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallationRegion_FIASVersionInstallationId",
                table: "FIASVersionInstallationRegion",
                column: "FIASVersionInstallationId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallationRegion_RegionCode_FIASVersionInstallationId",
                table: "FIASVersionInstallationRegion",
                columns: new[] { "RegionCode", "FIASVersionInstallationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallationRegion_StatusId",
                table: "FIASVersionInstallationRegion",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallationStatus_Name",
                table: "FIASVersionInstallationStatus",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallationStep_FIASVersionInstallationId",
                table: "FIASVersionInstallationStep",
                column: "FIASVersionInstallationId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASVersionInstallationStep_StatusId",
                table: "FIASVersionInstallationStep",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_House_AddedHouseTypeId1",
                table: "House",
                column: "AddedHouseTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_House_AddedHouseTypeId2",
                table: "House",
                column: "AddedHouseTypeId2");

            migrationBuilder.CreateIndex(
                name: "IX_House_HouseTypeId",
                table: "House",
                column: "HouseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_House_OperationTypeId",
                table: "House",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseParameter_TypeId",
                table: "HouseParameter",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NormativeDocument_KindId",
                table: "NormativeDocument",
                column: "KindId");

            migrationBuilder.CreateIndex(
                name: "IX_NormativeDocument_TypeId",
                table: "NormativeDocument",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationQueue_FIASVersionId",
                table: "NotificationQueue",
                column: "FIASVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationQueue_NotificationTypeId",
                table: "NotificationQueue",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationQueue_StatusId_Period_Id",
                table: "NotificationQueue",
                columns: new[] { "StatusId", "Period", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_ObjectRegistry_LevelId",
                table: "ObjectRegistry",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_OperationTypeId",
                table: "Room",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_RoomTypeId",
                table: "Room",
                column: "RoomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomParameter_TypeId",
                table: "RoomParameter",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Stead_OperationTypeId",
                table: "Stead",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SteadParameter_TypeId",
                table: "SteadParameter",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressObject");

            migrationBuilder.DropTable(
                name: "AddressObjectAdmHierarchy");

            migrationBuilder.DropTable(
                name: "AddressObjectDivision");

            migrationBuilder.DropTable(
                name: "AddressObjectParameter");

            migrationBuilder.DropTable(
                name: "AddressObjectType");

            migrationBuilder.DropTable(
                name: "Apartment");

            migrationBuilder.DropTable(
                name: "ApartmentParameter");

            migrationBuilder.DropTable(
                name: "CarPlace");

            migrationBuilder.DropTable(
                name: "CarPlaceParameter");

            migrationBuilder.DropTable(
                name: "ChangeHistory");

            migrationBuilder.DropTable(
                name: "FIASVersionInstallationRegion");

            migrationBuilder.DropTable(
                name: "FIASVersionInstallationStep");

            migrationBuilder.DropTable(
                name: "House");

            migrationBuilder.DropTable(
                name: "HouseParameter");

            migrationBuilder.DropTable(
                name: "MunHierarchy");

            migrationBuilder.DropTable(
                name: "NormativeDocument");

            migrationBuilder.DropTable(
                name: "NotificationQueue");

            migrationBuilder.DropTable(
                name: "ObjectRegistry");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "RoomParameter");

            migrationBuilder.DropTable(
                name: "Stead");

            migrationBuilder.DropTable(
                name: "SteadParameter");

            migrationBuilder.DropTable(
                name: "ApartmentType");

            migrationBuilder.DropTable(
                name: "FIASVersionInstallation");

            migrationBuilder.DropTable(
                name: "HouseType");

            migrationBuilder.DropTable(
                name: "NormativeDocKind");

            migrationBuilder.DropTable(
                name: "NormativeDocType");

            migrationBuilder.DropTable(
                name: "NotificationStatus");

            migrationBuilder.DropTable(
                name: "NotificationType");

            migrationBuilder.DropTable(
                name: "ObjectLevel");

            migrationBuilder.DropTable(
                name: "RoomType");

            migrationBuilder.DropTable(
                name: "OperationType");

            migrationBuilder.DropTable(
                name: "ParameterType");

            migrationBuilder.DropTable(
                name: "FIASVersionInstallationStatus");

            migrationBuilder.DropTable(
                name: "FIASVersionInstallationType");

            migrationBuilder.DropTable(
                name: "FIASVersion");
        }
    }
}
