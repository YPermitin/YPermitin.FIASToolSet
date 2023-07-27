﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using YPermitin.FIASToolSet.Storage.PostgreSQL.DbContexts;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.PostgreSQL.Migrations
{
    [DbContext(typeof(FIASToolSetServiceContext))]
    [Migration("20230726160330_AddedStartAndFinishDateForInstallation")]
    partial class AddedStartAndFinishDateForInstallation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("YPermitin.FIASToolSet.Storage.Core.Models.AddressObjectType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Level")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("ShortName")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("FIASAddressObjectTypes");
                });

            modelBuilder.Entity("YPermitin.FIASToolSet.Storage.Core.Models.ApartmentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("ShortName")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("FIASApartmentTypes");
                });

            modelBuilder.Entity("YPermitin.FIASToolSet.Storage.Core.Models.FIASVersion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("FIASDbfComplete")
                        .HasColumnType("text");

                    b.Property<string>("FIASDbfDelta")
                        .HasColumnType("text");

                    b.Property<string>("FIASXmlComplete")
                        .HasColumnType("text");

                    b.Property<string>("FIASXmlDelta")
                        .HasColumnType("text");

                    b.Property<string>("GARFIASXmlComplete")
                        .HasColumnType("text");

                    b.Property<string>("GARFIASXmlDelta")
                        .HasColumnType("text");

                    b.Property<string>("KLADR47zComplete")
                        .HasColumnType("text");

                    b.Property<string>("KLADR4ArjComplete")
                        .HasColumnType("text");

                    b.Property<DateTime>("Period")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("TextVersion")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("VersionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Period", "Id");

                    b.ToTable("FIASVersions");
                });

            modelBuilder.Entity("YPermitin.FIASToolSet.Storage.Core.Models.FIASVersionInstallation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("FIASVersionId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("FinishDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("InstallationTypeId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("StatusId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FIASVersionId");

                    b.HasIndex("Id");

                    b.HasIndex("InstallationTypeId");

                    b.HasIndex("StatusId", "Created", "Id");

                    b.ToTable("FIASVersionInstallations");
                });

            modelBuilder.Entity("YPermitin.FIASToolSet.Storage.Core.Models.FIASVersionInstallationStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("Name");

                    b.ToTable("FIASVersionInstallationStatuses");

                    b.HasData(
                        new
                        {
                            Id = new Guid("090cc6b8-a5c3-451c-b8fd-e5522ba9ce6a"),
                            Name = "New"
                        },
                        new
                        {
                            Id = new Guid("4dba445f-ff47-4071-b9ae-6d3c56d6fe7d"),
                            Name = "Installing"
                        },
                        new
                        {
                            Id = new Guid("b0473a78-2743-4f64-b2ea-683b97cc55c5"),
                            Name = "Installed"
                        });
                });

            modelBuilder.Entity("YPermitin.FIASToolSet.Storage.Core.Models.FIASVersionInstallationType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("FIASVersionInstallationsTypes");

                    b.HasData(
                        new
                        {
                            Id = new Guid("e4c31e19-cb2d-47cd-b96e-08a0876ac4f6"),
                            Name = "Full"
                        },
                        new
                        {
                            Id = new Guid("4dba445f-ff47-4071-b9ae-6d3c56d6fe7d"),
                            Name = "Update"
                        });
                });

            modelBuilder.Entity("YPermitin.FIASToolSet.Storage.Core.Models.HouseType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("ShortName")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("FIASHouseTypes");
                });

            modelBuilder.Entity("YPermitin.FIASToolSet.Storage.Core.Models.NormativeDocKind", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("FIASNormativeDocKinds");
                });

            modelBuilder.Entity("YPermitin.FIASToolSet.Storage.Core.Models.NormativeDocType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("FIASNormativeDocTypes");
                });

            modelBuilder.Entity("YPermitin.FIASToolSet.Storage.Core.Models.NotificationQueue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<Guid?>("FIASVersionId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("NotificationTypeId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Period")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("StatusId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FIASVersionId");

                    b.HasIndex("NotificationTypeId");

                    b.HasIndex("StatusId", "Period", "Id");

                    b.ToTable("NotificationsQueues");
                });

            modelBuilder.Entity("YPermitin.FIASToolSet.Storage.Core.Models.NotificationStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("NotificationsStatuses");

                    b.HasData(
                        new
                        {
                            Id = new Guid("fbb1221b-9a20-4672-b872-730810dbd7d5"),
                            Name = "Added"
                        },
                        new
                        {
                            Id = new Guid("f9ae7dcd-f55a-4810-8e96-62e1c0ad1923"),
                            Name = "Sent"
                        },
                        new
                        {
                            Id = new Guid("7d3064ab-45fb-48c0-ac44-a91d1b2369b1"),
                            Name = "Canceled"
                        });
                });

            modelBuilder.Entity("YPermitin.FIASToolSet.Storage.Core.Models.NotificationType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("NotificationType");

                    b.HasData(
                        new
                        {
                            Id = new Guid("50be368c-0f06-483a-a5b8-2de9113a4f27"),
                            Name = "New version of FIAS"
                        },
                        new
                        {
                            Id = new Guid("749041e9-f51d-48b7-abe0-14ba50436431"),
                            Name = "Custom"
                        });
                });

            modelBuilder.Entity("YPermitin.FIASToolSet.Storage.Core.Models.ObjectLevel", b =>
                {
                    b.Property<int>("Level")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Level"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Level");

                    b.HasIndex("Level");

                    b.ToTable("FIASObjectLevels");
                });

            modelBuilder.Entity("YPermitin.FIASToolSet.Storage.Core.Models.OperationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("FIASOperationTypes");
                });

            modelBuilder.Entity("YPermitin.FIASToolSet.Storage.Core.Models.ParameterType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("FIASParameterTypes");
                });

            modelBuilder.Entity("YPermitin.FIASToolSet.Storage.Core.Models.RoomType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("FIASRoomTypes");
                });

            modelBuilder.Entity("YPermitin.FIASToolSet.Storage.Core.Models.FIASVersionInstallation", b =>
                {
                    b.HasOne("YPermitin.FIASToolSet.Storage.Core.Models.FIASVersion", "FIASVersion")
                        .WithMany()
                        .HasForeignKey("FIASVersionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YPermitin.FIASToolSet.Storage.Core.Models.FIASVersionInstallationType", "InstallationType")
                        .WithMany()
                        .HasForeignKey("InstallationTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YPermitin.FIASToolSet.Storage.Core.Models.FIASVersionInstallationStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FIASVersion");

                    b.Navigation("InstallationType");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("YPermitin.FIASToolSet.Storage.Core.Models.NotificationQueue", b =>
                {
                    b.HasOne("YPermitin.FIASToolSet.Storage.Core.Models.FIASVersion", "FIASVersion")
                        .WithMany()
                        .HasForeignKey("FIASVersionId");

                    b.HasOne("YPermitin.FIASToolSet.Storage.Core.Models.NotificationType", "NotificationType")
                        .WithMany()
                        .HasForeignKey("NotificationTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YPermitin.FIASToolSet.Storage.Core.Models.NotificationStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FIASVersion");

                    b.Navigation("NotificationType");

                    b.Navigation("Status");
                });
#pragma warning restore 612, 618
        }
    }
}
