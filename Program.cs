using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ThreeMileIsland;

/// <summary>
/// Three Mile Island Nuclear Power Plant Simulation
/// 
/// Converted from the original Apple II Integer Basic game (1979)
/// by Richard Orban for MUSE Software.
/// 
/// This C# version recreates the gameplay experience using ASCII art
/// to approximate the Apple II low-resolution graphics mode.
/// 
/// Controls:
/// - Number keys (0-6): Switch between screens
/// - P: Show pump controls
/// - V: Show valve controls
/// - T: Show turbine controls (Turbine screen only)
/// - F: Show filter controls (Turbine screen only)
/// - Letter keys: Toggle pumps/valves/turbines
/// - Arrow keys: Move control rod selector (Reactor Core screen)
/// - +/-: Raise/lower control rods
/// - ESC: Pause simulation / Return to menu
/// - Enter: Return to main menu
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs/tmi-.log",
                rollingInterval: RollingInterval.Minute,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        try
        {
            // Configure services
            var services = new ServiceCollection();
            
            // Add logging
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog(dispose: true);
                builder.SetMinimumLevel(LogLevel.Information);
            });
            
            // Add game services
            services.AddGameServices();
            
            // Build service provider
            var serviceProvider = services.BuildServiceProvider();
            
            // Resolve and run game engine
            var game = serviceProvider.GetRequiredService<GameEngine>();
            game.Run();
        }
        catch (Exception ex)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("An error occurred:");
            Console.WriteLine(ex.Message);
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}
