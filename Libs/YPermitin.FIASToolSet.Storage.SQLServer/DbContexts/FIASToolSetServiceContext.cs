using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YPermitin.FIASToolSet.Storage.Core.Models;

namespace YPermitin.FIASToolSet.Storage.SQLServer.DbContexts
{
    public class FIASToolSetServiceContext : DbContext
    {
        public DbSet<FIASVersion> FIASVersions { get; set; }
        public DbSet<NotificationQueue> NotificationsQueues { get; set; }
        public DbSet<NotificationStatus> NotificationsStatuses { get; set; }

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
        }
    }
}
