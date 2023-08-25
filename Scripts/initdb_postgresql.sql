-- noinspection SqlNoDataSourceInspectionForFile

\connect fiastoolset_db

CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "FIASVersions" (
    "Id" uuid NOT NULL,
    "Period" timestamp without time zone NOT NULL,
    "VersionId" integer NOT NULL,
    "TextVersion" character varying(50) NULL,
    "Date" timestamp without time zone NOT NULL,
    "FIASDbfComplete" text NULL,
    "FIASDbfDelta" text NULL,
    "FIASXmlComplete" text NULL,
    "FIASXmlDelta" text NULL,
    "GARFIASXmlComplete" text NULL,
    "GARFIASXmlDelta" text NULL,
    "KLADR4ArjComplete" text NULL,
    "KLADR47zComplete" text NULL,
    CONSTRAINT "PK_FIASVersions" PRIMARY KEY ("Id")
);

CREATE TABLE "NotificationsStatuses" (
    "Id" uuid NOT NULL,
    "Name" character varying(50) NULL,
    CONSTRAINT "PK_NotificationsStatuses" PRIMARY KEY ("Id")
);

CREATE TABLE "NotificationType" (
    "Id" uuid NOT NULL,
    "Name" character varying(50) NULL,
    CONSTRAINT "PK_NotificationType" PRIMARY KEY ("Id")
);

CREATE TABLE "NotificationsQueues" (
    "Id" uuid NOT NULL,
    "Period" timestamp without time zone NOT NULL,
    "StatusId" uuid NOT NULL,
    "NotificationTypeId" uuid NOT NULL,
    "Content" text NULL,
    CONSTRAINT "PK_NotificationsQueues" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_NotificationsQueues_NotificationsStatuses_StatusId" FOREIGN KEY ("StatusId") REFERENCES "NotificationsStatuses" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_NotificationsQueues_NotificationType_NotificationTypeId" FOREIGN KEY ("NotificationTypeId") REFERENCES "NotificationType" ("Id") ON DELETE CASCADE
);

INSERT INTO "NotificationType" ("Id", "Name")
VALUES ('50be368c-0f06-483a-a5b8-2de9113a4f27', 'New version of FIAS');
INSERT INTO "NotificationType" ("Id", "Name")
VALUES ('749041e9-f51d-48b7-abe0-14ba50436431', 'Custom');

INSERT INTO "NotificationsStatuses" ("Id", "Name")
VALUES ('7d3064ab-45fb-48c0-ac44-a91d1b2369b1', 'Canceled');
INSERT INTO "NotificationsStatuses" ("Id", "Name")
VALUES ('f9ae7dcd-f55a-4810-8e96-62e1c0ad1923', 'Sent');
INSERT INTO "NotificationsStatuses" ("Id", "Name")
VALUES ('fbb1221b-9a20-4672-b872-730810dbd7d5', 'Added');

CREATE INDEX "IX_FIASVersions_Period_Id" ON "FIASVersions" ("Period", "Id");

CREATE INDEX "IX_NotificationsQueues_NotificationTypeId" ON "NotificationsQueues" ("NotificationTypeId");

CREATE INDEX "IX_NotificationsQueues_StatusId_Period_Id" ON "NotificationsQueues" ("StatusId", "Period", "Id");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220916080426_Init', '6.0.9');

COMMIT;

START TRANSACTION;

ALTER TABLE "NotificationsQueues" ADD "FIASVersionId" uuid NULL;

CREATE INDEX "IX_NotificationsQueues_FIASVersionId" ON "NotificationsQueues" ("FIASVersionId");

ALTER TABLE "NotificationsQueues" ADD CONSTRAINT "FK_NotificationsQueues_FIASVersions_FIASVersionId" FOREIGN KEY ("FIASVersionId") REFERENCES "FIASVersions" ("Id");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220921172442_AddedFIASVersionToNotificationQueue', '6.0.9');

COMMIT;

START TRANSACTION;

CREATE TABLE "FIASVersionInstallationStatuses" (
    "Id" uuid NOT NULL,
    "Name" text NULL,
    CONSTRAINT "PK_FIASVersionInstallationStatuses" PRIMARY KEY ("Id")
);

CREATE TABLE "FIASVersionInstallations" (
    "Id" uuid NOT NULL,
    "Created" timestamp without time zone NOT NULL,
    "FIASVersionId" uuid NOT NULL,
    "FIASVersionInstallationStatusId" uuid NULL,
    "StatusId" uuid NOT NULL,
    CONSTRAINT "PK_FIASVersionInstallations" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_FIASVersionInstallations_FIASVersionInstallationStatuses_FI~" FOREIGN KEY ("FIASVersionInstallationStatusId") REFERENCES "FIASVersionInstallationStatuses" ("Id"),
    CONSTRAINT "FK_FIASVersionInstallations_FIASVersions_FIASVersionId" FOREIGN KEY ("FIASVersionId") REFERENCES "FIASVersions" ("Id") ON DELETE CASCADE
);

INSERT INTO "FIASVersionInstallationStatuses" ("Id", "Name")
VALUES ('090cc6b8-a5c3-451c-b8fd-e5522ba9ce6a', 'New');
INSERT INTO "FIASVersionInstallationStatuses" ("Id", "Name")
VALUES ('4dba445f-ff47-4071-b9ae-6d3c56d6fe7d', 'Installing');
INSERT INTO "FIASVersionInstallationStatuses" ("Id", "Name")
VALUES ('b0473a78-2743-4f64-b2ea-683b97cc55c5', 'Installed');

CREATE INDEX "IX_FIASVersionInstallations_FIASVersionId" ON "FIASVersionInstallations" ("FIASVersionId");

CREATE INDEX "IX_FIASVersionInstallations_FIASVersionInstallationStatusId" ON "FIASVersionInstallations" ("FIASVersionInstallationStatusId");

CREATE INDEX "IX_FIASVersionInstallations_Id" ON "FIASVersionInstallations" ("Id");

CREATE INDEX "IX_FIASVersionInstallations_StatusId_Created_Id" ON "FIASVersionInstallations" ("StatusId", "Created", "Id");

CREATE INDEX "IX_FIASVersionInstallationStatuses_Id" ON "FIASVersionInstallationStatuses" ("Id");

CREATE INDEX "IX_FIASVersionInstallationStatuses_Name" ON "FIASVersionInstallationStatuses" ("Name");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230711123116_AddedFIASInstallationVersionsAndStatuses', '6.0.9');

COMMIT;

START TRANSACTION;

ALTER TABLE "FIASVersionInstallations" ADD "InstallationTypeId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';

CREATE TABLE "FIASVersionInstallationsTypes" (
    "Id" uuid NOT NULL,
    "Name" text NULL,
    CONSTRAINT "PK_FIASVersionInstallationsTypes" PRIMARY KEY ("Id")
);

INSERT INTO "FIASVersionInstallationsTypes" ("Id", "Name")
VALUES ('4dba445f-ff47-4071-b9ae-6d3c56d6fe7d', 'Update');
INSERT INTO "FIASVersionInstallationsTypes" ("Id", "Name")
VALUES ('e4c31e19-cb2d-47cd-b96e-08a0876ac4f6', 'Full');

CREATE INDEX "IX_FIASVersionInstallations_InstallationTypeId" ON "FIASVersionInstallations" ("InstallationTypeId");

CREATE INDEX "IX_FIASVersionInstallationsTypes_Id" ON "FIASVersionInstallationsTypes" ("Id");

ALTER TABLE "FIASVersionInstallations" ADD CONSTRAINT "FK_FIASVersionInstallations_FIASVersionInstallationsTypes_Inst~" FOREIGN KEY ("InstallationTypeId") REFERENCES "FIASVersionInstallationsTypes" ("Id") ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230711132936_AddedInstallationTypes', '6.0.9');

COMMIT;

START TRANSACTION;

ALTER TABLE "FIASVersionInstallations" DROP CONSTRAINT "FK_FIASVersionInstallations_FIASVersionInstallationStatuses_FI~";

DROP INDEX "IX_FIASVersionInstallations_FIASVersionInstallationStatusId";

ALTER TABLE "FIASVersionInstallations" DROP COLUMN "FIASVersionInstallationStatusId";

ALTER TABLE "FIASVersionInstallations" ADD CONSTRAINT "FK_FIASVersionInstallations_FIASVersionInstallationStatuses_St~" FOREIGN KEY ("StatusId") REFERENCES "FIASVersionInstallationStatuses" ("Id") ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230711162417_FixFIASInstallationEntity', '6.0.9');

COMMIT;

START TRANSACTION;

CREATE TABLE "FIASAddressObjectTypes" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Level" integer NOT NULL,
    "Name" character varying(250) NULL,
    "ShortName" character varying(250) NULL,
    "Description" character varying(500) NULL,
    "StartDate" timestamp without time zone NOT NULL,
    "EndDate" timestamp without time zone NOT NULL,
    "UpdateDate" timestamp without time zone NOT NULL,
    CONSTRAINT "PK_FIASAddressObjectTypes" PRIMARY KEY ("Id")
);

CREATE TABLE "FIASApartmentTypes" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" character varying(250) NULL,
    "ShortName" character varying(250) NULL,
    "Description" character varying(500) NULL,
    "StartDate" timestamp without time zone NOT NULL,
    "EndDate" timestamp without time zone NOT NULL,
    "UpdateDate" timestamp without time zone NOT NULL,
    "IsActive" boolean NOT NULL,
    CONSTRAINT "PK_FIASApartmentTypes" PRIMARY KEY ("Id")
);

CREATE TABLE "FIASHouseTypes" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" character varying(250) NULL,
    "ShortName" character varying(250) NULL,
    "Description" character varying(500) NULL,
    "StartDate" timestamp without time zone NOT NULL,
    "EndDate" timestamp without time zone NOT NULL,
    "UpdateDate" timestamp without time zone NOT NULL,
    "IsActive" boolean NOT NULL,
    CONSTRAINT "PK_FIASHouseTypes" PRIMARY KEY ("Id")
);

CREATE TABLE "FIASNormativeDocKinds" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" character varying(250) NULL,
    CONSTRAINT "PK_FIASNormativeDocKinds" PRIMARY KEY ("Id")
);

CREATE TABLE "FIASNormativeDocTypes" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" character varying(250) NULL,
    "StartDate" timestamp without time zone NOT NULL,
    "EndDate" timestamp without time zone NOT NULL,
    CONSTRAINT "PK_FIASNormativeDocTypes" PRIMARY KEY ("Id")
);

CREATE TABLE "FIASObjectLevels" (
    "Level" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" character varying(250) NULL,
    "StartDate" timestamp without time zone NOT NULL,
    "EndDate" timestamp without time zone NOT NULL,
    "UpdateDate" timestamp without time zone NOT NULL,
    "IsActive" boolean NOT NULL,
    CONSTRAINT "PK_FIASObjectLevels" PRIMARY KEY ("Level")
);

CREATE TABLE "FIASOperationTypes" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" character varying(250) NULL,
    "StartDate" timestamp without time zone NOT NULL,
    "EndDate" timestamp without time zone NOT NULL,
    "UpdateDate" timestamp without time zone NOT NULL,
    "IsActive" boolean NOT NULL,
    CONSTRAINT "PK_FIASOperationTypes" PRIMARY KEY ("Id")
);

CREATE TABLE "FIASParameterTypes" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" character varying(250) NULL,
    "Description" character varying(500) NULL,
    "Code" character varying(250) NULL,
    "StartDate" timestamp without time zone NOT NULL,
    "EndDate" timestamp without time zone NOT NULL,
    "UpdateDate" timestamp without time zone NOT NULL,
    "IsActive" boolean NOT NULL,
    CONSTRAINT "PK_FIASParameterTypes" PRIMARY KEY ("Id")
);

CREATE TABLE "FIASRoomTypes" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" character varying(250) NULL,
    "Description" character varying(500) NULL,
    "StartDate" timestamp without time zone NOT NULL,
    "EndDate" timestamp without time zone NOT NULL,
    "UpdateDate" timestamp without time zone NOT NULL,
    "IsActive" boolean NOT NULL,
    CONSTRAINT "PK_FIASRoomTypes" PRIMARY KEY ("Id")
);

CREATE INDEX "IX_FIASAddressObjectTypes_Id" ON "FIASAddressObjectTypes" ("Id");

CREATE INDEX "IX_FIASApartmentTypes_Id" ON "FIASApartmentTypes" ("Id");

CREATE INDEX "IX_FIASHouseTypes_Id" ON "FIASHouseTypes" ("Id");

CREATE INDEX "IX_FIASNormativeDocKinds_Id" ON "FIASNormativeDocKinds" ("Id");

CREATE INDEX "IX_FIASNormativeDocTypes_Id" ON "FIASNormativeDocTypes" ("Id");

CREATE INDEX "IX_FIASObjectLevels_Level" ON "FIASObjectLevels" ("Level");

CREATE INDEX "IX_FIASOperationTypes_Id" ON "FIASOperationTypes" ("Id");

CREATE INDEX "IX_FIASParameterTypes_Id" ON "FIASParameterTypes" ("Id");

CREATE INDEX "IX_FIASRoomTypes_Id" ON "FIASRoomTypes" ("Id");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230718192903_AddedBaseCatalogs', '6.0.9');

COMMIT;

START TRANSACTION;

ALTER TABLE "FIASVersionInstallations" ADD "FinishDate" timestamp without time zone NULL;

ALTER TABLE "FIASVersionInstallations" ADD "StartDate" timestamp without time zone NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230726160330_AddedStartAndFinishDateForInstallation', '6.0.9');

COMMIT;

START TRANSACTION;

DROP INDEX "IX_FIASVersionInstallationsTypes_Id";

DROP INDEX "IX_FIASVersionInstallationStatuses_Id";

DROP INDEX "IX_FIASVersionInstallations_Id";

DROP INDEX "IX_FIASRoomTypes_Id";

DROP INDEX "IX_FIASParameterTypes_Id";

DROP INDEX "IX_FIASOperationTypes_Id";

DROP INDEX "IX_FIASObjectLevels_Level";

DROP INDEX "IX_FIASNormativeDocTypes_Id";

DROP INDEX "IX_FIASNormativeDocKinds_Id";

DROP INDEX "IX_FIASHouseTypes_Id";

DROP INDEX "IX_FIASApartmentTypes_Id";

DROP INDEX "IX_FIASAddressObjectTypes_Id";

CREATE TABLE "AddressObjects" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "ObjectId" integer NOT NULL,
    "ObjectGuid" uuid NOT NULL,
    "ChangeId" integer NOT NULL,
    "Name" character varying(250) NULL,
    "TypeName" character varying(50) NULL,
    "LevelId" integer NOT NULL,
    "OperationTypeId" integer NOT NULL,
    "PreviousAddressObjectId" integer NULL,
    "NextAddressObjectId" integer NULL,
    "UpdateDate" timestamp without time zone NOT NULL,
    "StartDate" timestamp without time zone NOT NULL,
    "EndDate" timestamp without time zone NOT NULL,
    "IsActual" boolean NOT NULL,
    "IsActive" boolean NOT NULL,
    CONSTRAINT "PK_AddressObjects" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AddressObjects_AddressObjects_NextAddressObjectId" FOREIGN KEY ("NextAddressObjectId") REFERENCES "AddressObjects" ("Id"),
    CONSTRAINT "FK_AddressObjects_AddressObjects_PreviousAddressObjectId" FOREIGN KEY ("PreviousAddressObjectId") REFERENCES "AddressObjects" ("Id"),
    CONSTRAINT "FK_AddressObjects_FIASObjectLevels_LevelId" FOREIGN KEY ("LevelId") REFERENCES "FIASObjectLevels" ("Level") ON DELETE CASCADE,
    CONSTRAINT "FK_AddressObjects_FIASOperationTypes_OperationTypeId" FOREIGN KEY ("OperationTypeId") REFERENCES "FIASOperationTypes" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_AddressObjects_LevelId" ON "AddressObjects" ("LevelId");

CREATE INDEX "IX_AddressObjects_NextAddressObjectId" ON "AddressObjects" ("NextAddressObjectId");

CREATE INDEX "IX_AddressObjects_OperationTypeId" ON "AddressObjects" ("OperationTypeId");

CREATE INDEX "IX_AddressObjects_PreviousAddressObjectId" ON "AddressObjects" ("PreviousAddressObjectId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230729154033_AddedAddressObject', '6.0.9');

COMMIT;

START TRANSACTION;

ALTER TABLE "AddressObjects" DROP CONSTRAINT "FK_AddressObjects_AddressObjects_NextAddressObjectId";

ALTER TABLE "AddressObjects" DROP CONSTRAINT "FK_AddressObjects_AddressObjects_PreviousAddressObjectId";

ALTER TABLE "AddressObjects" DROP CONSTRAINT "FK_AddressObjects_FIASObjectLevels_LevelId";

ALTER TABLE "AddressObjects" DROP CONSTRAINT "FK_AddressObjects_FIASOperationTypes_OperationTypeId";

ALTER TABLE "AddressObjects" DROP CONSTRAINT "PK_AddressObjects";

ALTER TABLE "AddressObjects" RENAME TO "FIASAddressObjects";

ALTER INDEX "IX_AddressObjects_PreviousAddressObjectId" RENAME TO "IX_FIASAddressObjects_PreviousAddressObjectId";

ALTER INDEX "IX_AddressObjects_OperationTypeId" RENAME TO "IX_FIASAddressObjects_OperationTypeId";

ALTER INDEX "IX_AddressObjects_NextAddressObjectId" RENAME TO "IX_FIASAddressObjects_NextAddressObjectId";

ALTER INDEX "IX_AddressObjects_LevelId" RENAME TO "IX_FIASAddressObjects_LevelId";

ALTER TABLE "FIASAddressObjects" ADD CONSTRAINT "PK_FIASAddressObjects" PRIMARY KEY ("Id");

ALTER TABLE "FIASAddressObjects" ADD CONSTRAINT "FK_FIASAddressObjects_FIASAddressObjects_NextAddressObjectId" FOREIGN KEY ("NextAddressObjectId") REFERENCES "FIASAddressObjects" ("Id");

ALTER TABLE "FIASAddressObjects" ADD CONSTRAINT "FK_FIASAddressObjects_FIASAddressObjects_PreviousAddressObject~" FOREIGN KEY ("PreviousAddressObjectId") REFERENCES "FIASAddressObjects" ("Id");

ALTER TABLE "FIASAddressObjects" ADD CONSTRAINT "FK_FIASAddressObjects_FIASObjectLevels_LevelId" FOREIGN KEY ("LevelId") REFERENCES "FIASObjectLevels" ("Level") ON DELETE CASCADE;

ALTER TABLE "FIASAddressObjects" ADD CONSTRAINT "FK_FIASAddressObjects_FIASOperationTypes_OperationTypeId" FOREIGN KEY ("OperationTypeId") REFERENCES "FIASOperationTypes" ("Id") ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230730033259_FixTableNaming', '6.0.9');

COMMIT;

START TRANSACTION;

ALTER TABLE "FIASRoomTypes" ALTER COLUMN "Id" 
DROP IDENTITY;

ALTER TABLE "FIASParameterTypes" ALTER COLUMN "Id" 
DROP IDENTITY;

ALTER TABLE "FIASOperationTypes" ALTER COLUMN "Id" 
DROP IDENTITY;

ALTER TABLE "FIASObjectLevels" ALTER COLUMN "Level" 
DROP IDENTITY;

ALTER TABLE "FIASNormativeDocTypes" ALTER COLUMN "Id" 
DROP IDENTITY;

ALTER TABLE "FIASNormativeDocKinds" ALTER COLUMN "Id" 
DROP IDENTITY;

ALTER TABLE "FIASHouseTypes" ALTER COLUMN "Id" 
DROP IDENTITY;

ALTER TABLE "FIASApartmentTypes" ALTER COLUMN "Id" 
DROP IDENTITY;

ALTER TABLE "FIASAddressObjectTypes" ALTER COLUMN "Id" 
DROP IDENTITY;

ALTER TABLE "FIASAddressObjects" ALTER COLUMN "Id" 
DROP IDENTITY;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230806124101_DisableAutoGeneratedValuedForFIASItems', '6.0.9');

COMMIT;

START TRANSACTION;

ALTER TABLE "FIASAddressObjects" DROP CONSTRAINT "FK_FIASAddressObjects_FIASAddressObjects_NextAddressObjectId";

ALTER TABLE "FIASAddressObjects" DROP CONSTRAINT "FK_FIASAddressObjects_FIASAddressObjects_PreviousAddressObject~";

DROP INDEX "IX_FIASAddressObjects_NextAddressObjectId";

DROP INDEX "IX_FIASAddressObjects_PreviousAddressObjectId";

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230806144022_RemoveUnnecessaryRelations', '6.0.9');

COMMIT;

START TRANSACTION;

CREATE TABLE "FIASAddressObjectDivisions" (
    "Id" integer NOT NULL,
    "ParentId" integer NOT NULL,
    "ChildId" integer NOT NULL,
    "ChangeId" integer NOT NULL,
    CONSTRAINT "PK_FIASAddressObjectDivisions" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230812140640_AddedAddressObjectDivisions', '6.0.9');

COMMIT;

