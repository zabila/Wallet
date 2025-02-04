using Microsoft.EntityFrameworkCore;
using Wallet.Infrastructure.Repository;

namespace Wallet.API.Identity.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<RepositoryContext>();
        context.Database.Migrate();
    }

}
