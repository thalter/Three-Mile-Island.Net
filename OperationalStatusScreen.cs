using Microsoft.Extensions.Logging;

namespace ThreeMileIsland;

/// <summary>
/// Screen 6: Operational Status view
/// Shows all critical reactor readings and warning indicators
/// </summary>
public class OperationalStatusScreen(GameState state, LowResGraphics graphics, SoundSystem sound, ILogger<OperationalStatusScreen> logger) : GameScreen(state, graphics, sound, logger)
{
    public override void Draw()
    {
        Console.Clear();
        ShowOperationalStatus();
    }

    /// <summary>
    /// Display operational status (lines 8800-8850, 3600-3690)
    /// </summary>
    private void ShowOperationalStatus()
    {
        Console.SetCursorPosition(11, 0);
        Console.WriteLine("OPERATIONAL STATUS");

        // Check if gauges are under maintenance
        if (State.GaugeMaintenance)
        {
            ShowGaugesUnavailable();
            return;
        }

        // Display all gauges
        ShowGaugeValue(1, "CORE TEMP:", $"{State.CoreTemperature} DEG");
        ShowGaugeValue(2, "CTRL RODS:", $"{State.ControlRodTemp} DEG");
        ShowGaugeValue(3, "PCS PRES:", $"{State.BuildingBuffer[2] * 100 + 1200} PSI");
        ShowGaugeValue(4, "PMPS REQ'D:", $"{State.PumpsRequired}");
        ShowGaugeValue(5, "CNTMT PRES:", $"{State.BuildingBuffer[1]}{(State.BuildingBuffer[1] > 0 ? "00" : "")} PSI");
        ShowGaugeValue(6, "CNTMT WTR:", $"{State.BuildingBuffer[7]}{(State.BuildingBuffer[7] > 0 ? ",000" : "")} GAL");
        ShowGaugeValue(7, "PMP HSE WTR:", $"{State.BuildingBuffer[10]}{(State.BuildingBuffer[10] > 0 ? ",000" : "")} GAL");

        int rad = State.BuildingBuffer[11] % 100;
        string radStr = $"{State.BuildingBuffer[11] / 100}.{(rad < 10 ? "0" : "")}{rad} MREMS/HR";
        ShowGaugeValue(8, "PMP HSE RAD:", radStr);

        int flushTime = GameState.FlushTime0 - State.FlushCountdown;
        ShowGaugeValue(9, "FLUSH TIME:", flushTime >= 0 ? State.FormatTime(flushTime) : "N/A");

        ShowGaugeValue(10, "PRSZER WTR:", $"{State.BuildingBuffer[3]}{(State.BuildingBuffer[3] > 0 ? ",000" : "")} GAL");

        // Show warning indicators
        ShowWarningIndicators();
    }

    /// <summary>
    /// Show a gauge value with possible random error (lines 3610-3650)
    /// </summary>
    private void ShowGaugeValue(int index, string label, string value)
    {
        int row = index * 2 + 1;
        Console.SetCursorPosition(0, row);
        Console.Write(label);

        Console.SetCursorPosition(14, row);

        // Check if gauge might give wrong reading
        if (State.GaugeCountdown[index] <= 0 && State.Rnd.Next(2) == 0)
        {
            // Random reading
            Console.Write(State.Rnd.Next(100).ToString());
        }
        else
        {
            Console.Write(value);
        }
    }

    /// <summary>
    /// Show warning indicators (lines 3660-3690)
    /// </summary>
    private void ShowWarningIndicators()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Black;

        // Row 1: Containment sealed
        Console.SetCursorPosition(29, 3);
        if (State.ContainmentPressure == 32767)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("SEALED");
        }

        // Row 2: Temperature warning
        Console.SetCursorPosition(29, 5);
        if (State.CoreTemperature > GameState.TempThreshold3)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("TEMP");
        }

        // Row 3: Fuel rod damage
        Console.SetCursorPosition(29, 7);
        if (State.FuelRodDamage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("FR DAMAGE");
        }

        // Row 4: Scram
        Console.SetCursorPosition(29, 9);
        if (State.ControlRodTemp == 0)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("SCRAM");
        }

        // Row 5: ECCS and ESCS
        Console.SetCursorPosition(29, 11);
        if (State.PumpCluster1 > 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("ECCS");
        }
        Console.SetCursorPosition(34, 11);
        if (State.PumpCluster3 > 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("ESCS");
        }

        // Row 6: Radiation leak
        Console.SetCursorPosition(29, 13);
        if (State.BuildingBuffer[11] > 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("RADLEAK");
        }

        // Row 7: Filter and Air
        Console.SetCursorPosition(29, 15);
        if (State.FilterCount == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("FLTR");
        }
        Console.SetCursorPosition(34, 15);
        if (State.AirLeak)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("AIR");
        }

        // Row 8: Condenser
        Console.SetCursorPosition(29, 17);
        if (State.BuildingBuffer[6] > 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("CNDSER");
        }

        // Row 9: Steamer
        Console.SetCursorPosition(29, 19);
        if (State.BuildingBuffer[4] > 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("STMER");
        }

        // Row 10: PCS Leak
        Console.SetCursorPosition(29, 21);
        if (State.PrimaryLeak)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("PCSLEAK");
        }

        // Row 11: Power status
        Console.SetCursorPosition(29, 23);
        if (State.ElectricOutput > 0 && State.ElectricOutput < State.ElectricDemand)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("BROWNOUT");
        }
        else if (State.ElectricOutput == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("BLACKOUT");
        }

        Console.ResetColor();
    }

    /// <summary>
    /// Show message when gauges are unavailable (lines 21200-21250)
    /// </summary>
    private void ShowGaugesUnavailable()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.SetCursorPosition(0, 9);
        Console.WriteLine(GameState.Spaces.Substring(0, 20));
        Console.WriteLine(GameState.Spaces.Substring(0, 20));
        Console.WriteLine(" GAUGES UNAVAILABLE ");
        Console.WriteLine(GameState.Spaces.Substring(0, 20));
        Console.WriteLine(" DURING INSPECTION. ");
        Console.WriteLine(GameState.Spaces.Substring(0, 20));
        Console.WriteLine(GameState.Spaces.Substring(0, 20));
        Console.ResetColor();
    }

    public override void Update()
    {
        ShowOperationalStatus();
    }

    public override void HandleInput(ConsoleKeyInfo key)
    {
        // No special input handling for operational status screen
    }

    public override void ShowLabel()
    {
        // Status screen has no mode labels
    }

    public override void Render()
    {
        // Text-based screen, no graphics rendering needed
    }
}
