using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.SQLServer.Migrations
{
    public partial class AddedAllFiasEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FIASAddressObjectsAdmHierarchy",
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
                    table.PrimaryKey("PK_FIASAddressObjectsAdmHierarchy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASApartmentParameters",
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
                    table.PrimaryKey("PK_FIASApartmentParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASApartmentParameters_FIASParameterTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "FIASParameterTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FIASApartments",
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
                    table.PrimaryKey("PK_FIASApartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASApartments_FIASApartmentTypes_ApartmentTypeId",
                        column: x => x.ApartmentTypeId,
                        principalTable: "FIASApartmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FIASApartments_FIASOperationTypes_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "FIASOperationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FIASCarPlaceParameters",
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
                    table.PrimaryKey("PK_FIASCarPlaceParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASCarPlaceParameters_FIASParameterTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "FIASParameterTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FIASCarPlaces",
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
                    table.PrimaryKey("PK_FIASCarPlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASCarPlaces_FIASOperationTypes_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "FIASOperationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FIASHouseParameters",
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
                    table.PrimaryKey("PK_FIASHouseParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASHouseParameters_FIASParameterTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "FIASParameterTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FIASHouses",
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
                    table.PrimaryKey("PK_FIASHouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASHouses_FIASHouseTypes_AddedHouseTypeId1",
                        column: x => x.AddedHouseTypeId1,
                        principalTable: "FIASHouseTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FIASHouses_FIASHouseTypes_AddedHouseTypeId2",
                        column: x => x.AddedHouseTypeId2,
                        principalTable: "FIASHouseTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FIASHouses_FIASHouseTypes_HouseTypeId",
                        column: x => x.HouseTypeId,
                        principalTable: "FIASHouseTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FIASHouses_FIASOperationTypes_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "FIASOperationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FIASMunHierarchy",
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
                    table.PrimaryKey("PK_FIASMunHierarchy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIASNormativeDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    KindId = table.Column<int>(type: "int", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrgName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RegNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RegDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASNormativeDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASNormativeDocuments_FIASNormativeDocKinds_KindId",
                        column: x => x.KindId,
                        principalTable: "FIASNormativeDocKinds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FIASNormativeDocuments_FIASNormativeDocTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "FIASNormativeDocTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FIASObjectsRegistry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_FIASObjectsRegistry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASObjectsRegistry_FIASObjectLevels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "FIASObjectLevels",
                        principalColumn: "Level",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FIASRoomParameters",
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
                    table.PrimaryKey("PK_FIASRoomParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASRoomParameters_FIASParameterTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "FIASParameterTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FIASRooms",
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
                    table.PrimaryKey("PK_FIASRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASRooms_FIASOperationTypes_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "FIASOperationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FIASRooms_FIASRoomTypes_RoomTypeId",
                        column: x => x.RoomTypeId,
                        principalTable: "FIASRoomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FIASSteadParameters",
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
                    table.PrimaryKey("PK_FIASSteadParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASSteadParameters_FIASParameterTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "FIASParameterTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FIASSteads",
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
                    table.PrimaryKey("PK_FIASSteads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASSteads_FIASOperationTypes_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "FIASOperationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FIASChangeHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangeId = table.Column<int>(type: "int", nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    AddressObjectGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperationTypeId = table.Column<int>(type: "int", nullable: false),
                    NormativeDocId = table.Column<int>(type: "int", nullable: true),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIASChangeHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIASChangeHistory_FIASNormativeDocuments_NormativeDocId",
                        column: x => x.NormativeDocId,
                        principalTable: "FIASNormativeDocuments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FIASChangeHistory_FIASOperationTypes_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "FIASOperationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FIASAddressObjectParameters_TypeId",
                table: "FIASAddressObjectParameters",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASApartmentParameters_TypeId",
                table: "FIASApartmentParameters",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASApartments_ApartmentTypeId",
                table: "FIASApartments",
                column: "ApartmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASApartments_OperationTypeId",
                table: "FIASApartments",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASCarPlaceParameters_TypeId",
                table: "FIASCarPlaceParameters",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASCarPlaces_OperationTypeId",
                table: "FIASCarPlaces",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASChangeHistory_NormativeDocId",
                table: "FIASChangeHistory",
                column: "NormativeDocId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASChangeHistory_OperationTypeId",
                table: "FIASChangeHistory",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASHouseParameters_TypeId",
                table: "FIASHouseParameters",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASHouses_AddedHouseTypeId1",
                table: "FIASHouses",
                column: "AddedHouseTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_FIASHouses_AddedHouseTypeId2",
                table: "FIASHouses",
                column: "AddedHouseTypeId2");

            migrationBuilder.CreateIndex(
                name: "IX_FIASHouses_HouseTypeId",
                table: "FIASHouses",
                column: "HouseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASHouses_OperationTypeId",
                table: "FIASHouses",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASNormativeDocuments_KindId",
                table: "FIASNormativeDocuments",
                column: "KindId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASNormativeDocuments_TypeId",
                table: "FIASNormativeDocuments",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASObjectsRegistry_LevelId",
                table: "FIASObjectsRegistry",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASRoomParameters_TypeId",
                table: "FIASRoomParameters",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASRooms_OperationTypeId",
                table: "FIASRooms",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASRooms_RoomTypeId",
                table: "FIASRooms",
                column: "RoomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASSteadParameters_TypeId",
                table: "FIASSteadParameters",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FIASSteads_OperationTypeId",
                table: "FIASSteads",
                column: "OperationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FIASAddressObjectParameters_FIASParameterTypes_TypeId",
                table: "FIASAddressObjectParameters",
                column: "TypeId",
                principalTable: "FIASParameterTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FIASAddressObjectParameters_FIASParameterTypes_TypeId",
                table: "FIASAddressObjectParameters");

            migrationBuilder.DropTable(
                name: "FIASAddressObjectsAdmHierarchy");

            migrationBuilder.DropTable(
                name: "FIASApartmentParameters");

            migrationBuilder.DropTable(
                name: "FIASApartments");

            migrationBuilder.DropTable(
                name: "FIASCarPlaceParameters");

            migrationBuilder.DropTable(
                name: "FIASCarPlaces");

            migrationBuilder.DropTable(
                name: "FIASChangeHistory");

            migrationBuilder.DropTable(
                name: "FIASHouseParameters");

            migrationBuilder.DropTable(
                name: "FIASHouses");

            migrationBuilder.DropTable(
                name: "FIASMunHierarchy");

            migrationBuilder.DropTable(
                name: "FIASObjectsRegistry");

            migrationBuilder.DropTable(
                name: "FIASRoomParameters");

            migrationBuilder.DropTable(
                name: "FIASRooms");

            migrationBuilder.DropTable(
                name: "FIASSteadParameters");

            migrationBuilder.DropTable(
                name: "FIASSteads");

            migrationBuilder.DropTable(
                name: "FIASNormativeDocuments");

            migrationBuilder.DropIndex(
                name: "IX_FIASAddressObjectParameters_TypeId",
                table: "FIASAddressObjectParameters");
        }
    }
}
