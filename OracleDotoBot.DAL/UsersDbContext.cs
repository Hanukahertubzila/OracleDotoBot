using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OracleDotoBot.DAL.Entities;

namespace OracleDotoBot.DAL
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=UserDb;User Id=postgres;Password=ClockTicksBackwards8");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);
        }

        public DbSet<User> Users { get; set; }
    }

    public class BloggingContextFactory : IDesignTimeDbContextFactory<UsersDbContext>
    {
        public UsersDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UsersDbContext>();
            optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=UserDb;User Id=postgres;Password=ClockTicksBackwards8");

            return new UsersDbContext(optionsBuilder.Options);
        }
    }
}