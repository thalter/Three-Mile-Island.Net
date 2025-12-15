using System;

namespace ThreeMileIsland;

/// <summary>
/// Screen 2: Reactor Core view
/// Shows the control rod positions and reactor core cross-section
/// </summary>
public class ReactorCoreScreen : GameScreen
{
    public ReactorCoreScreen(GameState state, LowResGraphics graphics, SoundSystem sound)
        : base(state, graphics, sound) { }

    public override void Draw()
    {
        Graphics.Clear();
        DrawReactorVessel();
        DrawFuelMarkers();
        DrawControlRods();
        DrawCoreWaterLevel();
    }

    /// <summary>
    /// Draw the reactor vessel outline (lines 12000-12100)
    /// </summary>
    private void DrawReactorVessel()
    {
        // Left wall
        Graphics.SetColor(Colors.DarkBlue);
        Graphics.VLine(5, 39, 0);
        Graphics.VLine(5, 39, 1);
        Graphics.VLine(5, 39, 2);

        // Bottom
        Graphics.HLine(2, 37, 38);
        Graphics.HLine(2, 37, 39);

        // Right wall
        Graphics.VLine(5, 39, 38);
        Graphics.VLine(5, 39, 39);
    }

    /// <summary>
    /// Draw the fuel rod position markers (lines 12100-12110)
    /// </summary>
    private void DrawFuelMarkers()
    {
        Graphics.SetColor(Colors.White);
        for (int y = 36; y >= 5; y -= 2)
            Graphics.Plot(1, y);
    }

    /// <summary>
    /// Draw control rods (lines 12200-12235, 12900-12950)
    /// </summary>
    private void DrawControlRods()
    {
        // Draw the core water/pressure level at top
        for (int y = 4; y >= 0; y--)
        {
            Graphics.SetColor(Colors.DarkBlue);
            if (State.BuildingBuffer[2] < 5 - y) Graphics.SetColor(Colors.Gray2);
            if (State.BuildingBuffer[5] > 7 + y) Graphics.SetColor(Colors.Pink);

            Graphics.HLine(0, 2, y);
            for (int x = 4; x <= 37; x += 2)
                Graphics.Plot(x, y);
            Graphics.HLine(38, 39, y);
        }

        // Draw each control rod
        for (int r = 1; r <= 18; r++)
        {
            int x = r * 2 + 1;
            int pos = State.ControlRodPosition[r];
            int y2 = 37 - pos;

            // Draw the rod (colored portion)
            Graphics.SetColor(State.ControlRodColor[r]);
            if (y2 < 37)
                Graphics.VLine(y2 + 1, 37, x);

            // Draw empty space above
            Graphics.SetColor(Colors.Brown);
            Graphics.VLine(0, y2, x);

            // Draw fuel rod status
            Graphics.SetColor(State.FuelRodStatus[r]);
            if (r < 18)
                Graphics.VLine(5, 37, r * 2 + 2);
        }
    }

    /// <summary>
    /// Draw core water level indicator
    /// </summary>
    private void DrawCoreWaterLevel()
    {
        // This is shown in the control rod display
    }

    public override void Update()
    {
        DrawControlRods();
        DrawCoreWaterLevel();
    }

    public override void HandleInput(ConsoleKeyInfo key)
    {
        int r = State.RodPosition;

        // Move rod selector
        if (key.Key == ConsoleKey.LeftArrow || key.KeyChar == '<')
        {
            if (State.RodPosition > 1)
            {
                ClearRodSelector(State.RodPosition);
                State.RodPosition--;
                DrawRodSelector(State.RodPosition);
            }
            return;
        }

        if (key.Key == ConsoleKey.RightArrow || key.KeyChar == '>')
        {
            if (State.RodPosition < 18)
            {
                ClearRodSelector(State.RodPosition);
                State.RodPosition++;
                DrawRodSelector(State.RodPosition);
            }
            return;
        }

        // Raise rod (+/;)
        if (key.KeyChar == '+' || key.KeyChar == ';')
        {
            RaiseRod(r);
            return;
        }

        // Lower rod (-/=)
        if (key.KeyChar == '-' || key.KeyChar == '=')
        {
            LowerRod(r);
            return;
        }
    }

    private void RaiseRod(int r)
    {
        if (State.ControlRodPosition[r] >= 33) return;

        State.ControlRodTemp++;
        int r1 = (r - 1) / 9 + 1;
        State.ControlRodClusterCount[r1]++;
        State.ControlRodPosition[r]++;

        // Update rod color
        if (State.ControlRodColor[r] == 12)
            State.ControlRodColor[r] = 9;
        if (State.ControlRodPosition[r] > 14)
            State.ControlRodColor[r] = 1;

        UpdateRodDisplay(r);
        UpdateStatusLine();
    }

    private void LowerRod(int r)
    {
        if (State.ControlRodPosition[r] <= 0) return;

        State.ControlRodTemp--;
        int r1 = (r - 1) / 9 + 1;
        State.ControlRodClusterCount[r1]--;
        State.ControlRodPosition[r]--;

        // Update rod color
        if (State.ControlRodPosition[r] < 15)
            State.ControlRodColor[r] = 9;
        if (State.ControlRodPosition[r] < 4)
            State.ControlRodColor[r] = 12;

        UpdateRodDisplay(r);
        UpdateStatusLine();
    }

    private void UpdateRodDisplay(int r)
    {
        int x = r * 2 + 1;
        int pos = State.ControlRodPosition[r];
        int y2 = 37 - pos;

        // Draw the rod
        Graphics.SetColor(State.ControlRodColor[r]);
        if (y2 < 37)
            Graphics.VLine(y2 + 1, 37, x);

        // Draw empty space above
        Graphics.SetColor(Colors.Brown);
        Graphics.VLine(0, y2, x);
    }

    private void ClearRodSelector(int pos)
    {
        Console.SetCursorPosition(pos * 2 + 1, 20);
        Console.Write(" ");
    }

    private void DrawRodSelector(int pos)
    {
        Console.SetCursorPosition(pos * 2 + 1, 20);
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(" ");
        Console.ResetColor();
    }

    private void UpdateStatusLine()
    {
        Console.SetCursorPosition(0, 21);
        Console.Write($"TEMP={State.Temperature}  ");
        Console.SetCursorPosition(19, 21);
        Console.Write($"CNT={State.PumpCount}");
    }

    public override void ShowLabel()
    {
        LowResGraphics.ClearLine(20);
        LowResGraphics.ClearLine(21);
        LowResGraphics.ClearLine(23);

        // Draw rod selector
        DrawRodSelector(State.RodPosition);

        Console.SetCursorPosition(6, 23);
        Console.Write("REACTOR CORE - CONTROL RODS");

        UpdateStatusLine();
    }
}
