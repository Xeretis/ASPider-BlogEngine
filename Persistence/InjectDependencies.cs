using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Common;
using Persistence.Contexts;

namespace Persistence;

public static class InjectDependencies
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        return services;
    }
}