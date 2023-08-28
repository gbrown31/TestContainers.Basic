using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UsingTestContainers.Persistence
{
    public class SqlEdgeDatabase : AppDbContext
    {
        public SqlEdgeDatabase(int publicPort, string password)
            : base(new DbContextOptionsBuilder<AppDbContext>().UseSqlServer($"server=localhost,{publicPort};database=AppDb;user=sa;password={password};TrustServerCertificate=True").Options)
        {
        }
    }


    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public AppDbContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public bool IsHealthy()
        {
            return base.Database.CanConnect();
        }
    }

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
