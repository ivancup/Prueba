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
        public DbContextUsers() :base("Users")
        {
            Database.SetInitializer<DbContextUsers>(new DropCreateDatabaseIfModelChanges<DbContextUsers>());
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Permission> Permisos { get; set; }
    }
}