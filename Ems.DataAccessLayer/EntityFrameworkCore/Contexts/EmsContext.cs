using Ems.Entity.Accounds;
using Ems.Entity.Commons;
using Ems.Entity.Estates;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ems.DataAccessLayer.EntityFrameworkCore.Contexts
{
    public class EmsContext : DbContext
    {
        public EmsContext(DbContextOptions<EmsContext> options) : base(options)
        {
        }

        public DbSet<Estate> Estates { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
