namespace ThreeMileIsland;

/// <summary>
/// Screen 1: Turbine, Filter, and Condenser view
/// Shows turbines, air filters, condenser, and cooling tower
/// </summary>
public class TurbineScreen(GameState state, LowResGraphics graphics, SoundSystem sound) : GameScreen(state, graphics, sound)
{
    public override void Draw()
    {
        Graphics.Clear();
        DrawSteamPipes();
        DrawTurbines();
        DrawCondenser();
        DrawFilters();
        DrawCoolingTower();
        DrawSecondaryPool();
        DrawAirIntake();
        DrawWaterPool();
        DrawPipes();
        DrawValves();
        DrawPumps();
    }

    /// <summary>
    /// Draw steam pipes on left (lines 11000-11099)
    /// </summary>
    private void DrawSteamPipes()
    {
        Graphics.SetColor(Colors.White);
        Graphics.VLine(4, 39, 0);
        Graphics.VLine(4, 39, 1);
        Graphics.VLine(5, 6, 2);
        Graphics.VLine(5, 6, 3);
    }

    /// <summary>
    /// Draw turbines (lines 11100-11199)
    /// </summary>
    private void DrawTurbines()
    {
        // Base line
        Graphics.SetColor(Colors.White);
        Graphics.HLine(10, 24, 14);

        // Turbine separators
        Graphics.SetColor(Colors.Gray1);
        Graphics.VLine(7, 12, 13);
        Graphics.VLine(7, 12, 17);
        Graphics.VLine(7, 12, 21);

        // Turbine 1 (A)
        Graphics.SetColor((int)State.TurbineActive[1]);
        Graphics.VLine(8, 13, 10);
        Graphics.VLine(7, 13, 11);
        Graphics.VLine(6, 13, 12);

        // Turbine 2 (B)
        Graphics.SetColor((int)State.TurbineActive[2]);
        Graphics.VLine(6, 13, 14);
        Graphics.VLine(6, 13, 15);
        Graphics.VLine(6, 13, 16);

        // Turbine 3 (C)
        Graphics.SetColor((int)State.TurbineActive[3]);
        Graphics.VLine(6, 13, 18);
        Graphics.VLine(6, 13, 19);
        Graphics.VLine(6, 13, 20);

        // Turbine 4 (D)
        Graphics.SetColor((int)State.TurbineActive[4]);
        Graphics.VLine(6, 13, 22);
        Graphics.VLine(7, 13, 23);
        Graphics.VLine(8, 13, 24);
    }

    /// <summary>
    /// Draw condenser (lines 11200-11299)
    /// </summary>
    private void DrawCondenser()
    {
        int c = State.BuildingBuffer[6];

        for (int y = 15; y <= 16; y++)
        {
            Graphics.SetColor(Colors.LightBlue);
            if (c > y - 15) Graphics.SetColor(Colors.Gray2);
            Graphics.HLine(12, 22, y);
        }

        Graphics.SetColor(Colors.LightBlue);
        if (c > 2) Graphics.SetColor(Colors.Gray2);
        Graphics.Plot(12, 17);

        Graphics.SetColor(Colors.LightBlue);
        if (c > 3) Graphics.SetColor(Colors.Gray2);
        Graphics.Plot(12, 18);
        Graphics.HLine(14, 22, 18);

        Graphics.SetColor(Colors.LightBlue);
        if (c > 4) Graphics.SetColor(Colors.Gray2);
        Graphics.Plot(12, 19);

        for (int y = 20; y <= 21; y++)
        {
            Graphics.SetColor(Colors.LightBlue);
            if (c > y - 15) Graphics.SetColor(Colors.Gray2);
            Graphics.HLine(12, 22, y);
        }

        for (int y = 22; y <= 24; y++)
        {
            Graphics.SetColor(Colors.LightBlue);
            if (c > y - 15) Graphics.SetColor(Colors.Gray2);
            Graphics.HLine(12, 19, y);
        }
    }

    /// <summary>
    /// Draw air filters (lines 11300-11399)
    /// </summary>
    private void DrawFilters()
    {
        // Filter 1 (A)
        Graphics.SetColor((int)State.FilterStatus[1]);
        Graphics.HLine(6, 7, 32);
        Graphics.HLine(5, 8, 33);
        Graphics.HLine(4, 9, 34);
        Graphics.HLine(5, 8, 35);
        Graphics.HLine(6, 7, 36);

        // Filter 2 (B)
        Graphics.SetColor((int)State.FilterStatus[2]);
        Graphics.HLine(12, 13, 32);
        Graphics.HLine(11, 14, 33);
        Graphics.HLine(10, 15, 34);
        Graphics.HLine(11, 14, 35);
        Graphics.HLine(12, 13, 36);

        // Filter 3 (C)
        Graphics.SetColor((int)State.FilterStatus[3]);
        Graphics.HLine(18, 19, 32);
        Graphics.HLine(17, 20, 33);
        Graphics.HLine(16, 21, 34);
        Graphics.HLine(17, 20, 35);
        Graphics.HLine(18, 19, 36);

        // Draw soot levels
        Graphics.SetColor(Colors.Purple);
        for (int i = 1; i <= 3; i++)
        {
            int j = Math.Min(State.FilterSootLevel[i], 10);
            int x = i * 6;

            if (j >= 10) Graphics.Plot(x, 33);
            if (j >= 9) Graphics.Plot(x + 1, 33);
            if (j >= 8) Graphics.Plot(x - 1, 34);
            if (j >= 7) Graphics.Plot(x, 34);
            if (j >= 6) Graphics.Plot(x + 1, 34);
            if (j >= 5) Graphics.Plot(x + 2, 34);
            if (j >= 4) Graphics.Plot(x - 1, 35);
            if (j >= 3) Graphics.Plot(x, 35);
            if (j >= 2) Graphics.Plot(x + 1, 35);
            if (j >= 1) Graphics.Plot(x + 2, 35);
        }
    }

    /// <summary>
    /// Draw cooling tower (lines 11400-11499)
    /// </summary>
    private void DrawCoolingTower()
    {
        Graphics.SetColor(Colors.White);
        for (int x = 32; x <= 37; x++)
            Graphics.VLine(0, 25, x);

        Graphics.VLine(1, 3, 30);
        Graphics.VLine(0, 6, 31);
        Graphics.VLine(0, 6, 38);
        Graphics.VLine(1, 3, 39);
        Graphics.VLine(22, 25, 30);
        Graphics.VLine(19, 25, 31);
        Graphics.VLine(19, 25, 38);
        Graphics.VLine(22, 25, 39);
        Graphics.VLine(19, 24, 31);
        Graphics.VLine(19, 24, 38);
        Graphics.VLine(22, 24, 39);

        // Cooling tower base/water
        Graphics.SetColor(Colors.MediumBlue);
        for (int y = 26; y <= 31; y++)
            Graphics.HLine(30, 39, y);
    }

    /// <summary>
    /// Draw secondary pool (lines 11500-11599)
    /// </summary>
    private void DrawSecondaryPool()
    {
        Graphics.SetColor(Colors.DarkBlue);
        for (int y = 34; y <= 39; y++)
            Graphics.HLine(34, 39, y);
    }

    /// <summary>
    /// Draw air intake indicators (lines 11600-11699)
    /// </summary>
    private void DrawAirIntake()
    {
        Graphics.SetColor(Colors.Yellow);
        State.AirLeak = true;
        State.PipeStatus[24] = 13;

        // Check if any air intake valve is open
        if (State.ValveStatus[13] != ThreeMileIsland.ValveStatus.Open && 
            State.ValveStatus[14] != ThreeMileIsland.ValveStatus.Open && 
            State.ValveStatus[15] != ThreeMileIsland.ValveStatus.Open)
        {
            Graphics.SetColor(Colors.Gray2);
            State.AirLeak = false;
            State.PipeStatus[24] = GameEngine.Empty;
            State.FlushCountdown = GameState.FlushTime0;
        }

        Graphics.HLine(6, 8, 27);
        Graphics.HLine(6, 8, 28);
        Graphics.HLine(6, 19, 29);
        Graphics.Plot(7, 30);
        Graphics.Plot(13, 30);
        Graphics.Plot(19, 30);
    }

    /// <summary>
    /// Draw water pool at bottom (lines 11700-11720)
    /// </summary>
    private void DrawWaterPool()
    {
        Graphics.SetColor(Colors.DarkBlue);
        for (int y = 16; y <= 19; y++)
            Graphics.HLine(6, 10, y);
    }

    /// <summary>
    /// Draw pipes (lines 16100-16122)
    /// </summary>
    private void DrawPipes()
    {
        // Pipe 1 - Steam inlet
        Graphics.SetColor(State.PipeStatus[1]);
        Graphics.HLine(0, 9, 9);

        // Pipe 2 - Feedwater outlet
        Graphics.SetColor(State.PipeStatus[2]);
        Graphics.HLine(0, 3, 14);

        // Pipe 13 - Main condenser line
        Graphics.SetColor(State.PipeStatus[13]);
        Graphics.VLine(15, 24, 3);

        // Pipe 14
        Graphics.SetColor(State.PipeStatus[14]);
        Graphics.HLine(4, 9, 22);

        // Pipe 15
        Graphics.SetColor(State.PipeStatus[15]);
        Graphics.HLine(4, 9, 24);

        // Pipe 16
        Graphics.SetColor(State.PipeStatus[16]);
        Graphics.VLine(25, 34, 3);

        // Pipe 17 - Complex routing
        Graphics.SetColor(State.PipeStatus[17]);
        Graphics.VLine(19, 31, 25);
        Graphics.HLine(13, 24, 19);
        Graphics.Plot(13, 18);
        Graphics.HLine(13, 27, 17);
        Graphics.VLine(18, 25, 27);
        Graphics.HLine(27, 29, 26);

        // Pipe 18
        Graphics.SetColor(State.PipeStatus[18]);
        Graphics.VLine(25, 34, 22);

        // Pipe 19 - Air filter line
        Graphics.SetColor(State.PipeStatus[19]);
        Graphics.HLine(6, 33, 39);
        Graphics.Plot(6, 38);
        Graphics.Plot(12, 38);
        Graphics.Plot(18, 38);

        // Pipe 20
        Graphics.SetColor(State.PipeStatus[20]);
        Graphics.VLine(20, 24, 10);

        // Pipe 21
        Graphics.SetColor(State.PipeStatus[21]);
        Graphics.HLine(26, 29, 29);

        // Pipe 22
        Graphics.SetColor(State.PipeStatus[22]);
        Graphics.HLine(26, 29, 31);
    }

    /// <summary>
    /// Draw valves (lines 14100-14118)
    /// </summary>
    private void DrawValves()
    {
        // Valve 2
        Graphics.SetColor((int)State.ValveStatus[2]);
        Graphics.Plot(1, 9);

        // Valve 3
        Graphics.SetColor((int)State.ValveStatus[3]);
        Graphics.Plot(1, 14);

        // Valves 9-18
        Graphics.SetColor((int)State.ValveStatus[9]);
        Graphics.Plot(9, 22);

        Graphics.SetColor((int)State.ValveStatus[10]);
        Graphics.Plot(9, 24);

        Graphics.SetColor((int)State.ValveStatus[11]);
        Graphics.Plot(28, 29);

        Graphics.SetColor((int)State.ValveStatus[12]);
        Graphics.Plot(27, 31);

        Graphics.SetColor((int)State.ValveStatus[13]);
        Graphics.Plot(7, 31);

        Graphics.SetColor((int)State.ValveStatus[14]);
        Graphics.Plot(13, 31);

        Graphics.SetColor((int)State.ValveStatus[15]);
        Graphics.Plot(19, 31);

        Graphics.SetColor((int)State.ValveStatus[16]);
        Graphics.Plot(6, 37);

        Graphics.SetColor((int)State.ValveStatus[17]);
        Graphics.Plot(12, 37);

        Graphics.SetColor((int)State.ValveStatus[18]);
        Graphics.Plot(18, 37);
    }

    /// <summary>
    /// Draw pumps (lines 15100-15118)
    /// </summary>
    private void DrawPumps()
    {
        // Pumps G, H, I (7-9)
        Graphics.SetColor((int)State.PumpStatus[7]);
        Graphics.VLine(17, 19, 8);

        Graphics.SetColor((int)State.PumpStatus[8]);
        Graphics.VLine(17, 19, 9);

        Graphics.SetColor((int)State.PumpStatus[9]);
        Graphics.VLine(17, 19, 10);

        // Pumps J, K, L (10-12)
        Graphics.SetColor((int)State.PumpStatus[10]);
        Graphics.VLine(22, 24, 20);

        Graphics.SetColor((int)State.PumpStatus[11]);
        Graphics.VLine(22, 24, 21);

        Graphics.SetColor((int)State.PumpStatus[12]);
        Graphics.VLine(22, 24, 22);

        // Pumps M, N, O (13-15)
        Graphics.SetColor((int)State.PumpStatus[13]);
        Graphics.VLine(29, 31, 30);

        Graphics.SetColor((int)State.PumpStatus[14]);
        Graphics.VLine(29, 31, 31);

        Graphics.SetColor((int)State.PumpStatus[15]);
        Graphics.VLine(29, 31, 32);

        // Pumps P, Q, R (16-18)
        Graphics.SetColor((int)State.PumpStatus[16]);
        Graphics.VLine(37, 39, 34);

        Graphics.SetColor((int)State.PumpStatus[17]);
        Graphics.VLine(37, 39, 35);

        Graphics.SetColor((int)State.PumpStatus[18]);
        Graphics.VLine(37, 39, 36);
    }

    public override void Update()
    {
        DrawTurbines();
        DrawCondenser();
        DrawFilters();
        DrawAirIntake();
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

        if (key.Key == ConsoleKey.T)
        {
            ShowTurbineLabels();
            State.CurrentLabel = 3;
            return;
        }

        if (key.Key == ConsoleKey.F)
        {
            ShowFilterLabels();
            State.CurrentLabel = 4;
            return;
        }

        // Handle pump toggle (G-R = pumps 7-18)
        if (State.CurrentLabel == 1 && c >= 'G' && c <= 'R')
        {
            int u = c - 'A' + 1;
            TogglePump(u);
        }

        // Handle valve toggle
        if (State.CurrentLabel == 2)
        {
            int v = c - 'A' + 1;
            if (v == 2 || v == 3 || (v >= 9 && v <= 18))
                ToggleValve(v);
        }

        // Handle turbine toggle (A-D = turbines 1-4)
        if (State.CurrentLabel == 3 && c >= 'A' && c <= 'D')
        {
            int t = c - 'A' + 1;
            ToggleTurbine(t);
        }

        // Handle filter toggle (A-C = filters 1-3)
        if (State.CurrentLabel == 4 && c >= 'A' && c <= 'C')
        {
            int f = c - 'A' + 1;
            ToggleFilter(f);
        }
    }

    private void TogglePump(int u)
    {
        if (State.PumpStatus[u] == ThreeMileIsland.PumpStatus.Repair || 
            State.PumpCountdown[u] > GameState.PumpFailure1 + GameState.PumpFailure0)
            return;

        State.PumpStatus[u] = State.PumpStatus[u] == ThreeMileIsland.PumpStatus.Off 
            ? ThreeMileIsland.PumpStatus.On 
            : ThreeMileIsland.PumpStatus.Off;
        State.PumpCountdown[u] -= State.Rnd.Next(GameState.PumpAdjust1) + GameState.PumpAdjust0;
        DrawPumps();
    }

    private void ToggleValve(int v)
    {
        if (State.ValveStatus[v] == ThreeMileIsland.ValveStatus.Repair || 
            State.ValveCountdown[v] > GameState.ValveFailure1 + GameState.ValveFailure0)
            return;

        State.ValveStatus[v] = State.ValveStatus[v] == ThreeMileIsland.ValveStatus.Shut 
            ? ThreeMileIsland.ValveStatus.Open 
            : ThreeMileIsland.ValveStatus.Shut;
        State.ValveCountdown[v] -= State.Rnd.Next(GameState.ValveAdjust1) + GameState.ValveAdjust0;
        DrawValves();
    }

    private void ToggleTurbine(int t)
    {
        if (State.TurbineActive[t] == TurbineStatus.Repair || 
            State.TurbineCountdown[t] > GameState.TurbineFailure1 + GameState.TurbineFailure0)
            return;

        TurbineStatus newStatus = State.TurbineActive[t] == TurbineStatus.Offline 
            ? TurbineStatus.Online 
            : TurbineStatus.Offline;
        
        State.TurbineActive[t] = newStatus;
        
        State.TurbineCount = (State.TurbineActive[1] == TurbineStatus.Online ? 1 : 0) +
                             (State.TurbineActive[2] == TurbineStatus.Online ? 1 : 0) +
                             (State.TurbineActive[3] == TurbineStatus.Online ? 1 : 0) +
                             (State.TurbineActive[4] == TurbineStatus.Online ? 1 : 0);
        
        State.TurbineCountdown[t] -= State.Rnd.Next(GameState.TurbineAdjust1) + GameState.TurbineAdjust0;
        DrawTurbines();
    }

    private void ToggleFilter(int f)
    {
        ThreeMileIsland.FilterStatus newStatus = ThreeMileIsland.FilterStatus.Clean;
        if (State.FilterSootLevel[f] > 10 || State.FilterStatus[f] == ThreeMileIsland.FilterStatus.Clean)
            newStatus = ThreeMileIsland.FilterStatus.Dirty;
        
        State.FilterStatus[f] = newStatus;
        DrawFilters();

        State.FilterCount = (State.FilterStatus[1] == ThreeMileIsland.FilterStatus.Clean ? 1 : 0) +
                            (State.FilterStatus[2] == ThreeMileIsland.FilterStatus.Clean ? 1 : 0) +
                            (State.FilterStatus[3] == ThreeMileIsland.FilterStatus.Clean ? 1 : 0);
    }

    public override void ShowLabel()
    {
        switch (State.CurrentLabel)
        {
            case 1: ShowPumpLabels(); break;
            case 2: ShowValveLabels(); break;
            case 3: ShowTurbineLabels(); break;
            case 4: ShowFilterLabels(); break;
        }
    }

    private void ShowPumpLabels()
    {
        LowResGraphics.ClearLine(21);
        LowResGraphics.ClearLine(23);

        Console.SetCursorPosition(8, 21);
        Console.Write("GHI");
        Console.SetCursorPosition(20, 21);
        Console.Write("JKL");
        Console.SetCursorPosition(30, 21);
        Console.Write("MNO");
        Console.SetCursorPosition(34, 21);
        Console.Write("PQR");

        Console.SetCursorPosition(3, 23);
        Console.Write("TURBINE, FILTER, CONDENSER - PUMPS");
    }

    private void ShowValveLabels()
    {
        LowResGraphics.ClearLine(20);
        LowResGraphics.ClearLine(21);
        LowResGraphics.ClearLine(23);

        Console.SetCursorPosition(1, 20);
        Console.Write("B");
        Console.SetCursorPosition(7, 20);
        Console.Write("M I");
        Console.SetCursorPosition(13, 20);
        Console.Write("N");
        Console.SetCursorPosition(19, 20);
        Console.Write("O");
        Console.SetCursorPosition(28, 20);
        Console.Write("K");

        Console.SetCursorPosition(1, 21);
        Console.Write("C");
        Console.SetCursorPosition(6, 21);
        Console.Write("P  J  Q");
        Console.SetCursorPosition(18, 21);
        Console.Write("R");
        Console.SetCursorPosition(27, 21);
        Console.Write("L");

        Console.SetCursorPosition(2, 23);
        Console.Write("TURBINE, FILTER, CONDENSER - VALVES");
    }

    private void ShowTurbineLabels()
    {
        LowResGraphics.ClearLine(21);
        LowResGraphics.ClearLine(23);

        Console.SetCursorPosition(11, 21);
        Console.Write("A   B   C   D");

        Console.SetCursorPosition(2, 23);
        Console.Write("TURBINE, FILTER, CONDENSER - TURBINES");
    }

    private void ShowFilterLabels()
    {
        LowResGraphics.ClearLine(21);
        LowResGraphics.ClearLine(23);

        Console.SetCursorPosition(6, 21);
        Console.Write("A");
        Console.SetCursorPosition(12, 21);
        Console.Write("B");
        Console.SetCursorPosition(18, 21);
        Console.Write("C");

        Console.SetCursorPosition(2, 23);
        Console.Write("TURBINE, FILTER, CONDENSER - FILTERS");
    }
}
