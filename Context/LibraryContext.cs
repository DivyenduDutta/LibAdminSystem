using LibAdminSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LibAdminSystem.Context
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<Loan> Loans => Set<Loan>();

        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }
        
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("LibAdminSystem");
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }*/
    }
}
