using Mss.App.Logger.Persistence.Repository.Context;
using Mss.App.Logger.Persistence.Repository.GenericRepository;
using Microsoft.Extensions.DependencyInjection;

namespace Mss.App.Logger.StartupExtensions;

/// <summary>
/// Provides extension methods for configuring dependency injection services related to persistence and logging.
/// </summary>
/// <remarks>
/// This static class is intended to be used for registering application-specific services into the dependency injection container, such as the database context, logging mechanisms, and generic repositories.
/// </remarks>
public static class ConfigureServices
{
    /// <summary>
    /// Registers services related to database persistence and logging into the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to which the services will be added.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    /// <remarks>
    /// This method adds the following services to the dependency injection container:
    /// <list type="bullet">
    /// <item><see cref="DapperContext"/>: For database connections and queries.</item>
    /// <item><see cref="MssAppLogger"/>: For logging application events.</item>
    /// <item>A generic repository interface <see cref="IGenericRepository{T}"/> with its implementation <see cref="GenericRepository{T}"/>.</item>
    /// </list>
    /// </remarks>
    public static IServiceCollection AddInjectionPersistence(this IServiceCollection services)
    {
        services.AddSingleton<DapperContext>();
        services.AddSingleton<MssAppLogger>();
        services.AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // Initialize database when services are built
        var provider = services.BuildServiceProvider();
        var dbInitializer = provider.GetRequiredService<DapperContext>();
        dbInitializer.InitializeAsync().Wait();

        return services;
    }
}
