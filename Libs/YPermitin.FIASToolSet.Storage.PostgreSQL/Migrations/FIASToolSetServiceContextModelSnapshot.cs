﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using YPermitin.FIASToolSet.Storage.PostgreSQL.DbContexts;

#nullable disable

namespace YPermitin.FIASToolSet.Storage.PostgreSQL.Migrations
{
    [DbContext(typeof(FIASToolSetServiceContext))]
    partial class FIASToolSetServiceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

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

            modelBuilder.Entity("YPermitin.FIASToolSet.Storage.Core.Models.NotificationQueue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<Guid>("NotificationTypeId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Period")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("StatusId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

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

            modelBuilder.Entity("YPermitin.FIASToolSet.Storage.Core.Models.NotificationQueue", b =>
                {
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

                    b.Navigation("NotificationType");

                    b.Navigation("Status");
                });
#pragma warning restore 612, 618
        }
    }
}