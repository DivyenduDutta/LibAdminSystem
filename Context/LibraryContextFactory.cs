using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LibAdminSystem.Context
{

    public class LibraryContextFactory : IDesignTimeDbContextFactory<LibraryContext>
    {
        public LibraryContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseMySql(
                config.GetConnectionString("LibAdminSystem"),
                ServerVersion.AutoDetect(config.GetConnectionString("LibAdminSystem"))
            );

            return new LibraryContext(optionsBuilder.Options);
        }
    }

}
