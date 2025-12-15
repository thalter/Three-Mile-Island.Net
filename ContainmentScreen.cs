namespace ThreeMileIsland;

/// <summary>
/// Screen 0: Containment Building view
/// Shows the reactor containment with pressurizer, steamer, and primary cooling system
/// </summary>
public class ContainmentScreen(GameState state, LowResGraphics graphics, SoundSystem sound) : GameScreen(state, graphics, sound)
{
    public override void Draw()
    {
        Graphics.Clear();
        DrawContainmentBuilding();
        DrawPressureIndicator();
        DrawPressurizerWater();
        DrawReactorCore();
        DrawPressurizerLevel();
        DrawSteamer();
        DrawPrimaryLoop();
        DrawSecondaryPool();
        DrawPipes();
        DrawValves();
        DrawPumps();
    }

    /// <summary>
    /// Draw the dome-shaped containment building (lines 10000-10099)
    /// </summary>
    private void DrawContainmentBuilding()
    {
        Graphics.SetColor(Colors.White);

        // Draw the main containment walls
        for (int y = 39; y >= 7; y--)
            Graphics.HLine(2, 37, y);

        // Draw the dome top
        Graphics.HLine(0, 39, 6);
        Graphics.HLine(0, 39, 5);
        Graphics.HLine(2, 37, 4);
        Graphics.HLine(5, 34, 3);
        Graphics.HLine(8, 31, 2);
        Graphics.HLine(11, 28, 1);
        Graphics.HLine(14, 25, 0);
    }

    /// <summary>
    /// Draw the pressure/radiation indicator at top (lines 10600-10690)
    /// </summary>
    private void DrawPressureIndicator()
    {
        // Redraw dome top
        Graphics.SetColor(Colors.White);
        Graphics.HLine(8, 31, 2);
        Graphics.HLine(11, 28, 1);
        Graphics.HLine(14, 25, 0);

        // Draw containment pressure indicator
        Graphics.SetColor(Colors.Pink);
        int c = State.BuildingBuffer[1];
        if (c >= 1) Graphics.HLine(9, 17, 2);
        if (c >= 1) Graphics.HLine(26, 30, 2);
        if (c >= 2) Graphics.HLine(18, 22, 2);
        if (c >= 3) Graphics.HLine(12, 15, 1);
        if (c >= 4) Graphics.HLine(16, 19, 1);
        if (c >= 5) Graphics.HLine(20, 23, 1);
        if (c >= 6) Graphics.HLine(24, 27, 1);
        if (c >= 7) Graphics.HLine(15, 17, 0);
        if (c >= 8) Graphics.HLine(18, 21, 0);
        if (c >= 9) Graphics.HLine(22, 24, 0);

        // Draw floor water level indicator
        Graphics.SetColor(Colors.White);
        Graphics.HLine(2, 37, 39);
        Graphics.HLine(2, 37, 38);

        Graphics.SetColor(Colors.DarkBlue);
        c = State.BuildingBuffer[7];
        if (c >= 1) Graphics.HLine(3, 8, 38);
        if (c >= 2) Graphics.HLine(9, 15, 38);
        if (c >= 3) Graphics.HLine(16, 22, 38);
        if (c >= 4) Graphics.HLine(23, 29, 38);
        if (c >= 5) Graphics.HLine(30, 36, 38);
        if (c >= 6) Graphics.HLine(31, 36, 39);
        if (c >= 7) Graphics.HLine(3, 9, 39);
        if (c >= 8) Graphics.HLine(24, 30, 39);
        if (c >= 9) Graphics.HLine(10, 16, 39);
        if (c >= 10) Graphics.HLine(17, 23, 39);
    }

    /// <summary>
    /// Draw pressurizer water level (lines 10700-10775)
    /// </summary>
    private void DrawPressurizerWater()
    {
        // Clear pressurizer area
        Graphics.SetColor(Colors.Black);
        Graphics.VLine(19, 26, 7);
        Graphics.HLine(7, 18, 27);
        Graphics.VLine(19, 26, 18);
        Graphics.VLine(9, 17, 7);
        Graphics.Plot(8, 8);
        Graphics.HLine(9, 10, 7);
        Graphics.HLine(11, 14, 6);
        Graphics.HLine(15, 16, 7);
        Graphics.Plot(17, 8);
        Graphics.VLine(9, 17, 18);

        // Draw water
        Graphics.SetColor(Colors.DarkBlue);
        Graphics.VLine(19, 26, 8);
        Graphics.VLine(19, 26, 9);
        Graphics.HLine(9, 16, 26);
        Graphics.HLine(9, 16, 25);
        Graphics.VLine(19, 26, 16);
        Graphics.VLine(19, 26, 17);

        // Draw water level based on pressure
        for (int i = 1; i <= 12; i++)
        {
            Graphics.SetColor(Colors.Gray2);
            if (State.BuildingBuffer[2] >= i) Graphics.SetColor(Colors.DarkBlue);
            if (State.BuildingBuffer[5] > 12 - i) Graphics.SetColor(Colors.Pink);

            if (i <= 6)
            {
                Graphics.HLine(8, 10, 19 - i);
                if (State.ControlRodClusterCount[1] == 0) Graphics.Plot(11, 19 - i);
                Graphics.HLine(12, 13, 19 - i);
                if (State.ControlRodClusterCount[2] == 0) Graphics.Plot(14, 19 - i);
                Graphics.HLine(15, 17, 19 - i);
            }
            else if (i < 11)
            {
                Graphics.HLine(8, 17, 19 - i);
            }
            else if (i == 11)
            {
                Graphics.HLine(9, 16, 8);
            }
            else if (i == 12)
            {
                Graphics.HLine(11, 14, 7);
            }
        }
    }

    /// <summary>
    /// Draw the reactor core fuel rods display (lines 10100-10160)
    /// </summary>
    private void DrawReactorCore()
    {
        int x = 10, y = 19;

        for (int i = 1; i <= 6; i++)
        {
            int x0 = i % 2;
            int r = i + (i - 1) / 2;

            // Left cluster
            Graphics.SetColor(State.ControlRodColor[r]);
            if (x0 == 1) Graphics.Plot(x, y);
            Graphics.SetColor(State.FuelRodStatus[r]);
            Graphics.Plot(x + x0, y);
            Graphics.SetColor(State.ControlRodColor[r + 1]);
            Graphics.Plot(x + 1 + x0, y);
            Graphics.SetColor(State.FuelRodStatus[r + 1]);
            if (x0 == 0) Graphics.Plot(x + 2, y);

            // Right cluster
            Graphics.SetColor(State.FuelRodStatus[r + 9]);
            if (x0 == 1) Graphics.Plot(x + 3, y);
            Graphics.SetColor(State.ControlRodColor[r + 9]);
            Graphics.Plot(x + 3 + x0, y);
            Graphics.SetColor(State.FuelRodStatus[r + 10]);
            Graphics.Plot(x + 4 + x0, y);
            Graphics.SetColor(State.ControlRodColor[r + 10]);
            if (x0 == 0) Graphics.Plot(x + 5, y);

            y++;
        }

        // Draw cluster indicators
        Graphics.SetColor(Colors.Brown);
        int c = (State.ControlRodClusterCount[1] != 0) ? 6 : 0;
        Graphics.VLine(19 - c, 24 - c, 11);
        c = (State.ControlRodClusterCount[2] != 0) ? 6 : 0;
        Graphics.VLine(19 - c, 24 - c, 14);
    }

    /// <summary>
    /// Draw pressurizer level (lines 10200-10240)
    /// </summary>
    private void DrawPressurizerLevel()
    {
        // Clear area
        Graphics.SetColor(Colors.Black);
        Graphics.HLine(20, 22, 16);
        Graphics.HLine(24, 26, 16);
        Graphics.VLine(7, 15, 26);
        Graphics.Plot(27, 6);
        Graphics.Plot(26, 5);
        Graphics.Plot(25, 4);
        Graphics.Plot(24, 3);
        Graphics.Plot(22, 3);
        Graphics.Plot(21, 4);
        Graphics.Plot(20, 5);
        Graphics.Plot(19, 6);
        Graphics.VLine(7, 15, 20);

        // Draw water level
        int c = State.BuildingBuffer[3];
        for (int y = 15; y >= 4; y--)
        {
            Graphics.SetColor(Colors.Pink);
            if (c > 15 - y) Graphics.SetColor(Colors.DarkBlue);

            if (y == 5 || y > 6) Graphics.HLine(21, 25, y);
            if (y == 6) Graphics.HLine(20, 26, y);
            if (y == 4) Graphics.HLine(22, 24, y);
        }
    }

    /// <summary>
    /// Draw the steamer (lines 10300-10370)
    /// </summary>
    private void DrawSteamer()
    {
        // Clear area
        Graphics.SetColor(Colors.Black);
        Graphics.VLine(8, 19, 28);
        Graphics.Plot(29, 7);
        Graphics.Plot(30, 6);
        Graphics.HLine(31, 33, 5);
        Graphics.Plot(34, 6);
        Graphics.Plot(35, 7);
        Graphics.Plot(36, 8);
        Graphics.VLine(10, 13, 36);
        Graphics.VLine(15, 19, 36);

        // Draw steam/water based on steamer level
        int c = State.BuildingBuffer[4];

        Graphics.SetColor(Colors.Aqua);
        if (c > 0) Graphics.SetColor(Colors.Gray2);
        Graphics.HLine(31, 33, 6);

        Graphics.SetColor(Colors.Aqua);
        if (c > 1) Graphics.SetColor(Colors.Gray2);
        Graphics.HLine(30, 34, 7);

        for (int y = 8; y <= 16; y++)
        {
            Graphics.SetColor(Colors.Aqua);
            if (c > y - 6) Graphics.SetColor(Colors.Gray2);
            Graphics.HLine(29, 35, y);
        }

        Graphics.SetColor(Colors.Aqua);
        if (c > 11) Graphics.SetColor(Colors.Gray2);
        Graphics.Plot(29, 17);
        Graphics.Plot(35, 17);

        Graphics.SetColor(Colors.Aqua);
        if (c > 12) Graphics.SetColor(Colors.Gray2);
        Graphics.Plot(29, 18);
        Graphics.HLine(31, 33, 18);
        Graphics.Plot(35, 18);

        Graphics.SetColor(Colors.Aqua);
        if (c > 13) Graphics.SetColor(Colors.Gray2);
        Graphics.Plot(29, 19);
        Graphics.HLine(31, 33, 19);
        Graphics.Plot(35, 19);
    }

    /// <summary>
    /// Draw primary cooling loop pipes (lines 10400-10499)
    /// </summary>
    private void DrawPrimaryLoop()
    {
        Graphics.SetColor(Colors.Black);
        Graphics.VLine(22, 29, 28);
        Graphics.HLine(28, 31, 30);
        Graphics.HLine(33, 36, 30);
        Graphics.VLine(21, 29, 36);
        Graphics.HLine(28, 29, 20);
        Graphics.HLine(31, 33, 20);
        Graphics.HLine(35, 36, 20);

        Graphics.SetColor(Colors.DarkBlue);
        for (int y = 21; y <= 29; y++)
            Graphics.HLine(29, 35, y);

        Graphics.HLine(30, 34, 17);
        Graphics.VLine(17, 20, 30);
        Graphics.VLine(17, 20, 34);
    }

    /// <summary>
    /// Draw secondary pool (lines 10500-10599)
    /// </summary>
    private void DrawSecondaryPool()
    {
        Graphics.SetColor(Colors.DarkBlue);
        for (int y = 23; y <= 26; y++)
            Graphics.HLine(22, 26, y);
    }

    /// <summary>
    /// Draw all pipes for containment screen (lines 16000-16012)
    /// </summary>
    private void DrawPipes()
    {
        // Pipe 1 - Steam line to turbine
        Graphics.SetColor(State.PipeStatus[1]);
        Graphics.HLine(36, 39, 9);

        // Pipe 2 - Feedwater return
        Graphics.SetColor(State.PipeStatus[2]);
        Graphics.HLine(36, 39, 14);

        // Pipe 3 - Core loop
        Graphics.SetColor(State.PipeStatus[3]);
        Graphics.HLine(18, 22, 18);
        Graphics.VLine(16, 21, 23);
        Graphics.HLine(24, 28, 21);

        // Pipe 4 - Pump inlet
        Graphics.SetColor(State.PipeStatus[4]);
        Graphics.VLine(30, 36, 32);

        // Pipes 5-7 - Pump outlets
        Graphics.SetColor(State.PipeStatus[5]);
        Graphics.HLine(6, 31, 32);

        Graphics.SetColor(State.PipeStatus[6]);
        Graphics.HLine(6, 31, 34);

        Graphics.SetColor(State.PipeStatus[7]);
        Graphics.HLine(6, 31, 36);

        // Pipe 8 - Pump manifold
        Graphics.SetColor(State.PipeStatus[8]);
        Graphics.VLine(30, 36, 5);

        // Pipe 9 - Return line
        Graphics.SetColor(State.PipeStatus[9]);
        Graphics.VLine(19, 29, 5);
        Graphics.HLine(5, 7, 18);

        // Pipe 10 - ECCS inlet
        Graphics.SetColor(State.PipeStatus[10]);
        Graphics.VLine(27, 29, 25);

        // Pipe 11 - ECCS manifold
        Graphics.SetColor(State.PipeStatus[11]);
        Graphics.HLine(6, 24, 29);

        // Pipe 12 - Containment drain
        Graphics.SetColor(State.PipeStatus[12]);
        Graphics.HLine(0, 3, 35);
        Graphics.VLine(35, 37, 3);
    }

    /// <summary>
    /// Draw valves (lines 14000-14008)
    /// </summary>
    private void DrawValves()
    {
        // Valve 1 - Steam
        Graphics.SetColor(State.ValveActive[1]);
        Graphics.Plot(23, 3);

        // Valve 2 - Feedwater
        Graphics.SetColor(State.ValveActive[2]);
        Graphics.Plot(37, 9);

        // Valve 3
        Graphics.SetColor(State.ValveActive[3]);
        Graphics.Plot(37, 14);

        // Valve 4
        Graphics.SetColor(State.ValveActive[4]);
        Graphics.Plot(15, 29);

        // Valve 5
        Graphics.SetColor(State.ValveActive[5]);
        Graphics.Plot(13, 32);

        // Valve 6
        Graphics.SetColor(State.ValveActive[6]);
        Graphics.Plot(11, 34);

        // Valve 7
        Graphics.SetColor(State.ValveActive[7]);
        Graphics.Plot(9, 36);

        // Valve 8
        Graphics.SetColor(State.ValveActive[8]);
        Graphics.Plot(2, 35);
    }

    /// <summary>
    /// Draw pumps (lines 15000-15006)
    /// </summary>
    private void DrawPumps()
    {
        // Pumps A, B, C (1-3)
        Graphics.SetColor(State.PumpStatus[1]);
        Graphics.VLine(24, 26, 24);

        Graphics.SetColor(State.PumpStatus[2]);
        Graphics.VLine(24, 26, 25);

        Graphics.SetColor(State.PumpStatus[3]);
        Graphics.VLine(24, 26, 26);

        // Pumps D, E, F (4-6)
        Graphics.SetColor(State.PumpStatus[4]);
        Graphics.VLine(27, 29, 31);

        Graphics.SetColor(State.PumpStatus[5]);
        Graphics.VLine(27, 29, 32);

        Graphics.SetColor(State.PumpStatus[6]);
        Graphics.VLine(27, 29, 33);
    }

    public override void Update()
    {
        DrawPressureIndicator();
        DrawPressurizerWater();
        DrawReactorCore();
        DrawPressurizerLevel();
        DrawSteamer();
        DrawPipes();
        DrawValves();
        DrawPumps();
    }

    public override void HandleInput(ConsoleKeyInfo key)
    {
        char c = char.ToUpper(key.KeyChar);

        if (key.Key == ConsoleKey.P)
        {
            // Show pump labels
            ShowPumpLabels();
            State.CurrentLabel = 1;
            return;
        }

        if (key.Key == ConsoleKey.V)
        {
            // Show valve labels
            ShowValveLabels();
            State.CurrentLabel = 2;
            return;
        }

        // Handle pump toggle (A-F = pumps 1-6)
        if (State.CurrentLabel == 1 && c >= 'A' && c <= 'F')
        {
            int u = c - 'A' + 1;
            TogglePump(u);
        }

        // Handle valve toggle (A-H = valves 1-8)
        if (State.CurrentLabel == 2 && c >= 'A' && c <= 'H')
        {
            int v = c - 'A' + 1;
            ToggleValve(v);
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
        LowResGraphics.ClearLine(22);
        LowResGraphics.ClearLine(23);

        Console.SetCursorPosition(24, 21);
        Console.Write("ABC");
        Console.SetCursorPosition(31, 21);
        Console.Write("DEF");

        Console.SetCursorPosition(10, 23);
        Console.Write("CONTAINMENT - PUMPS");
    }

    private void ShowValveLabels()
    {
        LowResGraphics.ClearLine(20);
        LowResGraphics.ClearLine(21);
        LowResGraphics.ClearLine(22);
        LowResGraphics.ClearLine(23);

        Console.SetCursorPosition(23, 20);
        Console.Write("A");
        Console.SetCursorPosition(37, 20);
        Console.Write("B");

        Console.SetCursorPosition(2, 21);
        Console.Write("H");
        Console.SetCursorPosition(9, 21);
        Console.Write("G F E D");
        Console.SetCursorPosition(37, 21);
        Console.Write("C");

        Console.SetCursorPosition(10, 23);
        Console.Write("CONTAINMENT - VALVES");
    }
}
