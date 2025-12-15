using System;

namespace ThreeMileIsland;

/// <summary>
/// Base class for all game screens
/// </summary>
public abstract class GameScreen
{
    protected GameState State { get; }
    protected LowResGraphics Graphics { get; }
    protected SoundSystem Sound { get; }

    protected GameScreen(GameState state, LowResGraphics graphics, SoundSystem sound)
    {
        State = state;
        Graphics = graphics;
        Sound = sound;
    }

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

    /// <summary>
    /// Helper to write status line at bottom
    /// </summary>
    protected void WriteStatusLine()
    {
        Console.SetCursorPosition(0, 21);
        Console.Write($"TEMP={State.Temperature}  ");
        Console.SetCursorPosition(19, 21);
        Console.Write($"CNT={State.PumpCount}");
    }

    /// <summary>
    /// Helper to write time at bottom
    /// </summary>
    protected void WriteTimeLine()
    {
        Console.SetCursorPosition(16, 23);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(State.FormatTime(State.SimulationCount, true));
        Console.ResetColor();
    }
}
