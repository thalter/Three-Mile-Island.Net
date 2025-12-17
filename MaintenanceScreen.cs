using Microsoft.Extensions.Logging;

namespace ThreeMileIsland;

/// <summary>
/// Screen 4: Maintenance Schedule view
/// Shows status of pumps, valves, and turbines with repair times
/// </summary>
public class MaintenanceScreen(GameState state, LowResGraphics graphics, SoundSystem sound, ILogger<MaintenanceScreen> logger) : GameScreen(state, graphics, sound, logger)
{
    public override void Draw()
    {
        Console.Clear();
        ShowPumpSchedule();
    }

    /// <summary>
    /// Show pump maintenance schedule (lines 8600-8605)
    /// </summary>
    private void ShowPumpSchedule()
    {
        Console.Clear();
        Console.WriteLine("MAINTENANCE SCHEDULE FOR PUMPS");
        Console.WriteLine();
        Console.WriteLine("PU/STATUS     TIME  PU/STATUS     TIME");
        Console.WriteLine(GameState.LineSeparator.Substring(0, 18) + "  " + GameState.LineSeparator.Substring(0, 18));

        for (int u = 1; u <= 24; u++)
        {
            ShowPumpStatus(u);
        }
    }

    /// <summary>
    /// Show valve maintenance schedule (lines 8610-8619)
    /// </summary>
    private void ShowValveSchedule()
    {
        Console.Clear();
        Console.WriteLine("MAINTENANCE SCHEDULE FOR VALVES");
        Console.WriteLine();
        Console.WriteLine("VA/STATUS     TIME  VA/STATUS     TIME");
        Console.WriteLine(GameState.LineSeparator.Substring(0, 18) + "  " + GameState.LineSeparator.Substring(0, 18));

        for (int v = 1; v <= 19; v++)
        {
            ShowValveStatus(v);
        }
    }

    /// <summary>
    /// Show turbine maintenance schedule (lines 8620-8624)
    /// </summary>
    private void ShowTurbineSchedule()
    {
        Console.Clear();
        Console.WriteLine("MAINTENANCE SCHEDULE FOR TURBINES");
        Console.WriteLine();
        Console.WriteLine("TU/STATUS       TIME");
        Console.WriteLine(GameState.LineSeparator.Substring(0, 20));

        for (int t = 1; t <= 4; t++)
        {
            ShowTurbineStatus(t);
        }
    }

    /// <summary>
    /// Show individual pump status (lines 2250-2270)
    /// </summary>
    private void ShowPumpStatus(int u)
    {
        int c1 = (u > 12) ? 20 : 0;
        int row = 4 + (u - 1) % 12;

        Console.SetCursorPosition(c1, row);
        // Clear the area
        Console.Write(new string(' ', 18));
        Console.SetCursorPosition(c1, row);

        Console.Write($"{GameState.GetLetter(u)}  ");

        if (State.PumpCountdown[u] >= GameState.PumpFailure0 + GameState.PumpFailure1)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write("FAILED");
            Console.ResetColor();
            return;
        }

        if (State.PumpStatus[u] == ThreeMileIsland.PumpStatus.Off)
        {
            Console.Write("OFF");
            return;
        }

        if (State.PumpStatus[u] == ThreeMileIsland.PumpStatus.On)
        {
            Console.Write("ON");
            int c = State.SimulationCount + State.PumpCountdown[u] / State.PumpsRequired;
            Console.SetCursorPosition(11 + c1, row);
            Console.Write(State.FormatTime(c, true));
            return;
        }

        // Repair status
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("REPAIR");
        Console.ResetColor();
        int repairTime = State.SimulationCount + State.PumpCountdown[u];
        Console.SetCursorPosition(11 + c1, row);
        Console.Write(State.FormatTime(repairTime, true));
    }

    /// <summary>
    /// Show individual valve status (lines 2550-2570)
    /// </summary>
    private void ShowValveStatus(int v)
    {
        int c1 = (v > 10) ? 20 : 0;
        int row = 4 + (v - 1) % 10;

        Console.SetCursorPosition(c1, row);
        Console.Write(new string(' ', 18));
        Console.SetCursorPosition(c1, row);

        Console.Write($"{GameState.GetLetter(v)}  ");

        if (State.ValveCountdown[v] >= GameState.ValveFailure0 + GameState.ValveFailure1)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write("FAILED");
            Console.ResetColor();
            return;
        }

        if (State.ValveStatus[v] == ThreeMileIsland.ValveStatus.Shut)
        {
            Console.Write("SHUT");
            return;
        }

        if (State.ValveStatus[v] == ThreeMileIsland.ValveStatus.Open)
        {
            Console.Write("OPEN");
            int c = State.SimulationCount + State.ValveCountdown[v] / State.PumpsRequired;
            Console.SetCursorPosition(11 + c1, row);
            Console.Write(State.FormatTime(c, true));
            return;
        }

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("REPAIR");
        Console.ResetColor();
        int repairTime = State.SimulationCount + State.ValveCountdown[v];
        Console.SetCursorPosition(11 + c1, row);
        Console.Write(State.FormatTime(repairTime, true));
    }

    /// <summary>
    /// Show individual turbine status (lines 3450-3490)
    /// </summary>
    private void ShowTurbineStatus(int t)
    {
        int row = 4 + t;

        Console.SetCursorPosition(0, row);
        Console.Write(new string(' ', 20));
        Console.SetCursorPosition(0, row);

        Console.Write($"{GameState.GetLetter(t)}  ");

        if (State.TurbineCountdown[t] >= GameState.TurbineFailure0 + GameState.TurbineFailure1)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write("FAILED");
            Console.ResetColor();
            return;
        }

        if (State.TurbineActive[t] == TurbineStatus.Offline)
        {
            Console.Write("OFF LINE");
            return;
        }

        if (State.TurbineActive[t] == TurbineStatus.Online)
        {
            Console.Write("ON LINE");
            int c = State.SimulationCount + State.TurbineCountdown[t] / State.PumpsRequired;
            Console.SetCursorPosition(13, row);
            Console.Write(State.FormatTime(c, true));
            return;
        }

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("REPAIR");
        Console.ResetColor();
        int repairTime = State.SimulationCount + State.TurbineCountdown[t];
        Console.SetCursorPosition(13, row);
        Console.Write(State.FormatTime(repairTime, true));
    }

    public override void Update()
    {
        // Refresh current schedule view
        switch (State.CurrentLabel)
        {
            case 1:
                for (int u = 1; u <= 24; u++)
                    ShowPumpStatus(u);
                break;
            case 2:
                for (int v = 1; v <= 19; v++)
                    ShowValveStatus(v);
                break;
            case 3:
                for (int t = 1; t <= 4; t++)
                    ShowTurbineStatus(t);
                break;
        }
    }

    public override void HandleInput(ConsoleKeyInfo key)
    {
        char c = char.ToUpper(key.KeyChar);

        if (key.Key == ConsoleKey.P)
        {
            ShowPumpSchedule();
            State.CurrentLabel = 1;
            return;
        }

        if (key.Key == ConsoleKey.V)
        {
            ShowValveSchedule();
            State.CurrentLabel = 2;
            return;
        }

        if (key.Key == ConsoleKey.T)
        {
            ShowTurbineSchedule();
            State.CurrentLabel = 3;
            return;
        }

        // Repair pump (A-X)
        if (State.CurrentLabel == 1 && c >= 'A' && c <= 'X')
        {
            int u = c - 'A' + 1;
            RepairPump(u);
            ShowPumpStatus(u);
        }

        // Repair valve (A-S)
        if (State.CurrentLabel == 2 && c >= 'A' && c <= 'S')
        {
            int v = c - 'A' + 1;
            if (v <= 19)
            {
                RepairValve(v);
                ShowValveStatus(v);
            }
        }

        // Repair turbine (A-D)
        if (State.CurrentLabel == 3 && c >= 'A' && c <= 'D')
        {
            int t = c - 'A' + 1;
            RepairTurbine(t);
            ShowTurbineStatus(t);
        }
    }

    private void RepairPump(int u)
    {
        if (State.PumpStatus[u] == ThreeMileIsland.PumpStatus.Repair) return;

        State.PumpStatus[u] = ThreeMileIsland.PumpStatus.Repair;
        State.PumpCountdown[u] = State.Rnd.Next(GameState.PumpRepair1) + GameState.PumpRepair0;
        State.MaintenanceCost += State.Rnd.Next(GameState.PumpMaint1) + GameState.PumpMaint0;
    }

    private void RepairValve(int v)
    {
        if (State.ValveStatus[v] == ThreeMileIsland.ValveStatus.Repair) return;

        State.ValveStatus[v] = ThreeMileIsland.ValveStatus.Repair;
        State.ValveCountdown[v] = State.Rnd.Next(GameState.ValveRepair1) + GameState.ValveRepair0;
        State.MaintenanceCost += GameState.ValveMaint0 + State.Rnd.Next(GameState.ValveMaint1);
    }

    private void RepairTurbine(int t)
    {
        if (State.TurbineActive[t] == TurbineStatus.Repair) return;

        State.TurbineActive[t] = TurbineStatus.Repair;
        State.TurbineCountdown[t] = State.Rnd.Next(GameState.TurbineRepair1) + GameState.TurbineRepair0;
        State.MaintenanceCost += GameState.TurbineMaint0 + State.Rnd.Next(GameState.TurbineMaint1);

        State.TurbineCount = (State.TurbineActive[1] == TurbineStatus.Online ? 1 : 0) +
                             (State.TurbineActive[2] == TurbineStatus.Online ? 1 : 0) +
                             (State.TurbineActive[3] == TurbineStatus.Online ? 1 : 0) +
                             (State.TurbineActive[4] == TurbineStatus.Online ? 1 : 0);
    }

    public override void ShowLabel()
    {
        switch (State.CurrentLabel)
        {
            case 1:
                ShowPumpSchedule();
                break;
            case 2:
                ShowValveSchedule();
                break;
            case 3:
                ShowTurbineSchedule();
                break;
            default:
                ShowPumpSchedule();
                State.CurrentLabel = 1;
                break;
        }
    }

    public override void Render()
    {
        // Text-based screen, no graphics rendering needed
    }
}
