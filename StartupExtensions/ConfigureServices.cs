using Mss.App.Logger.Persistence.Repository.Context;
using Mss.App.Logger.Persistence.Repository.GenericRepository;
using Microsoft.Extensions.DependencyInjection;

namespace Mss.App.Logger.StartupExtensions;

public static class ConfigureServices
{
    public static IServiceCollection AddInjectionPersistence(this IServiceCollection services)
    {
        services.AddSingleton<DapperContext>();
        services.AddSingleton<MssAppLogger>();
        services.AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        // services.AddScoped<IUserRepository, UserRepository>();
        // services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
