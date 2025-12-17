using Microsoft.Extensions.Logging;

namespace ThreeMileIsland;

/// <summary>
/// Base class for all game screens
/// </summary>
public abstract class GameScreen(GameState state, LowResGraphics graphics, SoundSystem sound, ILogger logger)
{
    protected GameState State { get; } = state;
    protected LowResGraphics Graphics { get; } = graphics;
    protected SoundSystem Sound { get; } = sound;
    protected ILogger Logger { get; } = logger;

    /// <summary>
    /// Draw the screen graphics
    /// </summary>
    public abstract void Draw();

    /// <summary>
    /// Update the screen based on game state changes
    /// </summary>
    public abstract void Update();

    /// <summary>
    /// Handle keyboard input
    /// </summary>
    public abstract void HandleInput(ConsoleKeyInfo key);

    /// <summary>
    /// Show the screen label/mode indicator
    /// </summary>
    public abstract void ShowLabel();

    /// <summary>
    /// Render the graphics to console
    /// </summary>
    public virtual void Render()
    {
        Graphics.Render(0);
    }
}
