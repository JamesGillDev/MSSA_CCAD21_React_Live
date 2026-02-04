using AreYouDoneYetModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;

namespace AreYouDoneYetDataLayer;

public class AreYouDoneYetDbContext: DbContext
{
    public DbSet<Assignment> Assignments { get; set; }

    public AreYouDoneYetDbContext()
    {
        //blank
    }

    public AreYouDoneYetDbContext(DbContextOptions options)
    : base(options)
    {
        //blank
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.Options.Extensions.OfType<SqlServerOptionsExtension>().Any())
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var connStr = config.GetConnectionString("AreYouDoneYetDbConnection");
            optionsBuilder.UseSqlServer(connStr);
        }
    }
}
