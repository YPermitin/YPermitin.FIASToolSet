using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YPermitin.FIASToolSet.Storage.Core.Models;
using YPermitin.FIASToolSet.Storage.Core.Models.BaseCatalogs;
using YPermitin.FIASToolSet.Storage.Core.Models.ClassifierData;
using YPermitin.FIASToolSet.Storage.Core.Models.Notifications;
using YPermitin.FIASToolSet.Storage.Core.Models.Versions;

namespace YPermitin.FIASToolSet.Storage.SQLServer.DbContexts
{
    public class FIASToolSetServiceContext : DbContext
    {
        #region Versions

        public DbSet<FIASVersion> FIASVersions { get; set; }
        public DbSet<FIASVersionInstallationStatus> FIASVersionInstallationStatuses { get; set; }
        public DbSet<FIASVersionInstallation> FIASVersionInstallations { get; set; }
        public DbSet<FIASVersionInstallationType> FIASVersionInstallationsTypes { get; set; }

        #endregion
        
        #region Notification
        
        public DbSet<NotificationQueue> NotificationsQueues { get; set; }
        public DbSet<NotificationStatus> NotificationsStatuses { get; set; }

        #endregion
        
        #region BaseCatalogs
        
        public DbSet<AddressObjectType> FIASAddressObjectTypes { get; set; }
        public DbSet<ApartmentType> FIASApartmentTypes { get; set; }
        public DbSet<HouseType> FIASHouseTypes { get; set; }
        public DbSet<NormativeDocKind> FIASNormativeDocKinds { get; set; }
        public DbSet<NormativeDocType> FIASNormativeDocTypes { get; set; }
        public DbSet<ObjectLevel> FIASObjectLevels { get; set; }
        public DbSet<OperationType> FIASOperationTypes { get; set; }
        public DbSet<ParameterType> FIASParameterTypes { get; set; }
        public DbSet<RoomType> FIASRoomTypes { get; set; }
        
        #endregion

        #region ClassifierData

        public DbSet<AddressObject> FIASAddressObjects { get; set; }
        public DbSet<AddressObjectDivision> FIASAddressObjectDivisions { get; set; }
        public DbSet<AddressObjectParameter> FIASAddressObjectParameters { get; set; }

        #endregion

        private FIASToolSetServiceContext()
        {
        }
        public FIASToolSetServiceContext(DbContextOptions<FIASToolSetServiceContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                    .Build();

                string connectionString = configuration.GetConnectionString("FIASToolSetService");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region NotificationStatus

            modelBuilder.Entity<NotificationStatus>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<NotificationStatus>()
                .Property(e => e.Name)
                .HasMaxLength(50);
            modelBuilder.Entity<NotificationStatus>()
                .HasData(new List<NotificationStatus>()
                {
                    new()
                    {
                        Id = NotificationStatus.Added,
                        Name = "Added"
                    },
                    new()
                    {
                        Id = NotificationStatus.Sent,
                        Name = "Sent"
                    },
                    new()
                    {
                        Id = NotificationStatus.Canceled,
                        Name = "Canceled"
                    }
                });

            #endregion

            #region NotificationType

            modelBuilder.Entity<NotificationType>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<NotificationType>()
                .Property(e => e.Name)
                .HasMaxLength(50);
            modelBuilder.Entity<NotificationType>()
                .HasData(new List<NotificationType>()
                {
                    new()
                    {
                        Id = NotificationType.NewVersionOfFIAS,
                        Name = "New version of FIAS"
                    },
                    new()
                    {
                        Id = NotificationType.Custom,
                        Name = "Custom"
                    }
                });

            #endregion

            #region NotificationQueue

            modelBuilder.Entity<NotificationQueue>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<NotificationQueue>()
                .HasIndex(e => new { e.StatusId, e.Period, e.Id });

            #endregion

            #region FIASVersion

            modelBuilder.Entity<FIASVersion>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<FIASVersion>()
                .HasIndex(e => new { e.Period, e.Id });
            modelBuilder.Entity<FIASVersion>()
                .Property(e => e.TextVersion)
                .HasMaxLength(50);

            #endregion

            #region FIASVersionInstallationStatus

            modelBuilder.Entity<FIASVersionInstallationStatus>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<FIASVersionInstallationStatus>()
                .HasIndex(e => new { e.Name });
            modelBuilder.Entity<FIASVersionInstallationStatus>()
                .HasData(new List<FIASVersionInstallationStatus>()
                {
                    new()
                    {
                        Id = FIASVersionInstallationStatus.New,
                        Name = "New"
                    },
                    new()
                    {
                        Id = FIASVersionInstallationStatus.Installing,
                        Name = "Installing"
                    },
                    new()
                    {
                        Id = FIASVersionInstallationStatus.Installed,
                        Name = "Installed"
                    }
                });

            #endregion
            
            #region FIASVersionInstallation
            
            modelBuilder.Entity<FIASVersionInstallation>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<FIASVersionInstallation>()
                .HasIndex(e => new { e.StatusId, e.Created, e.Id });

            #endregion
            
            #region FIASVersionInstallationType
            
            modelBuilder.Entity<FIASVersionInstallationType>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<FIASVersionInstallationType>()
                .HasData(new List<FIASVersionInstallationType>()
                {
                    new()
                    {
                        Id = FIASVersionInstallationType.Full,
                        Name = "Full"
                    },
                    new()
                    {
                        Id = FIASVersionInstallationType.Update,
                        Name = "Update"
                    }
                });

            #endregion
            
            #region AddressObjectType

            modelBuilder.Entity<AddressObjectType>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<AddressObjectType>()
                .Property(e => e.Id)
                .ValueGeneratedNever();
            modelBuilder.Entity<AddressObjectType>()
                .Property(e => e.Name)
                .HasMaxLength(250);
            modelBuilder.Entity<AddressObjectType>()
                .Property(e => e.ShortName)
                .HasMaxLength(250);
            modelBuilder.Entity<AddressObjectType>()
                .Property(e => e.Description)
                .HasMaxLength(500);

            #endregion
            
            #region ApartmentType

            modelBuilder.Entity<ApartmentType>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<ApartmentType>()
                .Property(e => e.Id)
                .ValueGeneratedNever();
            modelBuilder.Entity<ApartmentType>()
                .Property(e => e.Name)
                .HasMaxLength(250);
            modelBuilder.Entity<ApartmentType>()
                .Property(e => e.ShortName)
                .HasMaxLength(250);
            modelBuilder.Entity<ApartmentType>()
                .Property(e => e.Description)
                .HasMaxLength(500);

            #endregion
            
            #region HouseType

            modelBuilder.Entity<HouseType>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<HouseType>()
                .Property(e => e.Id)
                .ValueGeneratedNever();
            modelBuilder.Entity<HouseType>()
                .Property(e => e.Name)
                .HasMaxLength(250);
            modelBuilder.Entity<HouseType>()
                .Property(e => e.ShortName)
                .HasMaxLength(250);
            modelBuilder.Entity<HouseType>()
                .Property(e => e.Description)
                .HasMaxLength(500);

            #endregion
            
            #region NormativeDocKind

            modelBuilder.Entity<NormativeDocKind>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<NormativeDocKind>()
                .Property(e => e.Id)
                .ValueGeneratedNever();
            modelBuilder.Entity<NormativeDocKind>()
                .Property(e => e.Name)
                .HasMaxLength(250);

            #endregion
            
            #region NormativeDocType

            modelBuilder.Entity<NormativeDocType>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<NormativeDocType>()
                .Property(e => e.Id)
                .ValueGeneratedNever();
            modelBuilder.Entity<NormativeDocType>()
                .Property(e => e.Name)
                .HasMaxLength(250);

            #endregion
            
            #region ObjectLevels

            modelBuilder.Entity<ObjectLevel>()
                .HasKey(e => e.Level);
            modelBuilder.Entity<ObjectLevel>()
                .Property(e => e.Level)
                .ValueGeneratedNever();
            modelBuilder.Entity<ObjectLevel>()
                .Property(e => e.Name)
                .HasMaxLength(250);

            #endregion
            
            #region OperationType

            modelBuilder.Entity<OperationType>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<OperationType>()
                .Property(e => e.Id)
                .ValueGeneratedNever();
            modelBuilder.Entity<OperationType>()
                .Property(e => e.Name)
                .HasMaxLength(250);

            #endregion
            
            #region ParameterType

            modelBuilder.Entity<ParameterType>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<ParameterType>()
                .Property(e => e.Id)
                .ValueGeneratedNever();
            modelBuilder.Entity<ParameterType>()
                .Property(e => e.Name)
                .HasMaxLength(250);
            modelBuilder.Entity<ParameterType>()
                .Property(e => e.Code)
                .HasMaxLength(250);
            modelBuilder.Entity<ParameterType>()
                .Property(e => e.Description)
                .HasMaxLength(500);

            #endregion
            
            #region RoomType

            modelBuilder.Entity<RoomType>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<RoomType>()
                .Property(e => e.Id)
                .ValueGeneratedNever();
            modelBuilder.Entity<RoomType>()
                .Property(e => e.Name)
                .HasMaxLength(250);
            modelBuilder.Entity<RoomType>()
                .Property(e => e.Description)
                .HasMaxLength(500);

            #endregion

            #region AddressObject

            modelBuilder.Entity<AddressObject>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<AddressObject>()
                .Property(e => e.Id)
                .ValueGeneratedNever();
            modelBuilder.Entity<AddressObject>()
                .Property(e => e.Name)
                .HasMaxLength(250);
            modelBuilder.Entity<AddressObject>()
                .Property(e => e.TypeName)
                .HasMaxLength(50);

            #endregion
            
            #region AddressObjectDivision

            modelBuilder.Entity<AddressObjectDivision>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<AddressObjectDivision>()
                .Property(e => e.Id)
                .ValueGeneratedNever();

            #endregion
            
            #region AddressObjectParameter

            modelBuilder.Entity<AddressObjectParameter>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<AddressObjectParameter>()
                .Property(e => e.Id)
                .ValueGeneratedNever();

            #endregion
        }
    }
}
