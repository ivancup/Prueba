namespace Proyecto.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web;
    using System.Web.Security;

    public class DbContextUsers : DbContext
    {
        public DbContextUsers() : base("Users")
        {
            Database.SetInitializer<DbContextUsers>(new DropCreateDatabaseIfModelChanges<DbContextUsers>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasMany(q => q.Permissions)
                .WithMany(q => q.Roles)
                .Map(q =>
                {
                    q.MapLeftKey("RoleId");
                    q.MapRightKey("PermissionID");
                });
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Permission> Permisos { get; set; }
    }
}