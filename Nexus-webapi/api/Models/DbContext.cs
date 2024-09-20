using Microsoft.EntityFrameworkCore;

namespace Nexus_webapi.Models
{
    public class NexusDbContext : DbContext
    {
        public NexusDbContext(DbContextOptions<NexusDbContext> options)
            : base(options)
        {
        }

        public DbSet<EmployeeRoomAccess> EmployeeRoomAccesses { get; set; } = null!;
        public DbSet<RolePermissions> RolePermissions { get; set; } = null!;
        public DbSet<Roles> Roles { get; set; } = null!;
        public DbSet<Permissions> Permissions { get; set; } = null!;
        public DbSet<AccessLogs> AccessLogs { get; set; } = null!;
        public DbSet<Employees> Employees { get; set; } = null!;
        public DbSet<Rooms> Rooms { get; set; } = null!;
        public DbSet<EmployeeRoles> EmployeeRoles { get; set; } = null!;
        public DbSet<UserToken> UserTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships with cascade delete
            ConfigureRelationships(modelBuilder);

            // Configure default values
            ConfigureDefaultValues(modelBuilder);

            // Configure max lengths for string properties
            ConfigureMaxLengths(modelBuilder);

            // Configure many-to-many relationship between Roles and Permissions
            modelBuilder.Entity<RolePermissions>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermissions>()
                .HasOne(rp => rp.Permission)
                .WithMany()
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserToken>()
                .HasOne(ut => ut.Employee)
                .WithMany()
                .HasForeignKey(ut => ut.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Permissions>()
                .HasKey(p => p.PermissionId);

            // Add indexes for performance
            modelBuilder.Entity<UserToken>()
                .HasIndex(ut => ut.Token)
                .IsUnique();

            modelBuilder.Entity<UserToken>()
                .HasIndex(ut => ut.Expiration);
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            var cascadeDeleteEntities = new[]
            {
                (typeof(AccessLogs), new[] { "Employee", "Room" }),
                (typeof(EmployeeRoles), new[] { "Employee", "Role" }),
                (typeof(EmployeeRoomAccess), new[] { "Employee", "Room" })
            };

            foreach (var (entityType, navigationProperties) in cascadeDeleteEntities)
            {
                var entity = modelBuilder.Entity(entityType);
                foreach (var navProperty in navigationProperties)
                {
                    entity
                        .HasOne(navProperty)
                        .WithMany()
                        .HasForeignKey($"{navProperty}Id")
                        .OnDelete(DeleteBehavior.Cascade);
                }
            }
        }

        private void ConfigureDefaultValues(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessLogs>()
                .Property(al => al.AccessTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }

        private void ConfigureMaxLengths(ModelBuilder modelBuilder)
        {
            var maxLengthConfigurations = new[]
            {
                (typeof(Employees), new[] { ("Name", 100), ("PinCode", 10) }),
                (typeof(Permissions), new[] { ("PermissionKey", 50) }),
                (typeof(Rooms), new[] { ("RoomName", 100) }),
                (typeof(Roles), new[] { ("RoleName", 50) }),
                (typeof(UserToken), new[] { ("Token", 100) })
            };

            foreach (var (entityType, properties) in maxLengthConfigurations)
            {
                var entity = modelBuilder.Entity(entityType);
                foreach (var (propertyName, maxLength) in properties)
                {
                    entity.Property(propertyName).HasMaxLength(maxLength);
                }
            }
        }
    }
}