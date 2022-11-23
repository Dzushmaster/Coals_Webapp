using Microsoft.EntityFrameworkCore;

namespace Coals_WebApp.Models
{
    public class AppDbContext : DbContext
    {
        private static AppDbContext? dbContext;
        public DbSet<Users> users { get; set; }
        public DbSet<Articles> articles { get; set; }
        public DbSet<Comments> comments { get; set; }
        private AppDbContext()
        {
            Database.EnsureCreated();
        }
        public static AppDbContext GetInstance()
        {
            if (dbContext == null)
                dbContext = new AppDbContext();
            return dbContext;
        }
        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;user=root;password=Rfvbycrbq_1;database=coalswebapp",
                new MySqlServerVersion(new Version(8, 0, 11))
            );
        }
    }
}
