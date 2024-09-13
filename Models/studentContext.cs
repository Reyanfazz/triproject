using Microsoft.EntityFrameworkCore;
using certificate.Models;

namespace certificate.Models
{
    public class studentContext : DbContext
    {
        public studentContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<student> students { get; set; }


        public DbSet<certificate.Models.studentmodel> Studentmodel { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<student>().ToTable("student");
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseSqlServer("Data Source=mssql101.windows.loopia.com;Initial Catalog=e003404;Persist Security Info=True;User ID=e003404a;Password=KmeDataBase4321!;Trust Server Certificate=True");
        }
        public DbSet<certificate.Models.updatecertModel> updatecertModel { get; set; }
    }
}
