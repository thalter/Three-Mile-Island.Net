namespace ThreeMileIsland;

/// <summary>
/// Contains all game state variables, arrays, and constants for the Three Mile Island simulation.
/// Converted from the original Apple II Integer Basic source.
/// </summary>
public class GameState
{
    // Random number generator
    public Random Rnd { get; } = new();

    // === Game Mode and Screen ===
    public int CurrentScreen { get; set; } = -1;  // DYN - current display screen (-1=menu, 0-7=screens)
    public int CurrentLabel { get; set; } = 1;    // LAB - current label mode for input
    public bool SimulationRunning { get; set; } = true;  // SYM - simulation running flag

    // === Time and Day ===
    public int SimulationCount { get; set; } = 0;   // SCNT - simulation counter (0-1440 per day)
    public int DemandCount { get; set; }            // DCNT - demand change counter
    public int Day { get; set; } = 1;               // DAY - current day

    // === Temperature Variables ===
    public int CoreTemperature { get; set; }      // TEMP - current core temperature
    public int OldTemperature { get; set; }    // OTMP - previous temperature
    public int ControlRodTemp { get; set; }   // TMP0 - control rod temperature contribution

    // === Temperature Constants ===
    public const int TempThreshold1 = 400;   // TMP1
    public const int TempThreshold2 = 500;   // TMP2
    public const int TempThreshold3 = 600;   // TMP3
    public const int TempThreshold4 = 750;   // TMP4
    public const int TempMeltdown = 2500;    // TMPMD

    // === Counter Variable ===
    public int PumpsRequired { get; set; }   // CNT - pumps required count

    // === Valve Arrays (19 valves) ===
    public int[] ValveCountdown { get; } = new int[20];   // VC - valve countdown timers
    public ValveStatus[] ValveStatus { get; } = new ValveStatus[20];       // VA - valve status
    public int[] ValvePipe { get; } = new int[20];         // VP - valve-to-pipe mapping

    // === Pipe Array (25 pipes) ===
    public int[] PipeStatus { get; } = new int[26];        // PI - pipe status/color

    // === Pump Arrays (24 pumps) ===
    public int[] PumpCountdown { get; } = new int[25];     // UC - pump countdown timers
    public PumpStatus[] PumpStatus { get; } = new PumpStatus[25];        // PU - pump status (0=repair, 1=off, 12=on)

    // === Turbine Arrays (4 turbines) ===
    public int[] TurbineCountdown { get; } = new int[5];   // TC - turbine countdown timers
    public TurbineStatus[] TurbineActive { get; } = new TurbineStatus[5];      // TU - turbine status (0=repair, 10=off, 13=on)

    // === Filter Arrays (3 filters) ===
    public int[] FilterCondition { get; } = new int[4];    // SC - filter condition countdown
    public FilterStatus[] FilterStatus { get; } = new FilterStatus[4];       // FI - filter status (8=clean, 10=dirty)
    public int[] FilterSootLevel { get; } = new int[4];    // SL - soot level
    public int[] FilterSootPressure { get; } = new int[4]; // SP - soot pressure

    // === Gauge Arrays ===
    public int[] GaugeCountdown { get; } = new int[12];    // GC - gauge reliability countdown

    // === Buffer/Building Arrays (11 elements) ===
    public int[] BuildingBuffer { get; } = new int[12];    // BU - building state buffer
    public int[] BuildingOld { get; } = new int[12];       // BO - old building state

    // === Control Rod Arrays (18 rods) ===
    public int[] ControlRodPosition { get; } = new int[19];  // CR - rod position (0-33)
    public int[] ControlRodColor { get; } = new int[19];     // CRC - rod display color
    public int[] ControlRodClusterCount { get; } = new int[3]; // CCR - cluster count

    // === Fuel Rod Array ===
    public int[] FuelRodStatus { get; } = new int[19];     // FR - fuel rod status

    // === Control Rod Coordinates ===
    public int[] ControlRodX { get; } = new int[19];       // CX
    public int[] ControlRodY { get; } = new int[19];       // CY

    // === Filter Color Array ===
    public int[] FilterColor { get; } = new int[5];        // FCOL

    // === Pump Cluster Counts ===
    public int PumpCluster1 { get; set; } = 0;   // UC1 - ECCS pumps 1-3
    public int PumpCluster2 { get; set; } = 0;   // UC2 - pumps 4-6
    public int PumpCluster3 { get; set; } = 0;   // UC3 - ESCS pumps 7-9
    public int PumpCluster4 { get; set; } = 0;   // UC4 - pumps 10-12
    public int PumpCluster5 { get; set; } = 0;   // UC5 - pumps 13-15
    public int PumpCluster6 { get; set; } = 0;   // UC6 - pumps 16-18
    public int PumpCluster7 { get; set; } = 0;   // UC7 - pumps 19-21
    public int PumpCluster8 { get; set; } = 0;   // UC8 - pumps 22-24

    // === Financial Variables ===
    public int OperatingCost { get; set; } = 200;    // COST - operating cost (in $1000s)
    public int MaintenanceCost { get; set; } = 0;    // MAIN - maintenance cost
    public int ElectricDemand { get; set; } = 100;   // DMND - electric demand in MW
    public int ElectricOutput { get; set; } = 0;     // EOUT - electric output in MW
    public int ProjectedProfit { get; set; } = 0;    // MAXP - projected profit
    public int ActualProfit { get; set; } = 0;       // ACTP - actual profit

    // === Loss Thresholds ===
    public const int LossThreshold1 = -200;   // LOSS1 - petition threshold
    public const int LossThreshold2 = -500;   // LOSS2 - game over threshold

    // === Pressure and Damage ===
    public int ContainmentPressure { get; set; } = 50;  // CP - containment pressure
    public bool FuelRodDamage { get; set; } = false;    // FRDMG
    public bool AirLeak { get; set; } = false;          // AIR
    public int LeakThreshold { get; set; } = 100;       // LK0
    public bool PrimaryLeak { get; set; } = false;      // LEAK

    // === Radiation ===
    public int RadiationLevel1 { get; set; } = 50;      // RAD1 - radiation warning level
    public const int RadDamage1 = 75;                   // RD1
    public const int RadDamage5 = 25;                   // RD5

    // === Filter and Turbine Counts ===
    public int FilterCount { get; set; } = 0;           // FC - active filter count
    public int TurbineCount { get; set; } = 0;          // TCNT - active turbine count
    public int FlushCountdown { get; set; } = 120;      // FCNT - flush countdown
    public const int FlushTime0 = 120;                  // FF0

    // === Valve/Pump Failure Constants ===
    public const int ValveFailure0 = 250;    // VF0
    public const int ValveFailure1 = 100;    // VF1
    public const int ValveAdjust0 = 5;       // VA0
    public const int ValveAdjust1 = 5;       // VA1
    public const int ValveRepair0 = 25;      // VR0
    public const int ValveRepair1 = 25;      // VR1

    public const int PumpFailure0 = 500;     // UF0
    public const int PumpFailure1 = 100;     // UF1
    public const int PumpAdjust0 = 5;        // UA0
    public const int PumpAdjust1 = 5;        // UA1
    public const int PumpRepair0 = 50;       // UR0
    public const int PumpRepair1 = 25;       // UR1

    public const int TurbineFailure0 = 2760; // TF0
    public const int TurbineFailure1 = 120;  // TF1
    public const int TurbineAdjust0 = 10;    // TA0
    public const int TurbineAdjust1 = 10;    // TA1
    public const int TurbineRepair0 = 100;   // TR0
    public const int TurbineRepair1 = 100;   // TR1

    public const int SootRepair0 = 25;       // SR0
    public const int SootRepair1 = 25;       // SR1

    public const int DemandChange0 = 50;     // DC0
    public const int DemandChange1 = 15;     // DC1
    public const int DemandMod0 = 75;        // DM0
    public const int DemandMod1 = 25;        // DM1

    public const int ValveMaint0 = 1;        // VM0
    public const int ValveMaint1 = 1;        // VM1
    public const int PumpMaint0 = 2;         // UM0
    public const int PumpMaint1 = 2;         // UM1
    public const int TurbineMaint0 = 5;      // TM0
    public const int TurbineMaint1 = 5;      // TM1

    public const int GaugeCheck0 = 750;      // GC0
    public const int GaugeCheck1 = 50;       // GC1
    public const int GaugeMaint0 = 10;       // GM0
    public const int GaugeMaint1 = 10;       // GM1

    // === Gauge Maintenance ===
    public bool GaugeMaintenance { get; set; } = false;   // GMAIN
    public int DefectiveGauges { get; set; } = 0;         // NDG

    // === Control Rod Position ===
    public int RodPosition { get; set; } = 9;             // RPOS - current selected rod

    // === Emergency Counters ===
    public int SafetyDirectiveNumber { get; set; } = 0;   // SDN
    public int EmergencyNoticeNumber { get; set; } = 0;   // ENN

    // === Flag Variables ===
    public bool ValveFlag { get; set; } = false;    // VFLG - valve failure check
    public bool PumpFlag { get; set; } = false;     // UFLG - pump failure check
    public bool TurbineFlag { get; set; } = false;  // TFLG - turbine failure check
    public bool FilterFlag { get; set; } = false;   // FFLG - filter check
    public bool EmergencyFlag { get; set; } = false; // EFLG - emergency flag

    // === Command Processing ===
    public string CommandString { get; set; } = "";  // CMMD$
    public bool CommandMode { get; set; } = false;   // CMMD
    public int CommandChar { get; set; } = 1;        // CHAR
    public int CommandCount { get; set; } = 0;       // CCNT

    // === String Constants ===
    public const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string LineSeparator = "--------------------------------------";
    public const string Spaces = "                                      ";

    /// <summary>
    /// Initialize all game state to starting values
    /// </summary>
    public void Initialize()
    {
        // Initialize valve-to-pipe mapping (VP array from lines 32200-32209)
        ValvePipe[1] = 0; ValvePipe[2] = 1;
        ValvePipe[3] = 2; ValvePipe[4] = 11;
        ValvePipe[5] = 5; ValvePipe[6] = 6;
        ValvePipe[7] = 7; ValvePipe[8] = 12;
        ValvePipe[9] = 14; ValvePipe[10] = 15;
        ValvePipe[11] = 21; ValvePipe[12] = 22;
        ValvePipe[13] = 24; ValvePipe[14] = 24;
        ValvePipe[15] = 24; ValvePipe[16] = 19;
        ValvePipe[17] = 19; ValvePipe[18] = 19;
        ValvePipe[19] = 23;

        // Initialize control rod X coordinates (lines 32210-32215)
        ControlRodX[1] = 10; ControlRodX[2] = 12; ControlRodX[3] = 11;
        ControlRodX[4] = 10; ControlRodX[5] = 12; ControlRodX[6] = 11;
        ControlRodX[7] = 10; ControlRodX[8] = 12; ControlRodX[9] = 11;
        ControlRodX[10] = 14; ControlRodX[11] = 13; ControlRodX[12] = 15;
        ControlRodX[13] = 14; ControlRodX[14] = 13; ControlRodX[15] = 15;
        ControlRodX[16] = 14; ControlRodX[17] = 13; ControlRodX[18] = 15;

        // Initialize control rod Y coordinates (lines 32220-32225)
        ControlRodY[1] = 19; ControlRodY[2] = 19; ControlRodY[3] = 20;
        ControlRodY[4] = 21; ControlRodY[5] = 21; ControlRodY[6] = 22;
        ControlRodY[7] = 23; ControlRodY[8] = 23; ControlRodY[9] = 24;
        ControlRodY[10] = 19; ControlRodY[11] = 20; ControlRodY[12] = 20;
        ControlRodY[13] = 21; ControlRodY[14] = 22; ControlRodY[15] = 22;
        ControlRodY[16] = 23; ControlRodY[17] = 24; ControlRodY[18] = 24;

        // Initialize filter colors (line 32600-32601)
        FilterColor[1] = 10; FilterColor[2] = 7;
        FilterColor[3] = 6; FilterColor[4] = 2;

        // Initialize pipes (line 32500-32502)
        for (int p = 1; p <= 25; p++)
        {
            PipeStatus[p] = GameEngine.Empty; // All pipes start EMPTY
        }

        // Initialize pumps (lines 32504-32508)
        for (int u = 1; u <= 24; u++)
        {
            PumpCountdown[u] = Rnd.Next(PumpFailure1) + PumpFailure0;
            PumpStatus[u] = ThreeMileIsland.PumpStatus.Off;  // All pumps start OFF
        }

        // Initialize valves (lines 32510-32514)
        for (int v = 1; v <= 19; v++)
        {
            ValveCountdown[v] = Rnd.Next(ValveFailure1) + ValveFailure0;
            ValveStatus[v] = ThreeMileIsland.ValveStatus.Shut;  // All valves start SHUT
        }

        // Initialize building buffers (lines 32516-32518)
        for (int b = 1; b <= 11; b++)
            BuildingBuffer[b] = 0;

        // Initialize filters (lines 32520-32524)
        for (int f = 1; f <= 3; f++)
        {
            FilterStatus[f] = ThreeMileIsland.FilterStatus.Dirty;  // Dirty
            FilterSootLevel[f] = 0;
            FilterCondition[f] = Rnd.Next(SootRepair1) + SootRepair0;
        }

        // Initialize turbines (lines 32526-32530)
        for (int t = 1; t <= 4; t++)
        {
            TurbineCountdown[t] = Rnd.Next(TurbineFailure1) + TurbineFailure0;
            TurbineActive[t] = TurbineStatus.Offline;  // All turbines start OFFLINE
        }

        // Initialize control rods (lines 32532-32534)
        for (int r = 1; r <= 18; r++)
        {
            ControlRodPosition[r] = 0;
            ControlRodColor[r] = 12;  // Green (inserted)
        }

        // Initialize fuel rods (lines 32536-32537)
        for (int r = 1; r <= 18; r++)
            FuelRodStatus[r] = 10;

        // Initialize gauges (lines 32538-32540)
        for (int g = 1; g <= 11; g++)
            GaugeCountdown[g] = GaugeCheck0 + Rnd.Next(GaugeCheck1);

        // Set initial building state (lines 32604-32606)
        BuildingBuffer[3] = 6;   // Pressurizer water level
        BuildingBuffer[2] = 12;  // PCS pressure

        // Initialize demand countdown
        DemandCount = DemandChange0 + Rnd.Next(DemandChange1) * (Rnd.Next(3) - 1);
    }

    /// <summary>
    /// Copy BuildingBuffer to BuildingOld for change detection
    /// </summary>
    public void UpdateBuildingOld()
    {
        for (int i = 1; i <= 11; i++)
            BuildingOld[i] = BuildingBuffer[i];
    }

    /// <summary>
    /// Get letter for pump/valve/turbine index
    /// </summary>
    public static char GetLetter(int index) => index >= 1 && index <= 26 ? Letters[index - 1] : '?';

    /// <summary>
    /// Format time from simulation count
    /// </summary>
    public string FormatTime(int count, bool includeDay = false)
    {
        int d = Day;
        int c = count;
        if (c >= 1440)
        {
            d++;
            c -= 1440;
        }
        int hours = c / 60;
        int minutes = c % 60;

        if (includeDay)
            return $"{d}/{hours:D2}:{minutes:D2}";
        return $"{hours:D2}:{minutes:D2}";
    }
}

/// <summary>
/// Represents the status of a turbine
/// </summary>
public enum TurbineStatus
{
    /// <summary>Under repair</summary>
    Repair = 0,

    /// <summary>Offline but functional</summary>
    Offline = 10,

    /// <summary>Online and generating power</summary>
    Online = 13
}

/// <summary>
/// Represents the status of an air filter
/// </summary>
public enum FilterStatus
{
    /// <summary>Filter is clean</summary>
    Clean = 8,

    /// <summary>Filter is dirty</summary>
    Dirty = 10
}

/// <summary>
/// Represents the status of a valve
/// </summary>
public enum ValveStatus
{
    /// <summary>Under repair</summary>
    Repair = 0,

    /// <summary>Valve is shut/closed</summary>
    Shut = 1,

    /// <summary>Valve is open</summary>
    Open = 12
}

/// <summary>
/// Represents the status of a pump
/// </summary>
public enum PumpStatus
{
    /// <summary>Under repair</summary>
    Repair = 0,

    /// <summary>Pump is off</summary>
    Off = 1,

    /// <summary>Pump is on</summary>
    On = 12
}