using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task_Management.Domain.Entities;

namespace Task_Management.Infrastructure.Persistence.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder Builder)
        {
            base.OnModelCreating(Builder);

            Builder.Entity<ApplicationUser>().ToTable("Users");
            Builder.Entity<IdentityRole>().ToTable("Roles");
            Builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            Builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            Builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            Builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            Builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        }
    }
}
