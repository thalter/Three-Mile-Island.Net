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

        try
        {
            var game = new GameEngine();
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
