using System;

namespace ThreeMileIsland;

/// <summary>
/// Sound system for audio feedback. Uses console beeps to mimic Apple II speaker sounds.
/// </summary>
public class SoundSystem(bool enabled = true)
{
    private readonly bool _soundEnabled = enabled && OperatingSystem.IsWindows();

    /// <summary>
    /// Play a single beep (CALL BELL equivalent)
    /// </summary>
    public void Beep()
    {
        if (!_soundEnabled) return;
        try
        {
            Console.Beep(800, 50);
        }
        catch
        {
            // Ignore if beep fails (e.g., on systems without speakers)
        }
    }

    /// <summary>
    /// Play multiple beeps
    /// </summary>
    public void Beep(int count)
    {
        for (int i = 0; i < count; i++)
            Beep();
    }

    /// <summary>
    /// Play warning sound (3 beeps)
    /// </summary>
    public void Warning()
    {
        Beep(3);
    }

    /// <summary>
    /// Play alert sound (double beep pattern)
    /// </summary>
    public void Alert()
    {
        Beep();
        Beep();
    }

    /// <summary>
    /// Play emergency alarm (15 beeps as per original)
    /// </summary>
    public void EmergencyAlarm()
    {
        if (!_soundEnabled) return;
        try
        {
            for (int i = 0; i < 15; i++)
                Console.Beep(1000, 100);
        }
        catch { }
    }

    /// <summary>
    /// Play day change notification (4 sets of 3 beeps)
    /// </summary>
    public void DayChange()
    {
        if (!_soundEnabled) return;
        try
        {
            for (int c = 0; c < 4; c++)
            {
                Console.Beep(600, 50);
                Console.Beep(600, 50);
                Console.Beep(600, 50);
                Thread.Sleep(100);
            }
        }
        catch { }
    }

    /// <summary>
    /// Play demand change notification (2 sets of 3 beeps)
    /// </summary>
    public void DemandChange()
    {
        if (!_soundEnabled) return;
        try
        {
            for (int c = 0; c < 2; c++)
            {
                Console.Beep(700, 50);
                Console.Beep(700, 50);
                Console.Beep(700, 50);
                Thread.Sleep(100);
            }
        }
        catch { }
    }

    /// <summary>
    /// Play meltdown sound effect
    /// </summary>
    public void Meltdown()
    {
        if (!_soundEnabled) return;
        try
        {
            // Dramatic descending tones
            for (int freq = 1000; freq > 200; freq -= 50)
            {
                Console.Beep(freq, 30);
            }
        }
        catch { }
    }

    /// <summary>
    /// Play speaker click (SPK poke equivalent)
    /// </summary>
    public void Click()
    {
        // In the original, this toggled the speaker for timing
        // We don't need to implement this for the simulation
    }
}
