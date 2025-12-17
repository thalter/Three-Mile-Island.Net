using Microsoft.Extensions.DependencyInjection;

namespace ThreeMileIsland;

/// <summary>
/// Extension methods for configuring game services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds all game services to the service collection.
    /// </summary>
    public static IServiceCollection AddGameServices(this IServiceCollection services)
    {
        // Register core services as singletons
        services.AddSingleton<GameState>();
        services.AddSingleton<LowResGraphics>();
        services.AddSingleton<SoundSystem>();

        // Register screens as transient (they hold references to state/graphics/sound)
        services.AddTransient<ContainmentScreen>();
        services.AddTransient<TurbineScreen>();
        services.AddTransient<ReactorCoreScreen>();
        services.AddTransient<PumpHouseScreen>();
        services.AddTransient<MaintenanceScreen>();
        services.AddTransient<CostAnalysisScreen>();
        services.AddTransient<OperationalStatusScreen>();

        // Register the game engine
        services.AddSingleton<GameEngine>();

        return services;
    }
}
