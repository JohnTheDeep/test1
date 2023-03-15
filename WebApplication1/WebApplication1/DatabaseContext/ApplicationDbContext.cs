using Microsoft.EntityFrameworkCore;
using WebApplication1.Interfaces;
using WebApplication1.Models.Entites;

namespace WebApplication1.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IApplicationConfiguration _configuration;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IApplicationConfiguration configuration) : base(options)
        {
            _configuration = configuration;
            Database.EnsureCreated();
            SeedRankTable().Wait();
        }
        public DbSet<Employee> employees { get; set; }
        public DbSet<Rank> ranks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_configuration.DEFAULT_CONNECTION_STRING, ServerVersion.AutoDetect(_configuration.DEFAULT_CONNECTION_STRING));
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Employee>()
                .HasIndex(u => u.FullName)
                .IsUnique();
        }
        public async Task SeedRankTable()
        {
            if (!this.ranks.Any())
            {
                this.ranks.AddRange(new[] { new Rank("Инженер"), new Rank("Профессиональный курильщик") });
                await this.SaveChangesAsync();
            }
        }
    }
}
