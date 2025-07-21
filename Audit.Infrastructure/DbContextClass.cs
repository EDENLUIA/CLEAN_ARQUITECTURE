using Microsoft.EntityFrameworkCore;
using Audit.Core;

namespace Audit.Infrastructure
{
    public class DbContextClass : DbContext
    {
        public DbContextClass(DbContextOptions<DbContextClass> contextOptions) : base(contextOptions)
        {

        }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<PermissionType> PermissionTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Permission>().ToTable("Permissions");

            modelBuilder.Entity<Permission>()
            .HasKey(i => new { i.Id });

            modelBuilder.Entity<PermissionType>().ToTable("PermissionTypes");

            modelBuilder.Entity<PermissionType>()
            .HasKey(i => new { i.Id });

            base.OnModelCreating(modelBuilder);
        }


    }
}
