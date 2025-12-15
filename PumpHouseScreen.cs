namespace ThreeMileIsland;

/// <summary>
/// Screen 3: Pump House view
/// Shows emergency pumps, water tanks, and radiation levels
/// </summary>
public class PumpHouseScreen(GameState state, LowResGraphics graphics, SoundSystem sound) : GameScreen(state, graphics, sound)
{
    public override void Draw()
    {
        Graphics.Clear();
        DrawBuildingWalls();
        DrawWaterTanks();
        DrawContainmentWaterTank();
        DrawPumpHouseWaterTank();
        DrawPumpHouseWaterLevel();
        DrawRadiationLevel();
        DrawPipes();
        DrawValves();
        DrawPumps();
    }

    /// <summary>
    /// Draw building walls (lines 13000-13099)
    /// </summary>
    private void DrawBuildingWalls()
    {
        // Right wall
        Graphics.SetColor(Colors.White);
        Graphics.VLine(4, 39, 38);
        Graphics.VLine(4, 39, 39);
        Graphics.VLine(5, 6, 37);
        Graphics.VLine(5, 6, 36);
    }

    /// <summary>
    /// Draw water tanks structure (lines 13100-13140)
    /// </summary>
    private void DrawWaterTanks()
    {
        Graphics.SetColor(Colors.Aqua);

        // Top pipe/header
        Graphics.HLine(16, 20, 2);
        Graphics.VLine(3, 17, 17);
        Graphics.VLine(3, 17, 18);
        Graphics.VLine(3, 17, 19);

        // Main lines
        Graphics.HLine(0, 35, 18);
        Graphics.HLine(0, 35, 19);

        // Tank area
        for (int y = 20; y <= 39; y++)
            Graphics.HLine(2, 33, y);
    }

    /// <summary>
    /// Draw containment water tank (lines 13200-13245)
    /// </summary>
    private void DrawContainmentWaterTank()
    {
        // Clear tank area
        Graphics.SetColor(Colors.Black);
        Graphics.HLine(22, 31, 26);
        Graphics.VLine(27, 34, 31);
        Graphics.Plot(31, 36);
        Graphics.HLine(22, 31, 37);
        Graphics.VLine(27, 36, 22);

        // Draw water level
        int level = State.BuildingBuffer[8];
        for (int i = 1; i <= 10; i++)
        {
            Graphics.SetColor(Colors.Gray2);
            if (level / 5 >= i) Graphics.SetColor(Colors.DarkBlue);

            if (i > 3)
                Graphics.HLine(23, 30, 37 - i);
            else
                Graphics.HLine(23, 27, 37 - i);
        }
    }

    /// <summary>
    /// Draw pump house water tank (lines 13300-13345)
    /// </summary>
    private void DrawPumpHouseWaterTank()
    {
        // Clear tank area
        Graphics.SetColor(Colors.Black);
        Graphics.HLine(5, 17, 20);
        Graphics.VLine(21, 33, 17);
        Graphics.Plot(17, 35);
        Graphics.HLine(4, 17, 36);
        Graphics.VLine(20, 35, 4);

        // Draw water level
        int level = State.BuildingBuffer[9];
        for (int i = 1; i <= 15; i++)
        {
            Graphics.SetColor(Colors.Gray2);
            if (level / 5 >= i) Graphics.SetColor(Colors.DarkBlue);

            if (i > 3)
                Graphics.HLine(5, 16, 36 - i);
            else
                Graphics.HLine(5, 13, 36 - i);
        }
    }

    /// <summary>
    /// Draw pump house water level on floor (lines 13400-13430)
    /// </summary>
    private void DrawPumpHouseWaterLevel()
    {
        Graphics.SetColor(Colors.Aqua);
        Graphics.HLine(2, 33, 38);
        Graphics.HLine(2, 33, 39);

        int c = State.BuildingBuffer[10];
        Graphics.SetColor(Colors.DarkBlue);

        if (c >= 1) { Graphics.HLine(3, 4, 38); Graphics.Plot(32, 38); }
        if (c >= 2) { Graphics.HLine(5, 7, 38); Graphics.HLine(30, 31, 38); }
        if (c >= 3) { Graphics.HLine(8, 10, 38); Graphics.HLine(28, 29, 38); }
        if (c >= 4) { Graphics.HLine(11, 13, 38); Graphics.HLine(25, 27, 38); }
        if (c >= 5) { Graphics.HLine(14, 17, 38); Graphics.HLine(22, 24, 38); }
        if (c >= 6) Graphics.HLine(17, 21, 38);
        if (c >= 7) { Graphics.HLine(3, 6, 39); Graphics.HLine(30, 32, 39); }
        if (c >= 8) { Graphics.HLine(7, 11, 39); Graphics.HLine(27, 29, 39); }
        if (c >= 9) { Graphics.HLine(12, 16, 39); Graphics.HLine(22, 26, 39); }
        if (c >= 10) Graphics.HLine(17, 21, 39);
    }

    /// <summary>
    /// Draw radiation level indicator (lines 13500-13535)
    /// </summary>
    private void DrawRadiationLevel()
    {
        Graphics.SetColor(Colors.Black);
        Graphics.HLine(0, 39, 0);
        Graphics.HLine(0, 39, 1);

        Graphics.SetColor(Colors.Pink);
        int c = State.BuildingBuffer[11] / 10;

        if (c >= 1) Graphics.HLine(36, 39, 1);
        if (c >= 2) Graphics.HLine(31, 35, 1);
        if (c >= 3) Graphics.HLine(0, 5, 1);
        if (c >= 4) Graphics.HLine(26, 30, 1);
        if (c >= 5) Graphics.HLine(6, 10, 1);
        if (c >= 6) Graphics.HLine(21, 25, 1);
        if (c >= 7) Graphics.HLine(11, 15, 1);
        if (c >= 8) Graphics.HLine(16, 20, 1);
        if (c >= 9) Graphics.HLine(0, 5, 0);
        if (c >= 10) Graphics.HLine(35, 39, 0);
        if (c >= 11) Graphics.HLine(6, 10, 0);
        if (c >= 12) Graphics.HLine(28, 34, 0);
        if (c >= 13) Graphics.HLine(11, 15, 0);
        if (c >= 14) Graphics.HLine(22, 27, 0);
        if (c >= 15) Graphics.HLine(16, 21, 0);
    }

    /// <summary>
    /// Draw pipes (lines 16300-16323)
    /// </summary>
    private void DrawPipes()
    {
        // Pipe 12 - Containment drain
        Graphics.SetColor(State.PipeStatus[12]);
        Graphics.HLine(31, 39, 35);
        Graphics.VLine(35, 37, 39);

        // Pipe 23 - Pump house drain
        Graphics.SetColor(State.PipeStatus[23]);
        Graphics.HLine(17, 19, 34);
        Graphics.VLine(34, 37, 19);
    }

    /// <summary>
    /// Draw valves (lines 14300-14319)
    /// </summary>
    private void DrawValves()
    {
        // Valve 8
        Graphics.SetColor(State.ValveActive[8]);
        Graphics.Plot(38, 35);

        // Valve 19
        Graphics.SetColor(State.ValveActive[19]);
        Graphics.Plot(19, 34);
    }

    /// <summary>
    /// Draw pumps (lines 15300-15324)
    /// </summary>
    private void DrawPumps()
    {
        // Pumps S, T, U (19-21)
        Graphics.SetColor(State.PumpStatus[19]);
        Graphics.VLine(33, 35, 14);

        Graphics.SetColor(State.PumpStatus[20]);
        Graphics.VLine(33, 35, 15);

        Graphics.SetColor(State.PumpStatus[21]);
        Graphics.VLine(33, 35, 16);

        // Pumps V, W, X (22-24)
        Graphics.SetColor(State.PumpStatus[22]);
        Graphics.VLine(34, 36, 28);

        Graphics.SetColor(State.PumpStatus[23]);
        Graphics.VLine(34, 36, 29);

        Graphics.SetColor(State.PumpStatus[24]);
        Graphics.VLine(34, 36, 30);
    }

    public override void Update()
    {
        DrawContainmentWaterTank();
        DrawPumpHouseWaterTank();
        DrawPumpHouseWaterLevel();
        DrawRadiationLevel();
        DrawPipes();
        DrawValves();
        DrawPumps();
    }

    public override void HandleInput(ConsoleKeyInfo key)
    {
        char c = char.ToUpper(key.KeyChar);

        if (key.Key == ConsoleKey.P)
        {
            ShowPumpLabels();
            State.CurrentLabel = 1;
            return;
        }

        if (key.Key == ConsoleKey.V)
        {
            ShowValveLabels();
            State.CurrentLabel = 2;
            return;
        }

        // Handle pump toggle (S-X = pumps 19-24)
        if (State.CurrentLabel == 1 && c >= 'S' && c <= 'X')
        {
            int u = c - 'A' + 1;
            TogglePump(u);
        }

        // Handle valve toggle (H, S = valves 8, 19)
        if (State.CurrentLabel == 2)
        {
            if (c == 'H') ToggleValve(8);
            if (c == 'S') ToggleValve(19);
        }
    }

    private void TogglePump(int u)
    {
        if (State.PumpStatus[u] == 0 || State.PumpCountdown[u] > GameState.PumpFailure1 + GameState.PumpFailure0)
            return;

        int c = State.PumpStatus[u] == 1 ? 12 : 1;
        State.PumpStatus[u] = c;
        State.PumpCountdown[u] -= State.Rnd.Next(GameState.PumpAdjust1) + GameState.PumpAdjust0;
        DrawPumps();
    }

    private void ToggleValve(int v)
    {
        if (State.ValveActive[v] == 0 || State.ValveCountdown[v] > GameState.ValveFailure1 + GameState.ValveFailure0)
            return;

        int c = State.ValveActive[v] == 1 ? 12 : 1;
        State.ValveActive[v] = c;
        State.ValveCountdown[v] -= State.Rnd.Next(GameState.ValveAdjust1) + GameState.ValveAdjust0;
        DrawValves();
    }

    public override void ShowLabel()
    {
        if (State.CurrentLabel == 1)
            ShowPumpLabels();
        else
            ShowValveLabels();
    }

    private void ShowPumpLabels()
    {
        LowResGraphics.ClearLine(21);
        LowResGraphics.ClearLine(23);

        Console.SetCursorPosition(14, 21);
        Console.Write("STU");
        Console.SetCursorPosition(28, 21);
        Console.Write("VWX");

        Console.SetCursorPosition(11, 23);
        Console.Write("PUMP HOUSE - PUMPS");
    }

    private void ShowValveLabels()
    {
        LowResGraphics.ClearLine(21);
        LowResGraphics.ClearLine(23);

        Console.SetCursorPosition(19, 21);
        Console.Write("S");
        Console.SetCursorPosition(38, 21);
        Console.Write("H");

        Console.SetCursorPosition(10, 23);
        Console.Write("PUMP HOUSE - VALVES");
    }
}
