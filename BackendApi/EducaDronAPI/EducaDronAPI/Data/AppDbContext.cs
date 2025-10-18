using EducaDronAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace EducaDronAPI.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions options) : base(options){}

        public DbSet<Progress> Progress { get; set; }

        public DbSet<LevelPoint> LevelPoint { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Progress>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Progresss)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LevelPoint>()
                .HasOne(lp => lp.Usuario)
                .WithMany(u => u.LevelPoints)
                .HasForeignKey(lp => lp.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
