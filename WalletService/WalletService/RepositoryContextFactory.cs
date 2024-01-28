using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace WalletService;

public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext> {
    public RepositoryContext CreateDbContext(string[] args) {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<RepositoryContext>()
            .UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("WalletService")
            );

        return new RepositoryContext(builder.Options);
    }
}