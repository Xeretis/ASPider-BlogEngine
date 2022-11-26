using Application.Services.Implementations;
using Application.Services.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class InjectDependencies
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IFileService, FileService>();

        return services;
    }
}